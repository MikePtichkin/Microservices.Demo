using FluentValidation;

namespace Microservices.Demo.OrdersFacade.Features.Orders.Grpc.Validators;

public class V1QueryOrdersByCustomerRequestValidator : AbstractValidator<V1QueryOrdersByCustomerRequest>
{
    public V1QueryOrdersByCustomerRequestValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);

        RuleFor(x => x.Limit).GreaterThan(0);

        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}
