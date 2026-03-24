using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000029)]
public class AddQuotationsTable : Migration
{
    public override void Up()
    {
        Create.Table("Quotations")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("QuotationRequestId").AsGuid().Nullable()
            .WithColumn("VendorId").AsGuid().NotNullable()
            .WithColumn("QuotationDate").AsDateTimeOffset().NotNullable()
            .WithColumn("ValidUntil").AsDateTimeOffset().Nullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Quotations_QuotationRequestId")
            .OnTable("Quotations").InSchema("PMS")
            .OnColumn("QuotationRequestId").Ascending();

        Create.Index("IX_Quotations_VendorId")
            .OnTable("Quotations").InSchema("PMS")
            .OnColumn("VendorId").Ascending();

        Create.Index("IX_Quotations_Status")
            .OnTable("Quotations").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_Quotations_QuotationRequests_QuotationRequestId")
            .FromTable("Quotations").InSchema("PMS").ForeignColumn("QuotationRequestId")
            .ToTable("QuotationRequests").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_Quotations_Vendors_VendorId")
            .FromTable("Quotations").InSchema("PMS").ForeignColumn("VendorId")
            .ToTable("Vendors").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Quotations_Vendors_VendorId").OnTable("Quotations").InSchema("PMS");
        Delete.ForeignKey("FK_Quotations_QuotationRequests_QuotationRequestId").OnTable("Quotations").InSchema("PMS");
        Delete.Index("IX_Quotations_Status").OnTable("Quotations").InSchema("PMS");
        Delete.Index("IX_Quotations_VendorId").OnTable("Quotations").InSchema("PMS");
        Delete.Index("IX_Quotations_QuotationRequestId").OnTable("Quotations").InSchema("PMS");
        Delete.Table("Quotations").InSchema("PMS");
    }
}
