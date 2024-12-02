using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Microservices.Demo.ClientBalance.Infrastructure.Validators;
using Microservices.Demo.ClientBalance.Protos;
using System;

namespace Microservices.Demo.ClientBalance.Features.Operations.Grpc.Validators;

public class V1WithdrawBalanceRequestValidator : AbstractValidator<V1WithdrawBalanceRequest>
{
    public V1WithdrawBalanceRequestValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();

        RuleFor(x => x.UserId).GreaterThan(0);

        RuleFor(x => x.Amount)
            .NotNull()
            .SetValidator(new MoneyValidator());

        RuleFor(x => x.OccuredAt)
            .NotNull()
            .Must(BeAValidDate).WithMessage("Occurred At must be a valid date and cannot be in the future");
    }

    private bool BeAValidDate(Timestamp date) =>
        date.ToDateTime().ToUniversalTime() <= DateTime.UtcNow;
}
