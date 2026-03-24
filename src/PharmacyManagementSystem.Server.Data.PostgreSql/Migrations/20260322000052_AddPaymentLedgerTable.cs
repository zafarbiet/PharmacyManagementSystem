using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000052)]
public class AddPaymentLedgerTable : Migration
{
    public override void Up()
    {
        Create.Table("PaymentLedger").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("VendorId").AsGuid().NotNullable()
            .WithColumn("PurchaseOrderId").AsGuid().Nullable()
            .WithColumn("InvoicedAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("PaidAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("DueDate").AsDateTimeOffset().NotNullable()
            .WithColumn("Status").AsString(20).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
    }

    public override void Down()
    {
        Delete.Table("PaymentLedger").InSchema("PMS");
    }
}
