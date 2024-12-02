using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class OrderNotFoundException : OrderServiceExceptionBase
{
    
    public OrderNotFoundException(long orderId)
    {
        ErrorCode = ErrorCode.OrderNotFound;

        Message = $"Order {orderId} is not found";
    }
}