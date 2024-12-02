using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class InvalidItemsCountException : OrderServiceExceptionBase
{
    public InvalidItemsCountException()
    {
        ErrorCode = ErrorCode.InvalidItemsCount;

        Message = "Quantity of the items must be greater than 0";
    }
}