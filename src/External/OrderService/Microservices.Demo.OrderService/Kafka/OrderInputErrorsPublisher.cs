using System.Text.Json;
using Microservices.Demo.OrderService.Kafka.Messages;
using Microservices.Demo.OrderService.Kafka.Producers;

namespace Microservices.Demo.OrderService.Kafka;

public class OrderInputErrorsPublisher : IOrderInputErrorsPublisher
{
    private readonly IKafkaProducer _kafkaProducer;
    private const string Topic = "orders_input_errors";

    public OrderInputErrorsPublisher(
        IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task PublishToKafka(
        OrderInputErrorsMessage message,
        CancellationToken token)
    {
        var value = JsonSerializer.Serialize(message);
        
        await _kafkaProducer.SendMessage(
            Topic,
            message.InputOrder.CustomerId.ToString(),
            value,
            token);
    }
}