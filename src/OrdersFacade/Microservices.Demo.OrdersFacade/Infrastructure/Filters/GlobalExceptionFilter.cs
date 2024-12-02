using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microservices.Demo.OrdersFacade.Application.Exceptions;

namespace Microservices.Demo.OrdersFacade.Infrastructure.Filters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        if (context.Exception is CustomerNotFoundException ex)
        {

            context.Result = new NotFoundObjectResult(new { message = ex.Message });
            context.ExceptionHandled = true;
        }
        else
        {
            // Обработка непредвиденных исключений
            context.Result = new ObjectResult(new { message = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
