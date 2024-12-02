namespace Microservices.Demo.DataGenerator.Infra.Kafka;

public record KafkaTopics
{
    public required string OrdersInput { get; init; }
}
