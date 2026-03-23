using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000048)]
public class AlterVendorsAddLicenseCreditTerms : Migration
{
    public override void Up()
    {
        Alter.Table("Vendors").InSchema("PMS")
            .AddColumn("DrugLicenseNumber").AsString(50).Nullable()
            .AddColumn("CreditTermsDays").AsInt32().NotNullable().WithDefaultValue(0)
            .AddColumn("CreditLimit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("OutstandingBalance").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("DrugLicenseNumber").FromTable("Vendors").InSchema("PMS");
        Delete.Column("CreditTermsDays").FromTable("Vendors").InSchema("PMS");
        Delete.Column("CreditLimit").FromTable("Vendors").InSchema("PMS");
        Delete.Column("OutstandingBalance").FromTable("Vendors").InSchema("PMS");
    }
}
