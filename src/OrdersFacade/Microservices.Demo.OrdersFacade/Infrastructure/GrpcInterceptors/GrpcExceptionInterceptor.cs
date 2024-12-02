using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microservices.Demo.OrdersFacade.Application.Exceptions;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.OrdersFacade.Infrastructure.GrpcInterceptors;

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
        catch (ValidationException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (CustomerNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}
