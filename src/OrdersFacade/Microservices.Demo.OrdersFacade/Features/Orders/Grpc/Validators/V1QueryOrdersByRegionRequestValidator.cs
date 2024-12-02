using FluentValidation;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Grpc.Validators;

public class V1QueryOrdersByRegionRequestValidator
    : AbstractValidator<V1QueryOrdersByRegionRequest>
{
    public V1QueryOrdersByRegionRequestValidator()
    {
        RuleFor(x => x.RegionId).GreaterThan(0);

        RuleFor(x => x.Limit).GreaterThan(0);

        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}
