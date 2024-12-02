using System.Reflection;

using FluentMigrator.Runner;

using Microservices.Demo.CustomerService.Infrastructure.Migrations.Database;
using Microservices.Demo.CustomerService.Infrastructure.PostgresRepositories;
using Microservices.Demo.CustomerService.Repositories;

namespace Microservices.Demo.CustomerService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddMigration();

        return services;
    }

    private static IServiceCollection AddMigration(this IServiceCollection services)
    {
        var connectionString = ConnectionStringProvider.GetConnectionString()!;
        return services.AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                x => x.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly())
                    .For.Migrations());
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        return services;
    }
}