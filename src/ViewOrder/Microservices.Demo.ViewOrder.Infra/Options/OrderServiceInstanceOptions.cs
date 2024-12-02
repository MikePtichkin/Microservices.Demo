namespace Microservices.Demo.ViewOrder.Infra.Options;

public record OrderServiceInstanceOptions
{
    public required string Host { get; init; }
    public required int PortGrpc { get; init; }
}
