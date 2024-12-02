using FluentMigrator;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Migrations;

[Migration(081024_095000, "AddErrorToOrders")]
public class AddErrorColumnToOrdersTable : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        ALTER TABLE orders
        ADD COLUMN error TEXT;
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        ALTER TABLE orders
        DROP COLUMN IF EXISTS error;
        """;
}
