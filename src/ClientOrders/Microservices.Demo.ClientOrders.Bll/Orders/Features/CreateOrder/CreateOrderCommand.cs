using MediatR;
using Microservices.Demo.ClientOrders.Domain.Orders;
using System.Collections.Generic;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder;

public record CreateOrderCommand(
    long CustomerId,
    IReadOnlyCollection<StockItem> StockItems) : IRequest;
