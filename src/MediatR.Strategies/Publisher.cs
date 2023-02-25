namespace MediatR.Strategies;

public class Publisher : IPublisher
{
    public readonly IDictionary<PublishStrategy, IMediator> PublishStrategies;

    private readonly IServiceProvider _serviceFactory;

    public Publisher(IServiceProvider serviceFactory)
    {
        _serviceFactory = serviceFactory;

        PublishStrategies = new Dictionary<PublishStrategy, IMediator>
        {
            [PublishStrategy.Async] = new CustomMediator(_serviceFactory, AsyncContinueOnException),
            [PublishStrategy.ParallelNoWait] = new CustomMediator(_serviceFactory, ParallelNoWait),
            [PublishStrategy.ParallelWhenAll] = new CustomMediator(_serviceFactory, ParallelWhenAll),
            [PublishStrategy.ParallelWhenAny] = new CustomMediator(_serviceFactory, ParallelWhenAny),
            [PublishStrategy.SyncContinueOnException] = new CustomMediator(_serviceFactory, SyncContinueOnException),
            [PublishStrategy.SyncStopOnException] = new CustomMediator(_serviceFactory, SyncStopOnException)
        };
    }

    public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken) where TNotification : INotification
        => Publish((object)notification, strategy, cancellationToken);

    public Task Publish(object notification, PublishStrategy strategy = PublishStrategy.SyncContinueOnException, CancellationToken cancellationToken = default)
    {
        if (notification is not INotification)
        {
            throw new ArgumentException($"The object does not implement the {nameof(INotification)} interface");
        }

        if (!PublishStrategies.TryGetValue(strategy, out var mediator))
        {
            throw new ArgumentException($"Unknown strategy: {strategy}");
        }

        return mediator.Publish(notification, cancellationToken);
    }

    private Task ParallelWhenAll(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        foreach (var handler in handlers)
        {
            tasks.Add(Task.Run(() => handler.HandlerCallback(notification, cancellationToken), cancellationToken));
        }

        return Task.WhenAll(tasks);
    }

    private Task ParallelWhenAny(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        foreach (var handler in handlers)
        {
            tasks.Add(Task.Run(() => handler.HandlerCallback(notification, cancellationToken), cancellationToken));
        }

        return Task.WhenAny(tasks);
    }

    private Task ParallelNoWait(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlers)
        {
            Task.Run(() => handler.HandlerCallback(notification, cancellationToken), cancellationToken);
        }

        return Task.CompletedTask;
    }

    private async Task AsyncContinueOnException(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();
        var exceptions = new List<Exception>();

        foreach (var handler in handlers)
        {
            try
            {
                tasks.Add(handler.HandlerCallback(notification, cancellationToken));
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }
        }

        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (AggregateException ex)
        {
            exceptions.AddRange(ex.Flatten().InnerExceptions);
        }
        catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
        {
            exceptions.Add(ex);
        }

        if (exceptions.Any())
        {
            throw new AggregateException(exceptions);
        }
    }

    private async Task SyncStopOnException(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlers)
        {
            await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task SyncContinueOnException(IEnumerable<NotificationHandlerExecutor> handlers, INotification notification, CancellationToken cancellationToken)
    {
        var exceptions = new List<Exception>();

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Any())
        {
            throw new AggregateException(exceptions);
        }
    }
}
