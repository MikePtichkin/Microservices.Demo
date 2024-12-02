using MediatR;
using Microservices.Demo.TestService.Common.Kafka.Consumer;
using Microservices.Demo.TestService.Data;
using Microservices.Demo.TestService.Domain.Actions.MatchOrderError;
using Microservices.Demo.TestService.Integrations.Orders.Messages;

namespace Microservices.Demo.TestService.Integrations.Orders;

public class OrderInputErrorsMessageProcessor : IMessageProcessor<string, OrderInputErrorsMessage>
{
    private readonly IMismatchFeature _mismatchFeature;
    private readonly IMediator _mediator;

    public OrderInputErrorsMessageProcessor(IMismatchFeature mismatchFeature, IMediator mediator)
    {
        _mismatchFeature = mismatchFeature;
        _mediator = mediator;
    }

    public Task ProcessMessageAsync(string key, OrderInputErrorsMessage payload, CancellationToken cancellationToken)
    {
        if (!_mismatchFeature.IsEnabled)
            return Task.CompletedTask;

        var command = new MatchOrderErrorCommand
        {
            Key = key,
            CustomerId = payload.InputOrder.CustomerId,
            RegionId = payload.InputOrder.RegionId,
            OrderItems = payload.InputOrder.Items
                .Select(item => new OrderItem
                {
                    Barcode = item.Barcode,
                    Quantity = item.Quantity
                })
                .ToArray()
        };

        return _mediator.Send(command, cancellationToken);
    }
}
