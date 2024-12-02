using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microservices.Demo.OrderService.Proto.OrderGrpc;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using Microservices.Demo.OrdersFacade.Infra.Options;
using System;
using static Microservices.Demo.CustomerService.CustomerService;
using Microservices.Demo.OrdersFacade.Infra.Clients.Grpc;

namespace Microservices.Demo.OrdersFacade.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ExternalServicesOptions>(configuration.GetSection("ExternalServicesOptions"));

        services.AddOrderGrpcClient(configuration);

        services.AddCustomerGrpcClient(configuration);

        services.AddScoped<ICustomersClient, CustomersGrpcClient>();
        services.AddScoped<IOrdersCient, OrdersGrpcClients>();

        return services;
    }

    private static void AddOrderGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ResolverFactory>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ExternalServicesOptions>>().Value;

            return new StaticResolverFactory(address =>
                [
                    new BalancerAddress(
                        host: options.OrderService1_Host,
                        port: options.OrderService1_PortGrpc),
                    new BalancerAddress(
                        host: options.OrderService2_Host,
                        port: options.OrderService2_PortGrpc)
                ]);
        });

        services.AddGrpcClient<OrderGrpc.OrderGrpcClient>((serviceProvider, config) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ExternalServicesOptions>>().Value;
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

    private static void AddCustomerGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<CustomerServiceClient>((serviceProvider, config) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ExternalServicesOptions>>().Value;
            config.Address = new Uri($"http://{options.CustomerService_Host}:{options.CustomerService_PortGrpc}");
        })
        .ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
        });
    }
}
