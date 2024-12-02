using Microservices.Demo.OrderService.Proto.Messages;

namespace Microservices.Demo.OrderService.Kafka;

public interface IOrderOutputEventPublisher
{
    Task PublishToKafka(
        OrderOutputEventMessage message,
        CancellationToken token);
}