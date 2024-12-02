using FluentMigrator;
using Microservices.Demo.ClientOrders.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientOrders.Infra.Dal.Migrations;

[Migration(041024_150002, "CreateOrdersTable")]
public class CreateOrdersTable : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE TABLE IF NOT EXISTS orders(
            id BIGSERIAL PRIMARY KEY,
            region_id BIGINT NOT NULL,
            customer_id BIGINT NOT NULL,
            status INT NOT NULL,
            created_at TIMESTAMPTZ NOT NULL,
            comment TEXT
        );
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        DROP TABLE IF EXISTS orders;
        """;
}