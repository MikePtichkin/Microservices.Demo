using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Infra.Clients.Grpc;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure;
using Microservices.Demo.ViewOrder.Infra.Dal.Infrastructure.Connection;
using Microservices.Demo.ViewOrder.Infra.Dal.Repositories;
using Microservices.Demo.ViewOrder.Infra.Kafka.Consumers;
using Microservices.Demo.ViewOrder.Infra.Kafka.Settings;
using Microservices.Demo.ViewOrder.Infra.Options;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Rules;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Settings;
using System;
using System.Linq;
using ShardConfigurationBuilder = Microservices.Demo.ViewOrder.ShardConfiguration.ConfigurationBuilder;

namespace Microservices.Demo.ViewOrder.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<OrderServiceOptions>(configuration.GetSection(nameof(OrderServiceOptions)));

        services.AddOrderGrpcClient(configuration);

        services.AddDal(configuration);

        services.AddKafka(configuration);

        return services;
    }

    private static IServiceCollection AddOrderGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderClient, OrderGrpcClient>();

        services.AddSingleton<ResolverFactory>(serviceProvider =>
        {
            var balancerAddreses = serviceProvider.GetRequiredService<IOptions<OrderServiceOptions>>()
                .Value
                .Instances
                .Select(instance => new BalancerAddress(
                    host: instance.Host,
                    port: instance.PortGrpc));

            return new StaticResolverFactory(address => balancerAddreses);
        });

        services.AddGrpcClient<OrderGrpc.OrderGrpcClient>((serviceProvider, config) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<OrderServiceOptions>>().Value;
            config.Address = new Uri("static://" + options.Name);
        })
        .ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
            x.ServiceConfig = new ServiceConfig
            {
                LoadBalancingConfigs = { new RoundRobinConfig() }
            };
        });

        return services;
    }

    private static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var shardedMigratorSettings = new ShardedMigratorSettings
        {
            ConnectionStringShard1 = configuration.GetValue<string>("VIEW_ORDER_SERVICE_DB_SHARD1_CONNECTION_STRINGS") ??
                throw new InvalidOperationException("View order service shard1 db connection string is not configured"),

            ConnectionStringShard2 = configuration.GetValue<string>("VIEW_ORDER_SERVICE_DB_SHARD2_CONNECTION_STRINGS") ??
                throw new InvalidOperationException("View order service shard2 db connection string is not configured"),

            BucketsPerShard = configuration.GetValue<int>("VIEW_ORDER_SERVICE_DB_BUCKETS_PER_SHARD")
        };

        var config = ShardConfigurationBuilder.Build(
            shardedMigratorSettings.BucketsPerShard,
            shardedMigratorSettings.ConnectionStringShard1,
            shardedMigratorSettings.ConnectionStringShard2);

        ShardedMigrationRunner.MigrateUp(config.Endpoints);

        services.AddSingleton<IDbStore>(new DbStore([.. config.Endpoints]));
        services.AddSingleton<IShardingRule<long>>(new LongShardingRule(config.BucketsCount));
        services.AddScoped<IShardConnectionFactory, ShardConnectionFactory>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    private static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaSettings =
            (
                configuration
                    .GetSection(nameof(KafkaSettings))
                    .Get<KafkaSettings>() ??
                        throw new InvalidOperationException("Kafka settings are not configured")
            )
            with
            {
                BootstrapServers = configuration.GetValue<string>("KAFKA_BROKERS") ??
                    throw new InvalidOperationException("KAFKA_BROKERS are not configured")
            };

        var consumerSettings = configuration
            .GetSection("KafkaSettings:Consumers:OrderOutputEventsConsumer")
            .Get<ConsumerSettings>() ?? throw new InvalidOperationException("KafkaConsumer settings are not configured");

        services.AddHostedService(sp => new OrderOutputEventsBackgroundConsumer(
            sp,
            kafkaSettings,
            consumerSettings));

        return services;
    }
}
