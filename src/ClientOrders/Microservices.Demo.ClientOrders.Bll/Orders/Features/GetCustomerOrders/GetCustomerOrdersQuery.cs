using MediatR;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;

public sealed record GetCustomerOrdersQuery(
    long CustomerId,
    int Limit,
    int Offset) : IRequest<CustomerOrdersResult>;
