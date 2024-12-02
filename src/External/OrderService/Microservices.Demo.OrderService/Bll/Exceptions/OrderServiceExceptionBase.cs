using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class OrderServiceExceptionBase : Exception
{
    public ErrorCode ErrorCode { get; init; }

    public string Message { get; set; }
}