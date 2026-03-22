using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000026)]
public class AddVendorExpiryReturnRequestsTable : Migration
{
    public override void Up()
    {
        Create.Table("VendorExpiryReturnRequests")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("ExpiryRecordId").AsGuid().NotNullable()
            .WithColumn("VendorId").AsGuid().NotNullable()
            .WithColumn("RequestedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("QuantityToReturn").AsInt32().NotNullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("VendorResponseAt").AsDateTimeOffset().Nullable()
            .WithColumn("VendorNotes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_VendorExpiryReturnRequests_ExpiryRecordId")
            .OnTable("VendorExpiryReturnRequests").InSchema("PMS")
            .OnColumn("ExpiryRecordId").Ascending();

        Create.Index("IX_VendorExpiryReturnRequests_VendorId")
            .OnTable("VendorExpiryReturnRequests").InSchema("PMS")
            .OnColumn("VendorId").Ascending();

        Create.Index("IX_VendorExpiryReturnRequests_Status")
            .OnTable("VendorExpiryReturnRequests").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_VendorExpiryReturnRequests_ExpiryRecords_ExpiryRecordId")
            .FromTable("VendorExpiryReturnRequests").InSchema("PMS").ForeignColumn("ExpiryRecordId")
            .ToTable("ExpiryRecords").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_VendorExpiryReturnRequests_Vendors_VendorId")
            .FromTable("VendorExpiryReturnRequests").InSchema("PMS").ForeignColumn("VendorId")
            .ToTable("Vendors").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_VendorExpiryReturnRequests_Vendors_VendorId").OnTable("VendorExpiryReturnRequests").InSchema("PMS");
        Delete.ForeignKey("FK_VendorExpiryReturnRequests_ExpiryRecords_ExpiryRecordId").OnTable("VendorExpiryReturnRequests").InSchema("PMS");
        Delete.Index("IX_VendorExpiryReturnRequests_Status").OnTable("VendorExpiryReturnRequests").InSchema("PMS");
        Delete.Index("IX_VendorExpiryReturnRequests_VendorId").OnTable("VendorExpiryReturnRequests").InSchema("PMS");
        Delete.Index("IX_VendorExpiryReturnRequests_ExpiryRecordId").OnTable("VendorExpiryReturnRequests").InSchema("PMS");
        Delete.Table("VendorExpiryReturnRequests").InSchema("PMS");
    }
}
