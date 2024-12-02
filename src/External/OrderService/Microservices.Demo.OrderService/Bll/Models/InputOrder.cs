namespace Microservices.Demo.OrderService.Bll.Models;

public record InputOrder(
    long RegionId,
    long CustomerId,
    string? Comment,
    Item[] Items);