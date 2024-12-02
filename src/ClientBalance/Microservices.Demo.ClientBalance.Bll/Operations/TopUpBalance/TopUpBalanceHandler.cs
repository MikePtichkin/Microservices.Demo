using MediatR;
using Microservices.Demo.ClientBalance.Bll.Exceptions;
using Microservices.Demo.ClientBalance.Domain.Operations;
using Microservices.Demo.ClientBalance.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Bll.Operations.TopUpBalance;

internal class TopUpBalanceHandler
    : IRequestHandler<TopUpBalanceCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IOperationsRepository _operationsRepository;

    public TopUpBalanceHandler(
        IUsersRepository usersRepository,
        IOperationsRepository operationRepository)
    {
        _usersRepository = usersRepository;
        _operationsRepository = operationRepository;
    }

    public async Task Handle(
        TopUpBalanceCommand request,
        CancellationToken cancellationToken)
    {
        _ = await _usersRepository.Get(
            request.UserId,
            cancellationToken) ?? throw new UserNotFoundException(request.UserId);

        await CreateOperation(request, cancellationToken);
    }

    private async Task CreateOperation(
        TopUpBalanceCommand request,
        CancellationToken cancellationToken)
    {
        var operation = new Operation(
            id: request.OperationId,
            userId: request.UserId,
            amount: request.Amount,
            type: OperationType.TopUp,
            status: OperationStatus.Pending,
            createdAt: request.CreatedAt);

        await _operationsRepository.CreateTopUp(
            operation,
            cancellationToken);
    }
}
