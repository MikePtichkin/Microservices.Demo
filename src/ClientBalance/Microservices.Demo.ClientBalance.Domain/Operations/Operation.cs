using Microservices.Demo.ClientBalance.Domain.Abstractions;
using System;

namespace Microservices.Demo.ClientBalance.Domain.Operations;

public sealed class Operation : Entity<Guid>
{
    public Operation(
        Guid id,
        long userId,
        decimal amount,
        OperationType type,
        OperationStatus status,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt = null)
        : base(id)
    {
        UserId = userId;
        Amount = amount;
        Type = type;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public long UserId { get; init; }
    public decimal Amount { get; init; }
    public OperationType Type { get; init; }
    public OperationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public void Complete(DateTimeOffset updatedAt)
    {
        if (Status is not OperationStatus.Pending)
            throw new InvalidOperationException("Cannot complete operation that is not pending.");

        UpdatedAt = updatedAt;
        Status = OperationStatus.Completed;
    }

    public void Cancel(DateTimeOffset updatedAt)
    {
        if (Status is not OperationStatus.Pending)
            throw new InvalidOperationException("Cannot cancel operation that is not pending.");

        UpdatedAt = updatedAt;
        Status = OperationStatus.Cancelled;
    }

    public void Reject()
    {
        if (Status is not OperationStatus.Pending)
            throw new InvalidOperationException("Cannot reject operation that is not pending.");

        Status = OperationStatus.Rejected;
    }
}