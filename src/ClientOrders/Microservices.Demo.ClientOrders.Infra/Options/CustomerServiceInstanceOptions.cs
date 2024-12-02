namespace Microservices.Demo.ClientOrders.Infra.Options;

public record CustomerServiceInstanceOptions
{
    public required string Host { get; init; }
    public required int PortGrpc { get; init; }
}