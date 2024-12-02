using System;

namespace Microservices.Demo.ViewOrder.Domain.Common;

public sealed record TimeStamp
{
    public TimeStamp()
    { }

    public required DateTimeOffset Value { get; init; }

    public static implicit operator DateTimeOffset(TimeStamp timeStamp) =>
        timeStamp.Value;

    public static implicit operator TimeStamp(DateTimeOffset dateTimeOffset) =>
        new()
        {
            Value  = dateTimeOffset
        };
}