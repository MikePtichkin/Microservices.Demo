using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Common;

public abstract class SqlMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = GetUpSql(context.ServiceProvider)
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = GetDownSql(context.ServiceProvider)
        });
    }

    protected abstract string GetUpSql(IServiceProvider services);
    protected abstract string GetDownSql(IServiceProvider services);

    public object ApplicationContext => throw new NotImplementedException();
    public string ConnectionString => throw new NotImplementedException();
}
