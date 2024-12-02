using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microservices.Demo.ViewOrder.ShardConfiguration;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;

public class ShardedMigrationRunner
{
    private static int _bucketsCount = 0;
    
    public static void MigrateUp(IReadOnlyList<DbEndpoint> endpoints)
    {
        _bucketsCount = endpoints
            .SelectMany(x => x.Buckets)
            .Count();
        
        foreach (var endpoint in endpoints)
        {
            foreach (var bucketId in endpoint.Buckets)
            {
                var serviceProvider = CreateServices(
                    endpoint,
                    bucketId);
                using var scope = serviceProvider.CreateScope();

                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }
    
    private static IServiceProvider CreateServices(
        DbEndpoint endpoint,
        int bucketId)
    {
        var connectionString = GetConnectionString(endpoint);
        
        var services = new ServiceCollection();
        var provider = services
            .AddScoped<IBucketMigrationContext>(_ => new BucketMigrationContext(endpoint, bucketId))
            .AddSingleton<IConventionSet>(new DefaultConventionSet(null, null))
            .AddSingleton<IShardingRule<long>>(new LongShardingRule(_bucketsCount))
            .AddFluentMigratorCore()
            .ConfigureRunner(builder => builder
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new MigrationRunnerConventions())
                .WithMigrationsIn(typeof(ShardedMigrationRunner).Assembly)
                .ScanIn(typeof(ShardVersionTableMetaData).Assembly).For.VersionTableMetaData()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();

        return provider;
    }
    
    private static string GetConnectionString(
        DbEndpoint endpoint)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host     = endpoint.ConnectionString.HostAndPort,
            Database = endpoint.ConnectionString.Database,
            Username = endpoint.ConnectionString.User,
            Password = endpoint.ConnectionString.Password
        };
        return builder.ToString();
    }
}