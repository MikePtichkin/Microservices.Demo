using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microservices.Demo.OrdersFacade.Features.Orders.Requests;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Infrastructure.Middlewares.OrdersValidation;

public class ValidateOrdersByRegionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateOrdersByRegionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IValidator<OrdersByRegionRequest> validator)
    {
        if (!TryGetRegionId(context, out long regionId) ||
            !TryGetQueryIntValue(context, "limit", out int limit) ||
            !TryGetQueryIntValue(context, "offset", out int offset))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid or missing parameters 'regionId', 'limit', or 'offset'." });
            return;
        }

        var request = new OrdersByRegionRequest
        {
            RegionId = regionId,
            Limit = limit,
            Offset = offset
        };

        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(validationResult.Errors);
            return;
        }

        await _next(context);
    }

    private bool TryGetRegionId(HttpContext context, out long regionId)
    {
        regionId = 0;
        return context.Request.RouteValues.TryGetValue("regionId", out var value) &&
            value is string stringValue &&
            long.TryParse(stringValue, out regionId);
    }

    private bool TryGetQueryIntValue(HttpContext context, string key, out int intValue)
    {
        intValue = 0;
        return context.Request.Query.TryGetValue(key, out var value) &&
            int.TryParse(value, out intValue);
    }
}
