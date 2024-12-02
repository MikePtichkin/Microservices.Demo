using Microservices.Demo.OrderService.Proto;

namespace Microservices.Demo.OrderService.Bll.Exceptions;

public class UnsupportedRegionException : OrderServiceExceptionBase
{
    public UnsupportedRegionException(long regionId)
    {
        ErrorCode = ErrorCode.UnsupportedRegion;

        Message = $"Unsupported region: {regionId}";
    }
}