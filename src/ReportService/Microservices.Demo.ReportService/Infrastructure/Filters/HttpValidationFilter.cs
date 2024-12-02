using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservices.Demo.ReportService.Infrastructure.Filters;

internal sealed class HttpValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        context.Result = new BadRequestObjectResult(context.ModelState);
    }
}
