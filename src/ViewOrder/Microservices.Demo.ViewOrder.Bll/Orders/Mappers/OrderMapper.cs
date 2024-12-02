using Microservices.Demo.ViewOrder.Bll.Orders.Models;
using Microservices.Demo.ViewOrder.Domain.Orders;
using OrderDomain = Microservices.Demo.ViewOrder.Domain.Orders.Order;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Mappers;

public static class OrderMapper
{
    public static OrderDomain ToDomain(this OrderClientModel bllOrder)
    {
        return new OrderDomain(
            id: bllOrder.OrderId,
            regionId: bllOrder.RegionId,
            customerId: bllOrder.CustomerId,
            status: (OrderStatus)bllOrder.Status,
            createdAt: bllOrder.CreatedAt,
            comment: bllOrder.Comment);
    }

    public static OrderDalModel ToDal(this OrderDomain domainOrder)
    {
        return new OrderDalModel
        {
            OrderId = domainOrder.Id,
            RegionId = domainOrder.RegionId,
            CustomerId = domainOrder.CustomerId,
            Status = (int)domainOrder.Status,
            CreatedAt = domainOrder.CreateAt,
            Comment = domainOrder.Comment
        };
    }
}
