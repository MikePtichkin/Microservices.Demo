namespace Microservices.Demo.ClientOrders.Infra.Options;

public record ConnectionStrings
{
    public required string Postgres {  get; init; }
    public required string Redis { get; init; }
    public required string Kafka { get; init; }
}
