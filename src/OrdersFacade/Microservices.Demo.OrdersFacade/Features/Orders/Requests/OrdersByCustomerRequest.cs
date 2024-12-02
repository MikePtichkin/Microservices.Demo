using Microsoft.AspNetCore.Mvc;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Requests;

public sealed record OrdersByCustomerRequest
{
    [FromRoute(Name = "customerId")]
    public long CustomerId { get; init; }

    [FromQuery]
    public int Limit { get; init; }

    [FromQuery]
    public int Offset { get; init; }
}
