using System.Threading.Tasks;
using System.Threading;
using System;
using Microservices.Demo.ClientBalance.Domain.Abstractions;

namespace Microservices.Demo.ClientBalance.Domain.Operations;

public interface IOperationsRepository : IBaseRepository
{
    Task<Operation?> Get(Guid id, OperationType type, CancellationToken cancellationToken);
    Task CreateTopUp(Operation operation, CancellationToken cancellationToken);
    Task CreateWithdraw(Operation operation, CancellationToken cancellationToken);
    Task Update(Operation operation, CancellationToken cancellationToken);
}
