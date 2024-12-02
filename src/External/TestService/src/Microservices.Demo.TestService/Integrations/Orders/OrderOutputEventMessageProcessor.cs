using MediatR;
using Microservices.Demo.OrderService.Proto.Messages;
using Microservices.Demo.TestService.Common.Kafka.Consumer;
using Microservices.Demo.TestService.Data;
using Microservices.Demo.TestService.Domain.Actions.MatchCreatedOrder;

namespace Microservices.Demo.TestService.Integrations.Orders;

public class OrderOutputEventMessageProcessor : IMessageProcessor<string, OrderOutputEventMessage>
{
    private readonly IMismatchFeature _mismatchFeature;
    private readonly IMediator _mediator;

    public OrderOutputEventMessageProcessor(IMismatchFeature mismatchFeature, IMediator mediator)
    {
        _mismatchFeature = mismatchFeature;
        _mediator = mediator;
    }

    public Task ProcessMessageAsync(string key, OrderOutputEventMessage payload, CancellationToken cancellationToken)
    {
        if (!_mismatchFeature.IsEnabled)
            return Task.CompletedTask;

        var command = new MatchCreatedOrderCommand
        {
            Key = key,
            OrderId = payload.OrderId,
            EventType = payload.EventType.ToString()
        };

        return _mediator.Send(command, cancellationToken);
    }
}
