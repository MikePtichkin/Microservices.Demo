using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Domain.Abstractions;

public interface IBaseRepository
{
    Task<ITransactionScope> CreateTransactionScope(CancellationToken cancellationToken);
}

public interface ITransactionScope : IAsyncDisposable
{
    Task CompleteAsync(CancellationToken cancellationToken);
}
