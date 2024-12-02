using System.Data.Common;

namespace Microservices.Demo.TestService.Common.Data;

public interface IPostgresConnectionFactory<TOptions>
    where TOptions : DataConnectionOptions
{
    DbConnection GetConnection();
}
