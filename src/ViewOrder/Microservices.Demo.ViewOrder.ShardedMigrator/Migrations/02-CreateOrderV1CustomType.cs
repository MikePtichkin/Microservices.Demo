using FluentMigrator;
using Microservices.Demo.ViewOrder.ShardedMigrator.Infrastructure.Migrator;
using System;

namespace Microservices.Demo.ViewOrder.ShardedMigrator.Migrations;

[Migration(151024_153001, "Create order_v1 custom type")]
public class CreateOrderV1CustomType : ShardSqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        SET search_path TO public;

        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'order_v1') THEN
                    CREATE TYPE order_v1 as
                    (
                        order_id BIGINT,
                        region_id BIGINT,
                        status INT,
                        customer_id BIGINT,
                        comment TEXT,
                        created_at TIMESTAMPTZ
                    );
                END IF;
            END
        $$
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        SET search_path TO public;
        DROP TYPE IF EXISTS order_v1;
        """;
}
