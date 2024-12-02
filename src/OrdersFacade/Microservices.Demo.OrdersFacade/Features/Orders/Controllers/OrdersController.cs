using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservices.Demo.OrdersFacade.Application.Orders.GetCustomerOrders;
using Microservices.Demo.OrdersFacade.Application.Orders.GetRegionOrders;
using Microservices.Demo.OrdersFacade.Features.Orders.Dtos;
using Microservices.Demo.OrdersFacade.Features.Orders.Requests;
using Microservices.Demo.OrdersFacade.Features.Orders.Responses;
using Microservices.Demo.OrdersFacade.Infrastructure.Filters;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderWithCustomerInfoDto = Microservices.Demo.OrdersFacade.Features.Orders.Dtos.OrderWithCustomerInfoDto;
using Microsoft.Extensions.Logging;
using Microservices.Demo.OrdersFacade.Infrastructure.Metrics;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Controllers;

[ApiController]
[GlobalExceptionFilter]
[Route("v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly ISender _sender;
    private readonly IGetOrdersMeter _meter;

    public OrdersController(
        ILogger<OrdersController> logger,
        ISender sender,
        IGetOrdersMeter meter)
    {
        _logger = logger;
        _sender = sender;
        _meter = meter;
    }
    
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerOrdersResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerOrdersResponse>> GetOrderByCustomer(
        OrdersByCustomerRequest request,
        CancellationToken cancellationToken)
    {
        _meter.IncrementOrdersByCustomerTotalCounter();

        _logger.LogInformation("Getting customer orders for customer Id {CustomerId}", request.CustomerId);

        var ordersByCustomerQuery = new GetCustomerOrdersQuery(
            request.CustomerId,
            request.Limit,
            request.Offset);

        var ordersByCustomerResult = await _sender.Send(ordersByCustomerQuery, cancellationToken);

        _meter.RecordOrdersByCustomerPerRequest(ordersByCustomerResult.Value.Orders.Count);

        return Ok(new CustomerOrdersResponse
        {
            Customer = new CustomerDto
            (
                Id: ordersByCustomerResult.Value.Customer.Id,
                RegionId: ordersByCustomerResult.Value.Customer.RegionId,
                Name: ordersByCustomerResult.Value.Customer.FullName.Value,
                CreatedAt: ordersByCustomerResult.Value.Customer.CreatedAt
            ),
            Orders = ordersByCustomerResult.Value.Orders.Select(o => new OrderDto
            (
                Id: o.Id,
                RegionId: o.RegionId,
                Status: o.Status.ToString(),
                CustomerId: o.CustomerId,
                Comment: o.Comment?.Value,
                CreatedAt: o.CreatedAt
            )).ToArray()
        });
    }

    [HttpGet("region/{regionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegionOrdersResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RegionOrdersResponse[]>> GetOrdersByRegion(
        OrdersByRegionRequest request,
        CancellationToken cancellationToken)
    {
        _meter.IncrementOrdersByRegionTotalCounter();

        var ordersInRegionQuery = new GetRegionOrdersQuery(
            request.RegionId,
            request.Limit,
            request.Offset);

        var ordersInRegionResult = await _sender.Send(ordersInRegionQuery, cancellationToken);

        _meter.OrdersByRegionPerRequest(ordersInRegionResult.Value.OrdersWithCustomerNames.Count);

        return Ok(new RegionOrdersResponse
        {
            Orders = ordersInRegionResult.Value.OrdersWithCustomerNames.Select(o => new OrderWithCustomerInfoDto
            (
                Id: o.OrderId,
                RegionId: o.RegionId,
                Status: o.Status.ToString(),
                CustomerId: o.CustomerId,
                CustomerName: o.CustomerName,
                Comment: o.Comment?.Value,
                CreatedAt: o.CreatedAt
            )).ToArray()
        });
    }
}
