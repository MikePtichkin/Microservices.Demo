using System;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Dtos;

public record OrderDto(
    long Id,
    long RegionId,
    string Status,
    long CustomerId,
    string? Comment,
    DateTime CreatedAt);
