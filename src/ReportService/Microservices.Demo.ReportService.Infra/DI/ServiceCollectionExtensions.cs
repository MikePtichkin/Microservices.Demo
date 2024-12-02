using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.ReportService.Bll.Contracts;
using Microservices.Demo.ReportService.Domain.Orders;
using Microservices.Demo.ReportService.Infra.Clients.Grpc;
using Microservices.Demo.ReportService.Infra.Options;
using Microservices.Demo.ReportService.Infra.RateLimiter;
using System;
using System.Linq;

namespace Microservices.Demo.ReportService.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<OrderServiceOptions>(configuration.GetSection(nameof(OrderServiceOptions)))
            .Configure<CsvReportGeneratorOptions>(configuration.GetSection(nameof(CsvReportGeneratorOptions)));

        services.AddOrdersGrpcClient(configuration);

        services.AddScoped<IOrdersCient, OrdersGrpcClient>();
        services.AddScoped<IReportGenerator, CsvReportGenerator>();

        services.AddSingleton<IRateLimiter, ReportGeneratorRateLimiter>();

        return services;
    }

    private static void AddOrdersGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
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
            config.Address = new Uri("static://" + options.OrderService_Name);
        })
        .ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
            x.ServiceConfig = new ServiceConfig
            {
                LoadBalancingConfigs = { new RoundRobinConfig() }
            };
        });
    }
}
