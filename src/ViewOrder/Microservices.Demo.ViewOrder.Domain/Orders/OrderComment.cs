﻿namespace Microservices.Demo.ViewOrder.Domain.Orders;

public sealed record OrderComment
{
    public OrderComment()
    { }

    public required string Value { get; init; }

    public static implicit operator OrderComment?(string? orderComment) =>
        orderComment is null
            ? null
            : new() { Value = orderComment };

    public static implicit operator string?(OrderComment? orderComment) =>
        orderComment?.Value;
}
