using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MediatR.Strategies.Tests;

public class PublisherTests
{
    public class PingNotification : INotification
    {
        public string Message { get; set; } = string.Empty;
    }

    public class PingNotificationHandler : INotificationHandler<PingNotification>
    {
        private readonly Logger _logger;

        public PingNotificationHandler(Logger logger) => _logger = logger;

        public Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            _logger.Logs.Add($"Ping {notification.Message}");
            return Task.CompletedTask;
        }
    }

    [Theory]
    [InlineData(PublishStrategy.Async)]
    [InlineData(PublishStrategy.ParallelWhenAll)]
    [InlineData(PublishStrategy.ParallelWhenAny)]
    [InlineData(PublishStrategy.SyncStopOnException)]
    [InlineData(PublishStrategy.SyncContinueOnException)]
    public async void Should_publish_specific_notification_to_all_handlers(PublishStrategy strategy)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(new Logger());
        services.AddSingleton<IPublisher>((provider) => new Publisher(provider));
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining<PublisherTests>();
        });

        IServiceProvider provider = services.BuildServiceProvider();
        IPublisher publisher = provider.GetRequiredService<IPublisher>();


        await publisher.Publish(new PingNotification() { Message = "Pong" }, strategy);


        Logger logger = provider.GetRequiredService<Logger>();

        logger.Logs.Count.ShouldBe(1);
        logger.Logs.ShouldContain("Ping Pong");
    }

    [Theory]
    [InlineData(PublishStrategy.Async)]
    [InlineData(PublishStrategy.ParallelWhenAll)]
    [InlineData(PublishStrategy.ParallelWhenAny)]
    [InlineData(PublishStrategy.SyncStopOnException)]
    [InlineData(PublishStrategy.SyncContinueOnException)]
    public async void Should_publish_notification_as_object_to_all_handlers(PublishStrategy strategy)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(new Logger());
        services.AddSingleton<IPublisher>((provider) => new Publisher(provider));
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining<PublisherTests>();
        });

        IServiceProvider provider = services.BuildServiceProvider();
        IPublisher publisher = provider.GetRequiredService<IPublisher>();


        await publisher.Publish(new PingNotification() { Message = "Pong" } as object, strategy);


        Logger logger = provider.GetRequiredService<Logger>();

        logger.Logs.Count.ShouldBe(1);
        logger.Logs.ShouldContain("Ping Pong");
    }

    [Fact]
    public async void Should_publish_specific_notification_to_all_handlers_with_parallel_no_wait_strategy()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(new Logger());
        services.AddSingleton<IPublisher>((provider) => new Publisher(provider));
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining<PublisherTests>();
        });

        IServiceProvider provider = services.BuildServiceProvider();
        IPublisher publisher = provider.GetRequiredService<IPublisher>();


        await publisher.Publish(new PingNotification() { Message = "Pong" }, PublishStrategy.ParallelNoWait);


        // We look forward to the completion of all tasks.
        Thread.Sleep(1000);
        Logger logger = provider.GetRequiredService<Logger>();

        logger.Logs.Count.ShouldBe(1);
        logger.Logs.ShouldContain("Ping Pong");
    }

    [Fact]
    public async void Should_publish_notification_as_object_to_all_handlers_with_parallel_no_wait_strategy()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(new Logger());
        services.AddSingleton<IPublisher>((provider) => new Publisher(provider));
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining<PublisherTests>();
        });

        IServiceProvider provider = services.BuildServiceProvider();
        IPublisher publisher = provider.GetRequiredService<IPublisher>();


        await publisher.Publish(new PingNotification() { Message = "Pong" } as object, PublishStrategy.ParallelNoWait);


        // We look forward to the completion of all tasks.
        Thread.Sleep(1000);
        Logger logger = provider.GetRequiredService<Logger>();

        logger.Logs.Count.ShouldBe(1);
        logger.Logs.ShouldContain("Ping Pong");
    }
}
