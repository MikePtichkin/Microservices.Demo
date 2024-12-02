using Microservices.Demo.OrderService.Proto.OrderGrpc;

namespace Microservices.Demo.OrderService.Bll.Models;

public record Order(
    long OrderId,
    long CustomerId,
    OrderStatus Status,
    Region Region,
    string Comment,
    DateTimeOffset CreatedAt);