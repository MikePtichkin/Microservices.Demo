using FluentValidation;
using Microservices.Demo.ClientOrders.Protos;

namespace Microservices.Demo.ClientOrders.Features.Orders.Grpc.Validators;

public class V1QueryOrdersRequestValidator
    : AbstractValidator<V1QueryOrdersRequest>
{
    public V1QueryOrdersRequestValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.Limit).GreaterThan(0);
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}
