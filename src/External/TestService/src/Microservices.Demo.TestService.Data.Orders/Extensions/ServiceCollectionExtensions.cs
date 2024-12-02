using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.TestService.Common.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Data.Orders;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrdersData(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPostgresDbFacade<OrdersDataConnectionOptions>(configuration.GetSection("Orders"))
            .AddScoped<IOrdersRepository, OrdersRepository>();
    }
}
