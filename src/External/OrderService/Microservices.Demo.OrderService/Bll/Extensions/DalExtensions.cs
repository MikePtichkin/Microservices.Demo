using Microservices.Demo.OrderService.Bll.Models;
using Microservices.Demo.OrderService.Dal.Entities;
using Microservices.Demo.OrderService.Proto.OrderGrpc;

namespace Microservices.Demo.OrderService.Bll.Extensions;

public static class DalExtensions
{
    public static OrderEntity ToDal(this Order order)
    {
        return new OrderEntity
        {
            OrderId = order.OrderId,
            RegionId = order.Region.Id,
            CustomerId = order.CustomerId,
            Status = (int)order.Status,
            Comment = order.Comment,
            CreatedAt = order.CreatedAt
        };
    }

    public static Order ToBll(this OrderInfoEntity entity)
    {
        return new Order(
            OrderId: entity.OrderId,
            CustomerId: entity.CustomerId,
            Status: (OrderStatus)entity.Status,
            Region: new Region(
                Id: entity.RegionId,
                Name: entity.RegionName),
            Comment: entity.Comment,
            CreatedAt: entity.CreatedAt);
    }
    
    public static ItemEntity ToDal(this Item item, long orderId)
    {
        return new ItemEntity
        {
            OrderId = orderId,
            Barcode = item.Barcode,
            Quantity = item.Quantity
        };
    }
}