using System.Diagnostics.CodeAnalysis;
using Microservices.Demo.OrderService.Proto.Messages;
using Microservices.Demo.TestService.Common.Kafka.Consumer;
using Microservices.Demo.TestService.Integrations.Orders;
using Microservices.Demo.TestService.Integrations.Orders.Messages;

namespace Microservices.Demo.TestService.AppStart;

[ExcludeFromCodeCoverage]
public static class IntegrationsExtensions
{
    private const string OrdersOutputEventsSection = "OrdersOutputEvents:Kafka";
    private const string OrdersInputErrorsSection = "OrdersInputErrors:Kafka";

    public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddKafkaIntegrations(configuration);
    }

    private static IServiceCollection AddKafkaIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IMessageProcessor<string, OrderOutputEventMessage>, OrderOutputEventMessageProcessor>()
            .AddKafkaConsumer<string, OrderOutputEventMessage, OrderOutputEventMessageProcessor>(
                configuration.GetSection(OrdersOutputEventsSection),
                builder => builder
                    .WithStringKey()
                    .WithProtoValue())
            .AddSingleton<IMessageProcessor<string, OrderInputErrorsMessage>, OrderInputErrorsMessageProcessor>()
            .AddKafkaConsumer<string, OrderInputErrorsMessage, OrderInputErrorsMessageProcessor>(
                configuration.GetSection(OrdersInputErrorsSection),
                builder => builder
                    .WithStringKey()
                    .WithJsonValue());
    }
}
