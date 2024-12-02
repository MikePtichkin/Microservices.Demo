using System.Collections.Generic;

namespace Microservices.Demo.ReportService.Infra.Options;

public record OrderServiceOptions
{
    public required int MaxConcurrentRequests { get; init; }
    public required string OrderService_Name { get; init; }
    public required IReadOnlyList<OrderServiceInstanceOptions> Instances { get; init; }
}
