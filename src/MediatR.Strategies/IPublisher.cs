namespace MediatR.Strategies;

public interface IPublisher
{
    Task Publish<TNotification>(TNotification notification, PublishStrategy strategy = PublishStrategy.SyncContinueOnException, CancellationToken cancellationToken = default) where TNotification : INotification;

    Task Publish(object notification, PublishStrategy strategy = PublishStrategy.SyncContinueOnException, CancellationToken cancellationToken = default);
}
