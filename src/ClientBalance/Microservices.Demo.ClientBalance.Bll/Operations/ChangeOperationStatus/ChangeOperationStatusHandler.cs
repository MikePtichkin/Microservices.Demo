using MediatR;
using Microservices.Demo.ClientBalance.Bll.Exceptions;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Bll.Operations.ChangeOperationStatus;

internal class ChangeOperationStatusHandler
    : IRequestHandler<ChangeOperationStatusCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IOperationsRepository _operationsRepository;

    public ChangeOperationStatusHandler(
        IUsersRepository usersRepository,
        IOperationsRepository operationRepository)
    {
        _usersRepository = usersRepository;
        _operationsRepository = operationRepository;
    }

    public async Task Handle(
        ChangeOperationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var operation = await _operationsRepository.Get(
            request.OperationId,
            request.Type,
            cancellationToken) ?? throw new OperationNotFoundException(request.OperationId);

        if (request.Status is OperationStatus.Completed)
        {
            await CompleteOperation(operation, request, cancellationToken);
        }

        if (request.Status is OperationStatus.Cancelled)
        {
            await CancelOperation(operation, request, cancellationToken);
        }
    }

    private async Task CompleteOperation(
        Operation operation,
        ChangeOperationStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (operation.Type is OperationType.Withdraw)
        {
            operation.Complete(request.UpdatedAt);
            await _operationsRepository.Update(operation, cancellationToken);

            return;
        }

        if (operation.Type is OperationType.TopUp)
        {
            var user = await _usersRepository.Get(
                operation.UserId,
                cancellationToken) ?? throw new UserNotFoundException(operation.UserId);

            await using var transactionScope = await _operationsRepository.CreateTransactionScope(cancellationToken);

            user.TopUp(operation.Amount);
            operation.Complete(request.UpdatedAt);

            await _usersRepository.Update(user, cancellationToken);
            await _operationsRepository.Update(operation, cancellationToken);

            await transactionScope.CompleteAsync(cancellationToken);
        }
    }

    private async Task CancelOperation(
        Operation operation,
        ChangeOperationStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (operation.Type is OperationType.TopUp)
        {
            operation.Cancel(request.UpdatedAt);
            return;
        }

        if (operation.Type is OperationType.Withdraw)
        {
            var user = await _usersRepository.Get(
                    operation.UserId,
                    cancellationToken) ?? throw new UserNotFoundException(operation.UserId);

            user.Refund(operation.Amount);
            await _usersRepository.Update(user, cancellationToken);

            operation.Cancel(request.UpdatedAt);
        }
    }
}
