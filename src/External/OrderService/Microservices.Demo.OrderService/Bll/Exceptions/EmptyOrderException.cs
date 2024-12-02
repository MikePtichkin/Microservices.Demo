using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class EmptyOrderException : OrderServiceExceptionBase
{
    public EmptyOrderException()
    {
        ErrorCode = ErrorCode.EmptyOrder;
        
        Message = "Order without items";
    }
}