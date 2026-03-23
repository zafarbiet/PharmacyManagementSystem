using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000049)]
public class AlterPurchaseOrdersAddPoNumberApproval : Migration
{
    public override void Up()
    {
        Alter.Table("PurchaseOrders").InSchema("PMS")
            .AddColumn("PoNumber").AsString(50).Nullable()
            .AddColumn("ApprovedBy").AsString(100).Nullable()
            .AddColumn("ApprovedAt").AsDateTimeOffset().Nullable()
            .AddColumn("ParentPurchaseOrderId").AsGuid().Nullable()
            .AddColumn("BranchId").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Column("PoNumber").FromTable("PurchaseOrders").InSchema("PMS");
        Delete.Column("ApprovedBy").FromTable("PurchaseOrders").InSchema("PMS");
        Delete.Column("ApprovedAt").FromTable("PurchaseOrders").InSchema("PMS");
        Delete.Column("ParentPurchaseOrderId").FromTable("PurchaseOrders").InSchema("PMS");
        Delete.Column("BranchId").FromTable("PurchaseOrders").InSchema("PMS");
    }
}
