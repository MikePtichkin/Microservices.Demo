using Microsoft.AspNetCore.Mvc;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Requests;

public sealed record OrdersByRegionRequest
{
    [FromRoute(Name = "regionId")]
    public long RegionId { get; init; }

    [FromQuery]
    public int Limit { get; init; }

    [FromQuery]
    public int Offset { get; init; }
}
