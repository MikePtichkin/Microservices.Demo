using MediatR;
using System;

namespace Microservices.Demo.ClientBalance.Bll.Balances.WithdrawBalance;

public sealed record WithdrawBalanceCommand(
    Guid OperationId,
    long UserId,
    decimal Amount,
    DateTimeOffset CreatedAt) : IRequest<bool>;
