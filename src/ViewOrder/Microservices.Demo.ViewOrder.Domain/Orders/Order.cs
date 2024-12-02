using Microservices.Demo.ViewOrder.Abstraction;
using Microservices.Demo.ViewOrder.Domain.Common;

namespace Microservices.Demo.ViewOrder.Domain.Orders;

public sealed class Order : Entity
{
    public Order(
        long id,
        long regionId,
        long customerId,
        OrderStatus status,
        TimeStamp createdAt,
        OrderComment? comment = null)
        : base(id)
    {
        RegionId = regionId;
        CustomerId = customerId;
        Status = status;
        CreateAt = createdAt;
        Comment = comment;
    }

    public long RegionId { get; init; }
    public long CustomerId { get; init; }
    public OrderStatus Status { get; init; }
    public TimeStamp CreateAt { get; init; }
    public OrderComment? Comment { get; init; }
}