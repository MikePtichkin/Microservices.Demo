using FluentValidation;
using Microservices.Demo.OrderService.Proto.OrderGrpc;

namespace Microservices.Demo.OrderService.Grpc.Validators;

public class V1DeliveryOrderRequestValidator : AbstractValidator<V1DeliveryOrderRequest>
{
    public V1DeliveryOrderRequestValidator()
    {
        RuleFor(query => query.OrderId)
            .GreaterThan(0);
    }
}