using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;

namespace Microservices.Demo.ViewOrder.Infra.UnitTests.Fixtures;

public class IntegrationTestFixture
{
    public IOrderRepository OrderRepository { get; }

    public IntegrationTestFixture()
    {
        var config = ShardConfiguration.ConfigurationBuilder.Build(
            10,
            "Host=localhost;Port=15434;Database=view-order-service-db;Username=postgres;Password=postgres;",
            "Host=localhost;Port=25434;Database=view-order-service-db;Username=postgres;Password=postgres;"
            );

        ShardedMigrationRunner.MigrateUp(config.Endpoints);

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var services = new ServiceCollection();
        var provider = services
            .AddSingleton<IDbStore>(new DbStore([.. config.Endpoints]))
            .AddSingleton<IShardingRule<long>>(new LongShardingRule(config.BucketsCount))
            .AddScoped<IShardConnectionFactory, ShardConnectionFactory>()
            .AddScoped<IOrderRepository, Dal.Repositories.OrderRepository>()
            .BuildServiceProvider();

        OrderRepository = provider.GetRequiredService<IOrderRepository>();

        FluentAssertionOptions.UseDefaultPrecision();
    }
}
