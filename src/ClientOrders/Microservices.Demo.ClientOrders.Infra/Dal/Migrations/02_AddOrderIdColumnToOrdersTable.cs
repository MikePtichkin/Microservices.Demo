using FluentMigrator;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Migrations;

[Migration(071024_163000, "AddOrderIdToOrders")]
public class AddOrderIdColumnToOrdersTable : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        ALTER TABLE orders
        ADD COLUMN order_id BIGINT;
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        ALTER TABLE orders
        DROP COLUMN IF EXISTS order_id;
        """;
}