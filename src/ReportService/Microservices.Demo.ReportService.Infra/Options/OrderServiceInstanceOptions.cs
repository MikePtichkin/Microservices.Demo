namespace Microservices.Demo.ReportService.Infra.Options;

public class OrderServiceInstanceOptions
{
    public required string Host { get; init; }
    public required int PortGrpc { get; init; }
}