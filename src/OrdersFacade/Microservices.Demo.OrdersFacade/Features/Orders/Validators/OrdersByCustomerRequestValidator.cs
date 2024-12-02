using FluentValidation;
using Microservices.Demo.OrdersFacade.Features.Orders.Requests;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Validators;

public class OrdersByCustomerRequestValidator : AbstractValidator<OrdersByCustomerRequest>
{
    public OrdersByCustomerRequestValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);

        RuleFor(x => x.Limit).GreaterThan(0);

        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}
