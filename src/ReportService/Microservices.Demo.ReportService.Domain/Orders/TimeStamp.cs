using System;
using GoogleTimestamp = Google.Protobuf.WellKnownTypes.Timestamp;
namespace Microservices.Demo.ReportService.Domain.Orders;

public sealed record TimeStamp
{
    public TimeStamp()
    { }

    public required DateTimeOffset Value { get; init; }

    public static implicit operator TimeStamp(GoogleTimestamp timestamp) => new()
    {
        Value = timestamp.ToDateTimeOffset()
    };

    public static implicit operator GoogleTimestamp(TimeStamp timestamp) =>
        GoogleTimestamp.FromDateTimeOffset(timestamp.Value);

    public static implicit operator string(TimeStamp timestamp) =>
        timestamp.Value.ToString("o");
}