﻿namespace Microservices.Demo.TestService.Domain.Actions.MatchOrderError;

public class Payload
{
    public required long CustomerId { get; init; }

    public required long RegionId { get; init; }

    public required IReadOnlyCollection<OrderItem> OrderItems { get; init; }
}