using MediatR;
using System;

namespace Microservices.Demo.ClientBalance.Bll.Operations.TopUpBalance;

public record TopUpBalanceCommand(
    Guid OperationId,
    long UserId,
    decimal Amount,
    DateTimeOffset CreatedAt) : IRequest;
