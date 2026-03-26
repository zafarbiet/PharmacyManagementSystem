using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260325000002)]
public class AddManufacturersQuotationVendorResponsesPromotionsTables : Migration
{
    public override void Up()
    {
        // Manufacturers table
        Create.Table("Manufacturers").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("ContactEmail").AsString(200).Nullable()
            .WithColumn("ContactPhone").AsString(50).Nullable()
            .WithColumn("Country").AsString(100).Nullable()
            .WithColumn("Address").AsString(500).Nullable()
            .WithColumn("Gstin").AsString(20).Nullable()
            .WithColumn("DrugLicenseNumber").AsString(100).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(100).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Manufacturers_Name").OnTable("Manufacturers").InSchema("PMS")
            .OnColumn("Name").Ascending();

        // QuotationVendorResponses table
        Create.Table("QuotationVendorResponses").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("QuotationRequestId").AsGuid().NotNullable()
            .WithColumn("VendorId").AsGuid().NotNullable()
            .WithColumn("Status").AsString(50).NotNullable().WithDefaultValue("pending")
            .WithColumn("RespondedAt").AsDateTimeOffset().Nullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(100).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_QuotationVendorResponses_QuotationRequests")
            .FromTable("QuotationVendorResponses").InSchema("PMS").ForeignColumn("QuotationRequestId")
            .ToTable("QuotationRequests").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_QuotationVendorResponses_Vendors")
            .FromTable("QuotationVendorResponses").InSchema("PMS").ForeignColumn("VendorId")
            .ToTable("Vendors").InSchema("PMS").PrimaryColumn("Id");

        Create.Index("IX_QuotationVendorResponses_QuotationRequestId").OnTable("QuotationVendorResponses").InSchema("PMS")
            .OnColumn("QuotationRequestId").Ascending();

        Create.Index("IX_QuotationVendorResponses_VendorId").OnTable("QuotationVendorResponses").InSchema("PMS")
            .OnColumn("VendorId").Ascending();

        // Promotions table
        Create.Table("Promotions").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Description").AsString(1000).Nullable()
            .WithColumn("DiscountPercentage").AsDecimal(5, 2).NotNullable()
            .WithColumn("MaxDiscountAmount").AsDecimal(18, 2).Nullable()
            .WithColumn("ValidFrom").AsDateTimeOffset().NotNullable()
            .WithColumn("ValidTo").AsDateTimeOffset().NotNullable()
            .WithColumn("ApplicableDrugId").AsGuid().Nullable()
            .WithColumn("ApplicableCategoryId").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(100).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Promotions_ValidFrom_ValidTo").OnTable("Promotions").InSchema("PMS")
            .OnColumn("ValidFrom").Ascending()
            .OnColumn("ValidTo").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Promotions_ValidFrom_ValidTo").OnTable("Promotions").InSchema("PMS");
        Delete.Table("Promotions").InSchema("PMS");

        Delete.Index("IX_QuotationVendorResponses_VendorId").OnTable("QuotationVendorResponses").InSchema("PMS");
        Delete.Index("IX_QuotationVendorResponses_QuotationRequestId").OnTable("QuotationVendorResponses").InSchema("PMS");
        Delete.ForeignKey("FK_QuotationVendorResponses_Vendors").OnTable("QuotationVendorResponses").InSchema("PMS");
        Delete.ForeignKey("FK_QuotationVendorResponses_QuotationRequests").OnTable("QuotationVendorResponses").InSchema("PMS");
        Delete.Table("QuotationVendorResponses").InSchema("PMS");

        Delete.Index("IX_Manufacturers_Name").OnTable("Manufacturers").InSchema("PMS");
        Delete.Table("Manufacturers").InSchema("PMS");
    }
}
