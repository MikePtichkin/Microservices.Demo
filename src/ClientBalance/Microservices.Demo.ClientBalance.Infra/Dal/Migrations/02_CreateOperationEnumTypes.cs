using FluentMigrator;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Migrations;

[Migration(2, "CreateOperationEnums")]
public class CreateOperationEnumTypesMigration : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE TYPE operation_type AS ENUM
            ('top_up', 'withdraw');

        CREATE TYPE operation_status AS ENUM
            ('pending', 'cancelled', 'completed', 'rejected');
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        DROP TYPE operation_status;
        DROP TYPE operation_type;
        """;
}
