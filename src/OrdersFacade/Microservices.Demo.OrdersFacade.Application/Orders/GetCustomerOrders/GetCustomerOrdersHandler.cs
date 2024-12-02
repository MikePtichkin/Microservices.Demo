using MediatR;
using Microservices.Demo.OrdersFacade.Application.Exceptions;
using Microservices.Demo.OrdersFacade.Domain.Abstraction;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;

internal sealed class GetCustomerOrdersHandler
    : IRequestHandler<GetCustomerOrdersQuery, Result<CustomerOrdersResult>>
{
    private readonly IOrdersCient _ordersClient;
    private readonly ICustomersClient _customerClient;

    public GetCustomerOrdersHandler(
        IOrdersCient orderRepository,
        ICustomersClient customerRepository)
    {
        _ordersClient = orderRepository;
        _customerClient = customerRepository;
    }

    public async Task<Result<CustomerOrdersResult>> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var customerEntities = await _customerClient.Query(
            customerIds: [request.CustomerId],
            limit: request.Limit,
            offset: request.Offset,
            cancellationToken);

        if (customerEntities.Length == 0)
        {
            throw new CustomerNotFoundException(request.CustomerId);
        }

        var customer = customerEntities.First();

        var orderEntities = await _ordersClient.Query(
            customersIds: [request.CustomerId],
            ordersIds: [],
            regionIds: [],
            limit: request.Limit,
            offset: request.Offset,
            token: cancellationToken);

        var result = new CustomerOrdersResult
        {
            Customer = customer,
            Orders = orderEntities
        };

        return result;
    }
}
