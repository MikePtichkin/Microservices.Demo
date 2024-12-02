using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.DataGenerator.Bll.Mediator.Commands;
using Microservices.Demo.DataGenerator.Bll.Services;
using Microservices.Demo.DataGenerator.Bll.Services.Contracts;

namespace Microservices.Demo.DataGenerator.Bll.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        return services
            .AddServices()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GenerateOrdersCommand).Assembly));
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICustomerService, Services.CustomerService>()
            .AddTransient<IOrderService, OrderService>()
            .AddTransient<IBrokenOrderService, BrokenOrderService>();
    }
}
