using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.TestService.Common.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.TestService.Data.Customers;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomersData(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPostgresDbFacade<CustomersDataConnectionOptions>(configuration.GetSection("Customers"))
            .AddScoped<ICustomersRepository, CustomersRepository>();
    }
}
