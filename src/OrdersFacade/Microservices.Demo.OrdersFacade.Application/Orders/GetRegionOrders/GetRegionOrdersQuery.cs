using MediatR;
using Microservices.Demo.OrdersFacade.Domain.Abstraction;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;

public sealed record GetRegionOrdersQuery(
    long RegionId,
    int Limit,
    int Offset) : IRequest<Result<RegionOrdersResult>>;