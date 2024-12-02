using Microservices.Demo.OrdersFacade.Domain.Abstraction;
using System;

namespace Microservices.Demo.OrdersFacade.Domain.Orders;

public sealed class Order : Entity
{
    public Order(
        long id,
        long regionId,
        OrderStatus status,
        long customerId,
        DateTime createdAt,
        OrderComment? comment = null)
        : base(id)
    {
        RegionId = regionId;
        Status = status;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Comment = comment;
    }

    public long RegionId { get; init; }
    public OrderStatus Status { get; init; }
    public long CustomerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public OrderComment? Comment { get; init; }
}