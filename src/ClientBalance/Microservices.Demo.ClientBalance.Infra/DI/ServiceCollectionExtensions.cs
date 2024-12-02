using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Domain.Users;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using Microservices.Demo.ClientBalance.Infra.Options;
using Microservices.Demo.ClientBalance.Infra.Repositories;
using System;

namespace Microservices.Demo.ClientBalance.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DEMO_CLIENT_BALANCE_SERVICE_DB_CONNECTION_STRING")
            ?? configuration.GetConnectionString("ClientBalanceServiceDb");
        services.Configure<ConnectionStrings>(opts => opts.ClientBalanceServiceDb = connectionString!);

        services.AddMigrationRunner(connectionString!);

        services.AddTransient<IUsersRepository, UserRepository>();
        services.AddTransient<IOperationsRepository, OperationsRepository>();

        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();

        return services;
    }

    private static void AddMigrationRunner(
        this IServiceCollection services,
        string connectionString) => services
        .AddFluentMigratorCore()
        .ConfigureRunner(builder => builder
            .AddPostgres()
            .ScanIn(typeof(ServiceCollectionExtensions).Assembly)
            .For
            .Migrations())
        .AddOptions<ProcessorOptions>()
        .Configure(options =>
        {
            options.ProviderSwitches = "Force Quote=false";
            options.Timeout = TimeSpan.FromMinutes(10);
            options.ConnectionString = connectionString;
        });
}
