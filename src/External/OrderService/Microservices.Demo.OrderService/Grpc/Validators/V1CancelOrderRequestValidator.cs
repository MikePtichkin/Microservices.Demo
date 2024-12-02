using FluentValidation;
using Microservices.Demo.OrderService.Proto.OrderGrpc;

namespace Microservices.Demo.OrderService.Grpc.Validators;

public class V1CancelOrderRequestValidator : AbstractValidator<V1CancelOrderRequest>
{
    public V1CancelOrderRequestValidator()
    {
        RuleFor(query => query.OrderId)
            .GreaterThan(0);
    }
}