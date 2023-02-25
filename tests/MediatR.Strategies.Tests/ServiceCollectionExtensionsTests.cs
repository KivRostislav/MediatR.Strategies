using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MediatR.Strategies.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void Should_register_Publisher_service()
    {
        IServiceCollection services = new ServiceCollection();


        services.AddPublisher((config) => config.RegisterServicesFromAssemblyContaining<ServiceCollectionExtensionsTests>());


        IServiceProvider provider = services.BuildServiceProvider();
        IPublisher publisher = provider.GetRequiredService<IPublisher>();
        publisher.ShouldBeOfType<Publisher>();
    }
}
