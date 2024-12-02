using MediatR;
using Microservices.Demo.ClientBalance.Bll.Exceptions;
using Microservices.Demo.ClientBalance.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Bll.Users.GetBalance;

internal class GetBalanceHandler
    : IRequestHandler<GetBalanceQuery, decimal>
{
    private readonly IUsersRepository _usersRepository;

    public GetBalanceHandler(
        IUsersRepository userRepository)
    {
        _usersRepository = userRepository;
    }

    public async Task<decimal> Handle(
        GetBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _usersRepository.Get(
            request.UserId,
            cancellationToken) ?? throw new UserNotFoundException(request.UserId);

        return user.Balance;
    }
}
