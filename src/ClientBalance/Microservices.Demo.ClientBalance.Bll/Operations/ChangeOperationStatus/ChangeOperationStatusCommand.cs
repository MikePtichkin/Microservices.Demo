using MediatR;
using Microservices.Demo.ClientBalance.Domain.Operations;
using System;

namespace Microservices.Demo.ClientBalance.Bll.Operations.ChangeOperationStatus;

// Стоит написать валидатор проверяющий статус. Допустимые значения completed и cancelled
public sealed record ChangeOperationStatusCommand(
    Guid OperationId,
    OperationType Type,
    long UserId,
    OperationStatus Status,
    DateTimeOffset UpdatedAt) : IRequest;
