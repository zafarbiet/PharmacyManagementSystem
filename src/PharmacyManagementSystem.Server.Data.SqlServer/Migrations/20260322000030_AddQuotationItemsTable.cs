using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000030)]
public class AddQuotationItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("QuotationItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("QuotationId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("QuantityOffered").AsInt32().NotNullable()
            .WithColumn("UnitPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("TotalPrice").AsDecimal(18, 2).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_QuotationItems_QuotationId")
            .OnTable("QuotationItems").InSchema("PMS")
            .OnColumn("QuotationId").Ascending();

        Create.Index("IX_QuotationItems_DrugId")
            .OnTable("QuotationItems").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.ForeignKey("FK_QuotationItems_Quotations_QuotationId")
            .FromTable("QuotationItems").InSchema("PMS").ForeignColumn("QuotationId")
            .ToTable("Quotations").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_QuotationItems_Drugs_DrugId")
            .FromTable("QuotationItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_QuotationItems_Drugs_DrugId").OnTable("QuotationItems").InSchema("PMS");
        Delete.ForeignKey("FK_QuotationItems_Quotations_QuotationId").OnTable("QuotationItems").InSchema("PMS");
        Delete.Index("IX_QuotationItems_DrugId").OnTable("QuotationItems").InSchema("PMS");
        Delete.Index("IX_QuotationItems_QuotationId").OnTable("QuotationItems").InSchema("PMS");
        Delete.Table("QuotationItems").InSchema("PMS");
    }
}
