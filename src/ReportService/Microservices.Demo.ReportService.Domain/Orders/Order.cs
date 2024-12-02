using Microservices.Demo.ReportService.Domain.Abstraction;
using System;

namespace Microservices.Demo.ReportService.Domain.Orders;

public sealed class Order : Entity
{
    public Order(
        long id,
        OrderStatus status,
        TimeStamp createdAt,
        OrderComment? comment = null)
        : base(id)
    {
        Status = status;
        CreatedAt = createdAt;
        Comment = comment;
    }

    public long RegionId { get; init; }
    public OrderStatus Status { get; init; }
    public TimeStamp CreatedAt { get; init; }
    public OrderComment? Comment { get; init; }
}

