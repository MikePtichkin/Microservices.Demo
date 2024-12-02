using MediatR;
using Microservices.Demo.OrdersFacade.Domain.Abstraction;
using Microservices.Demo.OrdersFacade.Domain.Customers;
using Microservices.Demo.OrdersFacade.Domain.Orders;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;

internal sealed class GetRegionOrdersHandler
    : IRequestHandler<GetRegionOrdersQuery, Result<RegionOrdersResult>>
{
    private readonly IOrdersCient _ordersClient;
    private readonly ICustomersClient _customersClient;

    public GetRegionOrdersHandler(
        IOrdersCient ordersRepository,
        ICustomersClient customersClient)
    {
        _ordersClient = ordersRepository;
        _customersClient = customersClient;
    }

    public async Task<Result<RegionOrdersResult>> Handle(
        GetRegionOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _ordersClient.Query(
            customersIds: [],
            ordersIds: [],
            regionIds: [ request.RegionId ],
            limit: request.Limit,
            offset: request.Offset,
            token: cancellationToken);

        var customerIds = orders
            .Select(o => o.CustomerId)
            .Distinct()
            .ToArray();

        var customers = await _customersClient.Query(customerIds, request.Limit, 0, cancellationToken);

        var ordersWithCustomerNames = orders.Join(customers,
            order => order.CustomerId,
            customer => customer.Id,
            (order, customer) => new OrderWithCustomerInfoDto(
                orderId: order.Id,
                regionId: order.RegionId,
                createdAt: order.CreatedAt,
                status: order.Status,
                customerId: customer.Id,
                customerName: customer.FullName.Value,
                comment: order.Comment));

        return new RegionOrdersResult
        {
            OrdersWithCustomerNames = [.. ordersWithCustomerNames]
        };
    }
}
