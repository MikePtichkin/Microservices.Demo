using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.TestService.Data;
using Microservices.Demo.TestService.Data.Customers;
using Microservices.Demo.TestService.Data.Orders;

namespace Microservices.Demo.TestService.AppStart;

[ExcludeFromCodeCoverage]
public static class DataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMismatchDataServices(configuration)
            .AddCustomersData(configuration)
            .AddOrdersData(configuration);
    }
}
