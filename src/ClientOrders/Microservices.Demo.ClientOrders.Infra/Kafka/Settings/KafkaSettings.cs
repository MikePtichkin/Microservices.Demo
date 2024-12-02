namespace Microservices.Demo.ClientOrders.Infra.Kafka.Settings;

public record KafkaSettings
{
    public required string GroupId { get; init; }
    public required string BootstrapServers {  get; init; }
    public int TimeoutForRetryInSeconds { get; init; } = 2;
}
