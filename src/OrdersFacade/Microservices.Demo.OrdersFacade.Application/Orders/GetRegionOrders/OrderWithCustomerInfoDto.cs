using Microservices.Demo.OrdersFacade.Domain.Orders;
using System;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;

public record OrderWithCustomerInfoDto
{
    public OrderWithCustomerInfoDto(
        long orderId,
        long regionId,
        DateTime createdAt,
        OrderStatus status,
        long customerId,
        string customerName,
        OrderComment? comment)
    {
        OrderId = orderId;
        RegionId = regionId;
        CreatedAt = createdAt;
        Status = status;
        CustomerId = customerId;
        CustomerName = customerName;
        Comment = comment;
    }

    public long OrderId { get; init; }
    public long RegionId { get; init; }
    public DateTime CreatedAt { get; init; }
    public OrderStatus Status { get; init; }
    public long CustomerId { get; init; }
    public string CustomerName { get; init; }
    public OrderComment? Comment { get; init; }
}
