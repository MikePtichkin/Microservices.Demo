using Microsoft.AspNetCore.Builder;
using Microservices.Demo.OrdersFacade.Infrastructure.Middlewares.OrdersValidation;

namespace Microservices.Demo.OrdersFacade.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseValidationMiddleware(this IApplicationBuilder app)
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/v1/orders/customer"), appBuilder =>
        {
            appBuilder.UseMiddleware<ValidateOrdersByCustomerMiddleware>();
        });

        app.UseWhen(context => context.Request.Path.StartsWithSegments("/v1/orders/region"), appBuilder =>
        {
            appBuilder.UseMiddleware<ValidateOrdersByRegionMiddleware>();
        });

        return app;
    }
}
