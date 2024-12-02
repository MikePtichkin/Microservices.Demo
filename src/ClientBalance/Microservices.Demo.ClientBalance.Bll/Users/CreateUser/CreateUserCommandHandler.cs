using MediatR;
using Microservices.Demo.ClientBalance.Bll.Exceptions;
using Microservices.Demo.ClientBalance.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Bll.Users.CreateUser;

internal class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand>
{
    private readonly IUsersRepository _userRepository;

    public CreateUserCommandHandler(
        IUsersRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = new User(request.UserId);

        _ = await _userRepository.Create(
            user,
            cancellationToken) ?? throw new UserNotCreatedExcepton(request.UserId);
    }
}
