namespace Microservices.Demo.OrdersFacade.Infra.Options;

public class ExternalServicesOptions
{
    public required string OrderService_Name { get; init; }
    public required string OrderService1_Host { get; init; }
    public required string OrderService2_Host { get; init; }
    public int OrderService1_PortGrpc { get; init; }
    public int OrderService2_PortGrpc { get; init; }

    public required string CustomerService_Host { get; init; }
    public int CustomerService_PortGrpc { get; init; }
}
