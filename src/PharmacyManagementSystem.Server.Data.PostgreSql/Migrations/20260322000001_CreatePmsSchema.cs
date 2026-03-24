using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000001)]
public class CreatePmsSchema : Migration
{
    public override void Up() => Execute.Sql("CREATE SCHEMA PMS");

    public override void Down() => Execute.Sql("DROP SCHEMA PMS");
}
