using System.Collections.Generic;

namespace Microservices.Demo.ClientOrders.Infra.Options;

public record OrderServiceOptions
{
    public required string Name { get; init; }
    public required IReadOnlyList<OrderServiceInstanceOptions> Instances { get; init; }
}
