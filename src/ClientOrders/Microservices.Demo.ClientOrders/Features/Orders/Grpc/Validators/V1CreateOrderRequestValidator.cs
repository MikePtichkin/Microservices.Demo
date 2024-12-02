using FluentValidation;
using Microservices.Demo.ClientOrders.Protos;

namespace Microservices.Demo.ClientOrders.Features.Orders.Grpc.Validators;

public class V1CreateOrderRequestValidator
    : AbstractValidator<V1CreateOrderRequest>
{
    public V1CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
    }
}
