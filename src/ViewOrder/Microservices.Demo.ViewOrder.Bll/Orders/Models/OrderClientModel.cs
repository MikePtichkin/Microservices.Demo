using System;
using OrderStatus = Microservices.Demo.OrderService.Proto.OrderGrpc.OrderStatus;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Models;

public record OrderClientModel(
    long OrderId,
    long CustomerId,
    long RegionId,
    string RegionName,
    OrderStatus Status,
    string? Comment,
    DateTimeOffset CreatedAt);