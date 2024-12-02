using MediatR;
using Microservices.Demo.ViewOrder.Bll.Exceptions;
using Microservices.Demo.ViewOrder.Bll.Orders.Abstractions;
using Microservices.Demo.ViewOrder.Bll.Orders.Mappers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ViewOrder.Bll.Orders.Features.OrderOutputEventMessage;

internal sealed class OrderOutputEventMessageHandler : IRequestHandler<OrderOutputEventMessage>
{
    private readonly IOrderClient _orderClient;
    private readonly IOrderRepository _orderRepository;

    public OrderOutputEventMessageHandler(
        IOrderClient orderClient,
        IOrderRepository orderRepository)
    {
        _orderClient = orderClient;
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        OrderOutputEventMessage request,
        CancellationToken cancellationToken)
    {
        var clientOrder = await _orderClient.Query(
            request.OrderId,
            cancellationToken) ?? throw new OrderNotFoundException(request.OrderId);

        var domainOrder = clientOrder.ToDomain();

        await _orderRepository.Add(domainOrder.ToDal(), cancellationToken);
    }
}
