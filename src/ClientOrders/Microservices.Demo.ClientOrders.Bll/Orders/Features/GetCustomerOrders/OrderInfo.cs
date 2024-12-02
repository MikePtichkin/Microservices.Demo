using Microservices.Demo.ClientOrders.Domain.Common;
using Microservices.Demo.ClientOrders.Domain.Orders;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;

public sealed record OrderInfo(
    long OrderId,
    OrderStatus Status,
    TimeStamp CreatedAt);
