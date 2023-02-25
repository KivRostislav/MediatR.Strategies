using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Strategies;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPublisher(this IServiceCollection services, Action<MediatRServiceConfiguration> configuration)
    {
        services.AddMediatR(configuration);
        services.AddSingleton<IPublisher>((provider) => new Publisher(provider));

        return services;
    }
}
