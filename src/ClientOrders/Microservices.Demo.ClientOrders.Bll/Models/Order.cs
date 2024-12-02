using Microservices.Demo.OrderService.Proto.OrderGrpc;
using System;

namespace Microservices.Demo.ClientOrders.Bll.Models;

public record Order(
    long OrderId,
    long CustomerId,
    long RegionId,
    string RegionName,
    OrderStatus Status,
    string? Comment,
    DateTimeOffset CreatedAt);