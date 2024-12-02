using FluentMigrator;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;
using System;

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Migrations;

[Migration(151024_153000, "Create sharded orders table")]
public class CreateOrdersTable : ShardSqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE TABLE IF NOT EXISTS orders (
            order_id BIGINT primary key,
            region_id BIGINT not null,
            status INT not null,
            customer_id BIGINT not null,
            comment text null,
            created_at TIMESTAMPTZ
        );

        CREATE INDEX IF NOT EXISTS ix_orders_customer_id ON orders (customer_id)
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        DROP TABLE IF EXISTS orders;
        """;
}
