namespace Microservices.Demo.ClientOrders.Infra.Kafka.Settings;

public record ConsumerSettings
{
    public required string Topic { get; init; }
    public bool AutoCommit { get; init; }
}
