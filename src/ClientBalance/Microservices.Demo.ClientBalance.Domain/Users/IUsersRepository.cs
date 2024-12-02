using Microservices.Demo.ClientBalance.Domain.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Domain.Users;

public interface IUsersRepository : IBaseRepository
{
    Task<User?> Get(long id, CancellationToken cancellationToken);

    Task<long?> Create(User user, CancellationToken cancellationToken);

    IAsyncEnumerable<long> Create(User[] users, CancellationToken cancellationToken);

    Task<int> Update(User user, CancellationToken cancellationToken);
}
