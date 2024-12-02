using System.ComponentModel.DataAnnotations;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Microservices.Demo.OrderService.Grpc.Interceptors;

public sealed class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await continuation(request, context);
            return response;
        }
        catch (RpcException)
        {
            throw;
        }
        catch (ValidationException exception)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, exception.Message));
        }
        catch (Exception exception)
        {
            throw new RpcException(new Status(StatusCode.Internal, exception.Message));
        }
    }
}
