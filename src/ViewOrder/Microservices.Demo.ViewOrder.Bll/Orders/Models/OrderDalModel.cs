using System;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Models;

public sealed record OrderDalModel
{
    public required long OrderId { get; init; }
    public required long RegionId { get; init; }
    public required long CustomerId { get; init; }
    public required int Status { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public string? Comment { get; init; }
}
