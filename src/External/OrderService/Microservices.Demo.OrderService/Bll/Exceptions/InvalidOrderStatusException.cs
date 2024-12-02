using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class InvalidOrderStatusException : OrderServiceExceptionBase
{
    public InvalidOrderStatusException()
    {
        ErrorCode = ErrorCode.InvalidOrderStatus;

        Message = "Invalid order status";
    }
}