using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using System.Collections.Generic;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;

public sealed class CustomerOrdersResult
{
    public required Customer Customer { get; init; }
    public required IReadOnlyList<Order> Orders { get; init; }
}