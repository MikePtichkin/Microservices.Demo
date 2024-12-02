using MediatR;

namespace Microservices.Demo.ClientBalance.Bll.Users.GetBalance;

public sealed record GetBalanceQuery(
    long UserId) : IRequest<decimal>;
