using FluentMigrator;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using System;

namespace Microservices.Demo.ClientBalance.Infra.Dal.Migrations;

[Migration(1, "CreateUsersTable")]
public class CreateUsersTableMigration : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => """
        CREATE TABLE IF NOT EXISTS users(
            id BIGINT PRIMARY KEY,
            balance DECIMAL(18, 2) NOT NULL
        );
        """;

    protected override string GetDownSql(IServiceProvider services) => """
        DROP TABLE IF EXISTS users;
        """;
}
