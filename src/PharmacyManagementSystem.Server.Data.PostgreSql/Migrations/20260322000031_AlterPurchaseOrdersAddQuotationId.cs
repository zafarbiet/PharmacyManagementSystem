using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000031)]
public class AlterPurchaseOrdersAddQuotationId : Migration
{
    public override void Up()
    {
        Alter.Table("PurchaseOrders").InSchema("PMS").AddColumn("QuotationId").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Column("QuotationId").FromTable("PurchaseOrders").InSchema("PMS");
    }
}
