using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Microservices.Demo.ReportService.Infrastructure.Filters;

internal sealed class GlobalExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        if (context.Exception is OperationCanceledException ||
            context.Exception.InnerException is OperationCanceledException)
        {
            context.Result = new StatusCodeResult(499);
            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = new ObjectResult(new { message = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
