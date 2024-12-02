using MediatR;
using Microservices.Demo.ClientOrders.Domain.Orders.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Bll.Orders.Features.GetCustomerOrders;

internal sealed class GetCustomerOrdersHandler
    : IRequestHandler<GetCustomerOrdersQuery, CustomerOrdersResult>
{
    private readonly IOrdersRepository _ordersRepository;

    public GetCustomerOrdersHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<CustomerOrdersResult> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _ordersRepository.GetCustomerOrders(
            request.CustomerId,
            request.Limit,
            request.Offset,
            cancellationToken);

        return new CustomerOrdersResult
        {
            Orders = orders.Select(o => new OrderInfo(
                o.Id,
                o.Status,
                o.CreatedAt)).ToArray()
        };
    }
}
