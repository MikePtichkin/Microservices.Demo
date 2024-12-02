using FluentMigrator;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Migrations;

[Migration(4, "CreateWithdrawsTable")]
public class CreateWithdrawsTableMigration : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE TABLE IF NOT EXISTS withdraws(
            id UUID PRIMARY KEY,
            user_id BIGINT NOT NULL,
            amount DECIMAL(18, 2) NOT NULL,
            type operation_type NOT NULL,
            status operation_status NOT NULL,
            created_at TIMESTAMP WITH TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITH TIME ZONE
        );
        """;


    protected override string GetDownSql(IServiceProvider services) => """
        DROP TABLE IF EXISTS withdraws;
        """;

}
