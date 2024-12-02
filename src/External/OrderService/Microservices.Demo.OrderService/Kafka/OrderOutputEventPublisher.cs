using Google.Protobuf;
using Microservices.Demo.OrderService.Kafka.Producers;
using Microservices.Demo.OrderService.Proto.Messages;

namespace Microservices.Demo.OrderService.Kafka;

public class OrderOutputEventPublisher : IOrderOutputEventPublisher
{
    private readonly IKafkaProducer _kafkaProducer;
    private const string Topic = "order_output_events";

    public OrderOutputEventPublisher(
        IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task PublishToKafka(
        OrderOutputEventMessage message,
        CancellationToken token)
    {
        await _kafkaProducer.SendProtoMessage(
            Topic,
            key: message.OrderId.ToString(),
            value: message.ToByteArray(),
            token);
    }
}