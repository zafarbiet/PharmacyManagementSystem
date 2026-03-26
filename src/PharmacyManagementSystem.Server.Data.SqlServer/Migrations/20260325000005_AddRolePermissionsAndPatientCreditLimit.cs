using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260325000005)]
public class AddRolePermissionsAndPatientCreditLimit : Migration
{
    public override void Up()
    {
        Alter.Table("Roles").InSchema("PMS")
            .AddColumn("Permissions").AsInt64().NotNullable().WithDefaultValue(0);

        Alter.Table("Patients").InSchema("PMS")
            .AddColumn("CreditLimit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("Permissions").FromTable("Roles").InSchema("PMS");
        Delete.Column("CreditLimit").FromTable("Patients").InSchema("PMS");
    }
}
