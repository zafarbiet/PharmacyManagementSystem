using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000007)]
public class AddDrugPricingTable : Migration
{
    public override void Up()
    {
        Create.Table("DrugPricing")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("CostPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("SellingPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("Discount").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("GstRate").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("EffectiveFrom").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_DrugPricing_Drugs_DrugId")
            .FromTable("DrugPricing").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DrugPricing_Drugs_DrugId").OnTable("DrugPricing").InSchema("PMS");
        Delete.Table("DrugPricing").InSchema("PMS");
    }
}
