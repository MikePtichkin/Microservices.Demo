using Microservices.Demo.OrdersFacade.Features.Orders.Dtos;
using System.Collections.Generic;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Responses;

public class CustomerOrdersResponse
{
    public required CustomerDto Customer { get; init; }
    public required IReadOnlyCollection<OrderDto> Orders { get; init; }
}
