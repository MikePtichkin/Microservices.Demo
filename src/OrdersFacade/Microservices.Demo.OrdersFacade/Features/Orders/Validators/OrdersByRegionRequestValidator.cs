using FluentValidation;
using Microservices.Demo.OrdersFacade.Features.Orders.Requests;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Validators;

public class OrdersByRegionRequestValidator : AbstractValidator<OrdersByRegionRequest>
{
    public OrdersByRegionRequestValidator()
    {
        RuleFor(x => x.RegionId).GreaterThan(0);

        RuleFor(x => x.Limit).GreaterThan(0);

        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}
