using MediatR;
using Microservices.Demo.OrdersFacade.Domain.Abstraction;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;

public sealed record GetCustomerOrdersQuery(
    long CustomerId,
    int Limit,
    int Offset) : IRequest<Result<CustomerOrdersResult>>;
