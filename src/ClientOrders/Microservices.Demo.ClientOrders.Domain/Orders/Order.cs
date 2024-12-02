using Microservices.Demo.ClientOrders.Abstraction;
using Microservices.Demo.ClientOrders.Domain.Common;
using System;

namespace Microservices.Demo.ClientOrders.Domain.Orders;

public sealed class Order : Entity
{
    public Order(
        long regionId,
        long customerId,
        OrderStatus status,
        TimeStamp createdAt,
        OrderComment comment,
        long? orderId = null)
    {
        RegionId = regionId;
        CustomerId = customerId;
        Status = status;
        CreatedAt = createdAt;
        Comment = comment;
    }

    public long? OrderId { get; private set; } 
    public long RegionId { get; init; }
    public long CustomerId { get; init; }
    public OrderStatus Status { get; private set; }
    public TimeStamp CreatedAt { get; init; }
    public OrderComment Comment { get; init; }
    public string? Error { get; private set; }

    public static Order New(
        long regionId,
        long customerId)
    {
        return new(
            regionId,
            customerId,
            OrderStatus.New,
            DateTimeOffset.Now,
            Guid.NewGuid().ToString());
    }

    public void Confirm(long id) => OrderId = id;

    public void SetError(string error) => Error = error;

    public void Cancel()
    {
        if (Status is not OrderStatus.New)
            throw new InvalidOperationException("Cannot change order status that is not New.");

        Status = OrderStatus.Canceled;
    }

    public void SetStatus(OrderStatus newStatus)
    {
        if (newStatus is OrderStatus.Undefined)
            throw new InvalidOperationException("Cannot change order status to Undefined.");

        if (Status is not OrderStatus.New)
            throw new InvalidOperationException("Cannot change order status that is not New.");

        Status = newStatus;
    }
}

