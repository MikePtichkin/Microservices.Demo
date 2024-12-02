using Microservices.Demo.OrderService.Kafka.Messages;

namespace Microservices.Demo.OrderService.Kafka;

public interface IOrderInputErrorsPublisher
{
    Task PublishToKafka(
        OrderInputErrorsMessage message,
        CancellationToken token);
}