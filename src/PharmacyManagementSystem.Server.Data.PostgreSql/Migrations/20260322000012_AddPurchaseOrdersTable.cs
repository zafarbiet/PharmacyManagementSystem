using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000012)]
public class AddPurchaseOrdersTable : Migration
{
    public override void Up()
    {
        Create.Table("PurchaseOrders")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("VendorId").AsGuid().NotNullable()
            .WithColumn("OrderDate").AsDateTimeOffset().NotNullable()
            .WithColumn("Status").AsString(50).NotNullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_PurchaseOrders_VendorId")
            .OnTable("PurchaseOrders").InSchema("PMS")
            .OnColumn("VendorId").Ascending();

        Create.Index("IX_PurchaseOrders_Status")
            .OnTable("PurchaseOrders").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_PurchaseOrders_Vendors_VendorId")
            .FromTable("PurchaseOrders").InSchema("PMS").ForeignColumn("VendorId")
            .ToTable("Vendors").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_PurchaseOrders_Vendors_VendorId").OnTable("PurchaseOrders").InSchema("PMS");
        Delete.Index("IX_PurchaseOrders_Status").OnTable("PurchaseOrders").InSchema("PMS");
        Delete.Index("IX_PurchaseOrders_VendorId").OnTable("PurchaseOrders").InSchema("PMS");
        Delete.Table("PurchaseOrders").InSchema("PMS");
    }
}
