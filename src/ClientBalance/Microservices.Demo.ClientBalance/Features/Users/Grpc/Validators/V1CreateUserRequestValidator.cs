using FluentValidation;
using Microservices.Demo.ClientBalance.Protos;

namespace Microservices.Demo.ClientBalance.Features.Users.Grpc.Validators;

public class V1CreateUserRequestValidator : AbstractValidator<V1CreateUserRequest>
{
    public V1CreateUserRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}
