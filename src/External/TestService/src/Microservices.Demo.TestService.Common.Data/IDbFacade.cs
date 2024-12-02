using Dapper;

namespace Microservices.Demo.TestService.Common.Data;

public interface IDbFacade
{
    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command);
}

public interface IDbFacade<TOptions> : IDbFacade
    where TOptions : DataConnectionOptions
{
}
