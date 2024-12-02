using System.Collections.Generic;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;

public sealed class RegionOrdersResult
{
    public required IReadOnlyList<OrderWithCustomerInfoDto> OrdersWithCustomerNames { get; init; }
}
