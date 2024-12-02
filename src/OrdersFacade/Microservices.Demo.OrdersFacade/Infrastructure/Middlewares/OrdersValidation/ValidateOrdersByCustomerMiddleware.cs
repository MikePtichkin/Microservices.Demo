using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microservices.Demo.OrdersFacade.Features.Orders.Requests;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Infrastructure.Middlewares.OrdersValidation;

public class ValidateOrdersByCustomerMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateOrdersByCustomerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IValidator<OrdersByCustomerRequest> validator)
    {
        if (!TryGetCustomerId(context, out long customerId) ||
            !TryGetQueryIntValue(context, "limit", out int limit) ||
            !TryGetQueryIntValue(context, "offset", out int offset))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid or missing parameters");
            return;
        }

        var requestModel = new OrdersByCustomerRequest
        {
            CustomerId = customerId,
            Limit = limit,
            Offset = offset
        };

        var validationResult = validator.Validate(requestModel);
        if (!validationResult.IsValid)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(validationResult.Errors);
            return;
        }

        await _next(context);
    }

    private bool TryGetCustomerId(HttpContext context, out long customerId)
    {
        customerId = 0;
        return context.Request.RouteValues.TryGetValue("customerId", out var value) &&
            value is string stringValue &&
            long.TryParse(stringValue, out customerId);
    }

    private bool TryGetQueryIntValue(HttpContext context, string key, out int intValue)
    {
        intValue = 0;
        return context.Request.Query.TryGetValue(key, out var value) &&
            int.TryParse(value, out intValue);
    }
}
