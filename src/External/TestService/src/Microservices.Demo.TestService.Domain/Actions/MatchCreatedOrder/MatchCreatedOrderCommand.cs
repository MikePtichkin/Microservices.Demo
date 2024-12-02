using MediatR;

namespace Microservices.Demo.TestService.Domain.Actions.MatchCreatedOrder;

public class MatchCreatedOrderCommand : IRequest
{
    public required string Key { get; init; }

    public required long OrderId { get; init; }

    public required string EventType { get; init; }
}
