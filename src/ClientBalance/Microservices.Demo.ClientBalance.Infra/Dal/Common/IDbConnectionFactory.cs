using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Common;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> GetConnection(CancellationToken token);
}
