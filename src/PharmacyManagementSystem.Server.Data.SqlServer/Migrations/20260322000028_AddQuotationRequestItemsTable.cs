using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000028)]
public class AddQuotationRequestItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("QuotationRequestItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("QuotationRequestId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("QuantityRequired").AsInt32().NotNullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_QuotationRequestItems_QuotationRequestId")
            .OnTable("QuotationRequestItems").InSchema("PMS")
            .OnColumn("QuotationRequestId").Ascending();

        Create.Index("IX_QuotationRequestItems_DrugId")
            .OnTable("QuotationRequestItems").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.ForeignKey("FK_QuotationRequestItems_QuotationRequests_QuotationRequestId")
            .FromTable("QuotationRequestItems").InSchema("PMS").ForeignColumn("QuotationRequestId")
            .ToTable("QuotationRequests").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_QuotationRequestItems_Drugs_DrugId")
            .FromTable("QuotationRequestItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_QuotationRequestItems_Drugs_DrugId").OnTable("QuotationRequestItems").InSchema("PMS");
        Delete.ForeignKey("FK_QuotationRequestItems_QuotationRequests_QuotationRequestId").OnTable("QuotationRequestItems").InSchema("PMS");
        Delete.Index("IX_QuotationRequestItems_DrugId").OnTable("QuotationRequestItems").InSchema("PMS");
        Delete.Index("IX_QuotationRequestItems_QuotationRequestId").OnTable("QuotationRequestItems").InSchema("PMS");
        Delete.Table("QuotationRequestItems").InSchema("PMS");
    }
}
