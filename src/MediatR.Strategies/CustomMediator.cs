namespace MediatR.Strategies;

public class CustomMediator : Mediator
{
    private readonly Func<IEnumerable<NotificationHandlerExecutor>, INotification, CancellationToken, Task> _publisher;

    public CustomMediator(IServiceProvider serviceProvider, Func<IEnumerable<NotificationHandlerExecutor>, INotification, CancellationToken, Task> publisher) : base(serviceProvider) => _publisher = publisher;

    protected override Task PublishCore(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken) =>
        _publisher.Invoke(handlerExecutors, notification, cancellationToken);
}
