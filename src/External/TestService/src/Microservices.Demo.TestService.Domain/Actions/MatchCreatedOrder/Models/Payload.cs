namespace Microservices.Demo.TestService.Domain.Actions.MatchCreatedOrder;

public class Payload
{
    public required long OrderId { get; init; }

    public required string EventType { get; init; }
}
