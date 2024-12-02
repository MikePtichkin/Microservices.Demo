using FluentValidation;
using Microservices.Demo.ClientBalance.Protos;

namespace Microservices.Demo.ClientBalance.Features.Users.Grpc.Validators;

public class V1GetBalanceRequestValidator : AbstractValidator<V1GetBalanceRequest>
{
    public V1GetBalanceRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}
