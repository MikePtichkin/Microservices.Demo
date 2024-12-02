using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.ClientBalance.Bll.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}
