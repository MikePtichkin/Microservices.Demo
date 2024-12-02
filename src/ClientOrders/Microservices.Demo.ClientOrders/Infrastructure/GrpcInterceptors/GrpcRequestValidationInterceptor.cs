using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientOrders.Infrastructure.GrpcInterceptors;

public class GrpcRequestValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public GrpcRequestValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var validators = _serviceProvider.GetServices<IValidator<TRequest>>();
        foreach (var validator in validators)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        var response = await continuation(request, context);
        return response;
    }
}
