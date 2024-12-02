using System.Collections.Generic;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;

public sealed class CustomerOrdersResult
{
    public required IReadOnlyList<OrderInfo> Orders { get; init; }
}