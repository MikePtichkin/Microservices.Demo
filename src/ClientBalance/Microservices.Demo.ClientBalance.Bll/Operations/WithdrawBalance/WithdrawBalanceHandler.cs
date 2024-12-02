using MediatR;
using Microservices.Demo.ClientBalance.Bll.Exceptions;
using Microservices.Demo.ClientBalance.Domain.Abstractions;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Bll.Balances.WithdrawBalance;

internal class WithdrawBalanceHandler
    : IRequestHandler<WithdrawBalanceCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IOperationsRepository _operationsRepository;

    public WithdrawBalanceHandler(
        IUsersRepository usersRepository,
        IOperationsRepository operationsRepository)
    {
        _usersRepository = usersRepository;
        _operationsRepository = operationsRepository;
    }

    public async Task<bool> Handle(
        WithdrawBalanceCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _usersRepository.Get(
            request.UserId,
            cancellationToken) ?? throw new UserNotFoundException(request.UserId);

        var operation = CreateOperation(request);
        
        if (user.IsWithdrawAvailable(request.Amount))
        {
            await DoSuccessfullWithdraw(request, user, operation, cancellationToken);

            return true;
        }
        else
        {
            operation.Reject();

            await _operationsRepository.Update(
                operation,
                cancellationToken);
        }

        return false;
    }

    private async Task DoSuccessfullWithdraw(WithdrawBalanceCommand request,
        User user,
        Operation operation,
        CancellationToken cancellationToken)
    {
        await using var transactionScope = await _usersRepository.CreateTransactionScope(cancellationToken);

        user.Withdraw(request.Amount);

        await _usersRepository.Update(user, cancellationToken);

        await _operationsRepository.CreateWithdraw(
            operation,
            cancellationToken);

        await transactionScope.CompleteAsync(cancellationToken);
    }

    private Operation CreateOperation(
        WithdrawBalanceCommand request) => new Operation(
            id: request.OperationId,
            userId: request.UserId,
            amount: request.Amount,
            type: OperationType.Withdraw,
            status: OperationStatus.Pending,
            createdAt: DateTimeOffset.Now);
}
