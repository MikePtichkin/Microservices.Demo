using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Microservices.Demo.ClientBalance.Protos;
using System;

namespace Microservices.Demo.ClientBalance.Features.Operations.Grpc.Validators;

public class V1ChangeOperationStatusRequestValidator
    : AbstractValidator<V1ChangeOperationStatusRequest>
{
    public V1ChangeOperationStatusRequestValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();

        RuleFor(x => x.UserId).GreaterThan(0);

        RuleFor(x => x.Type)
            .Must(t => t is OperationType.TopUp || t is OperationType.Withdrawal)
            .WithMessage("Type must be either TopUp or Withdrawal");

        RuleFor(x => x.Status)
            .Must(s => s is OperationStatus.Cancelled || s is OperationStatus.Completed)
            .WithMessage("Status must be either Cancelled or Completed");

        RuleFor(x => x.OccuredAt)
            .Must(BeAValidDate).WithMessage("Occurred At must be a valid date");
    }

    private bool BeAValidDate(Timestamp date) =>
        date is not null && date.ToDateTime().ToUniversalTime() <= DateTime.UtcNow;
}
