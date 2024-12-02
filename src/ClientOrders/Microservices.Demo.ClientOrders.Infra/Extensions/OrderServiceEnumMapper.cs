using Microservices.Demo.ClientOrders.Domain.Orders;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Extensions;

internal static class OrderServiceEnumMapper
{
    public static OrderStatus ToDomain(this OrderService.Proto.OrderGrpc.OrderStatus protoOrderStatus) => protoOrderStatus switch
    {
        OrderService.Proto.OrderGrpc.OrderStatus.New => OrderStatus.New,
        OrderService.Proto.OrderGrpc.OrderStatus.Undefined => OrderStatus.Undefined,
        OrderService.Proto.OrderGrpc.OrderStatus.Canceled => OrderStatus.Canceled,
        OrderService.Proto.OrderGrpc.OrderStatus.Delivered => OrderStatus.Delivered,
        _ => throw new ArgumentOutOfRangeException(
            nameof(protoOrderStatus),
            $"Unknown order status: {protoOrderStatus}")

    };
}
