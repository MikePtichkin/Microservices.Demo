using FluentValidation;
using Google.Type;

namespace Microservices.Demo.ClientBalance.Infrastructure.Validators;

public class MoneyValidator : AbstractValidator<Money>
{
    public MoneyValidator()
    {
        RuleFor(m => m.CurrencyCode)
            .NotEmpty()
            .Matches("^[A-Z]{3}$").WithMessage("Currency code must have three capital letters");

        RuleFor(m => m.Units)
            .GreaterThanOrEqualTo(0);

        RuleFor(m => m.Nanos)
            .InclusiveBetween(0, 999_999_999).WithMessage("Nanos must be between 0 and 999999999");
    }
}
