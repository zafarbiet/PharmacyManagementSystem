using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000043)]
public class AlterDrugPricingAddMrpHsn : Migration
{
    public override void Up()
    {
        Alter.Table("DrugPricing").InSchema("PMS")
            .AddColumn("Mrp").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("HsnCode").AsString(20).Nullable();
    }

    public override void Down()
    {
        Delete.Column("Mrp").FromTable("DrugPricing").InSchema("PMS");
        Delete.Column("HsnCode").FromTable("DrugPricing").InSchema("PMS");
    }
}
