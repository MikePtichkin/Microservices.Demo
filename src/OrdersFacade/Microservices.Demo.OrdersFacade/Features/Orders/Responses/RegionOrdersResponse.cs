using Microservices.Demo.OrdersFacade.Features.Orders.Dtos;
using System.Collections.Generic;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Responses;

public class RegionOrdersResponse
{
    public required IReadOnlyCollection<OrderWithCustomerInfoDto> Orders { get; init; }
}