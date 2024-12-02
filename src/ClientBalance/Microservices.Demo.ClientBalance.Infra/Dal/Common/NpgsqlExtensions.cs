using Npgsql;
using System.Data.Common;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Common;

public static class NpgsqlExtensions
{
    public static void Add<T>(this DbParameterCollection parameters, string name, T? value) =>
        parameters.Add(new NpgsqlParameter<T>
        {
            ParameterName = name,
            TypedValue = value
        });
}
