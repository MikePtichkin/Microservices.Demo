using FluentMigrator;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Migrations;

[Migration(081024_104000, "AddOrdersCustomerIdIndex")]
public class AddIndexToOrdersTable : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE INDEX IF NOT EXISTS ix_orders_customer_id
        ON orders(customer_id);
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        DROP INDEX IF EXISTS ix_orders_customer_id;
        """;
}
