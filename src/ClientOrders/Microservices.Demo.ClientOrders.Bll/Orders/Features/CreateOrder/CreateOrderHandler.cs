using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Demo.ClientOrders.Bll.Orders.Abstractions;
using Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder.Messages;
using Microservices.Demo.ClientOrders.Domain.Customers;
using Microservices.Demo.ClientOrders.Domain.Orders;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.CreateOrder;

internal sealed class CreateOrderHandler
    : IRequestHandler<CreateOrderCommand>
{
    private readonly ICustomerClient _customerCachedClient;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IOrdersInputEventPublisher _orderInputEventPublisher;

    public CreateOrderHandler(
        [FromKeyedServices("cachedDecorator")] ICustomerClient customerCachedClient,
        IOrdersRepository ordersRepository,
        IOrdersInputEventPublisher orderInputEventPublisher)
    {
        _customerCachedClient = customerCachedClient;
        _ordersRepository = ordersRepository;
        _orderInputEventPublisher = orderInputEventPublisher;
    }

    public async Task Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerCachedClient.Query(
            request.CustomerId,
            cancellationToken);

        // в конструкторе используется DateTimeOffset Now - есть смысл добавить провайдера даты времени
        var order = Order.New(
            customer!.RegionId,
            request.CustomerId);

        await _ordersRepository.Add(order, cancellationToken);

        var message = new OrdersInputMessage()
        {
            RegionId = order.RegionId,
            CustomerId = request.CustomerId,
            Comment = OrderIdParser.GenerateComment(order.Id, order.Comment),
            Items = request.StockItems.Select(o => new Item()
            {
                Barcode = o.ItemBarcode,
                Quantity = o.Quantity
            }).ToArray()
        };

        await _orderInputEventPublisher.PublishToKafka(message, cancellationToken);
    }
}