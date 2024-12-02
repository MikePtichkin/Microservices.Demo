using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Domain.Customers;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using Microservices.Demo.ClientOrders.Infra.Clients.Grpc;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using Microservices.Demo.ClientOrders.Infra.Dal.Repositories;
using Microservices.Demo.ClientOrders.Infra.Kafka.Consumers;
using Microservices.Demo.ClientOrders.Infra.Kafka.Producers;
using Microservices.Demo.ClientOrders.Infra.Kafka.Producers.Orders;
using Microservices.Demo.ClientOrders.Infra.Kafka.Settings;
using Microservices.Demo.ClientOrders.Infra.Options;
using Microservices.Demo.ClientOrders.Infra.Orders.Decorators;
using Microservices.Demo.ClientOrders.Infra.Redis;
using Microservices.Demo.ClientOrders.Infra.Redis.Repositories;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using System;
using System.Linq;
using static Microservices.Demo.CustomerService.CustomerService;

namespace Microservices.Demo.ClientOrders.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<CustomerServiceInstanceOptions>(configuration.GetSection(nameof(CustomerServiceInstanceOptions)))
            .Configure<OrderServiceOptions>(configuration.GetSection(nameof(OrderServiceOptions)));

        services
            .AddOrderGrpcClient(configuration)
            .AddCustomerGrpcClient(configuration);

        var connectionStrings = services.GetConnectionStrings(configuration);
        services
            .AddDal(connectionStrings.Postgres)
            .AddKafka(configuration, connectionStrings.Kafka)
            .AddRedis(connectionStrings.Redis);

        services.AddKeyedTransient<ICustomerClient, CustomerClientCachedDecorator>("cachedDecorator");

        return services;
    }

    private static ConnectionStrings GetConnectionStrings(this IServiceCollection services, IConfiguration configuration)
    {
        const string PostgresEnvVarName =
            "DEMO_CLIENT_ORDER_SERVICE_DB_CONNECTION_STRING";
        const string RedisEnvVarName =
            "DEMO_CLIENT_ORDER_SERVICE_REDIS_CONNECTION_STRING";
        const string KafkaEnvVarName =
            "DEMO_KAFKA_BROKERS";

        var dbConnectionString = Environment.GetEnvironmentVariable(PostgresEnvVarName)
            ?? configuration["ConnectionStrings:Postgres"]!;
        var kafkaConnectionString = Environment.GetEnvironmentVariable(KafkaEnvVarName)
            ?? configuration["ConnectionStrings:Kafka"]!;
        var redisConnectionString = Environment.GetEnvironmentVariable(RedisEnvVarName)
            ?? configuration["ConnectionStrings:Redis"]!;

        services.Configure<ConnectionStrings>(opts => new ConnectionStrings
        {
            Postgres = dbConnectionString,
            Kafka = kafkaConnectionString,
            Redis = redisConnectionString
        });

        return new ConnectionStrings
        {
            Postgres = dbConnectionString,
            Kafka = kafkaConnectionString,
            Redis = redisConnectionString
        };
    }

    private static IServiceCollection AddDal(this IServiceCollection services, string dbConnectionStrings)
    {
        services.AddMigrationRunner(dbConnectionStrings);

        services.AddDbContext<ClientOrdersDbContext>(options =>
        {
            options.UseNpgsql(dbConnectionStrings);
        });

        services.AddScoped<IOrdersRepository, OrdersRepository>();

        return services;
    }

    private static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration,
        string kafkaConnectionStrings)
    {
        services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<KafkaProducer>>();
            return new KafkaProducer(logger, kafkaConnectionStrings);
        });

        services.AddTransient<IOrdersInputEventPublisher, OrdersInputEventPublisher>();

        var kafkaSettings = (configuration
            .GetSection(nameof(KafkaSettings))
            .Get<KafkaSettings>() ?? throw new InvalidOperationException("Kafka settings are not configured"))
            with
        {
            BootstrapServers = kafkaConnectionStrings
        };

        services.AddOrderOutputEventsConsumer(configuration, kafkaSettings);
        services.AddOrdersInputErrorsConsumer(configuration, kafkaSettings);

        return services;
    }

    private static IServiceCollection AddOrderOutputEventsConsumer(
        this IServiceCollection services,
        IConfiguration configuration,
        KafkaSettings kafkaSettings)
    {
        var consumerSettings = configuration
            .GetSection("KafkaSettings:Consumer:OrderOutputEventsConsumer")
            .Get<ConsumerSettings>() ?? throw new InvalidOperationException("KafkaConsumer settings are not configured");

        services.AddHostedService(sp =>
            new OrderOutputEventsBackgroundConsumer(
                sp,
                kafkaSettings,
                consumerSettings));

        return services;
    }

    private static IServiceCollection AddOrdersInputErrorsConsumer(
        this IServiceCollection services,
        IConfiguration configuration,
        KafkaSettings kafkaSettings)
    {
        var consumerSettings = configuration
            .GetSection("KafkaSettings:Consumer:OrdersInputErrorsConsumer")
            .Get<ConsumerSettings>() ?? throw new InvalidOperationException("KafkaConsumer settings are not configured");

        services.AddHostedService(sp =>
            new OrdersInputErrorsBackgroundConsumer(
                sp,
                kafkaSettings,
                consumerSettings));

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, string redisConnectionStrings)
    {
        services.AddScoped<IRedisDatabaseFactory>(_ => new RedisDatabaseFactory(redisConnectionStrings));
        services.AddScoped<ICustomerCacheRepository, CustomerRedisRepository>();

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

    private static IServiceCollection AddCustomerGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerClient, CustomersGrpcClient>();

        services.AddGrpcClient<CustomerServiceClient>((serviceProvider, config) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CustomerServiceInstanceOptions>>().Value;
            config.Address = new Uri($"http://{options.Host}:{options.PortGrpc}");
        })
        .ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
        });

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
