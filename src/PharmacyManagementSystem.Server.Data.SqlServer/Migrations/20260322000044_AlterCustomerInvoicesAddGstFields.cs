using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000044)]
public class AlterCustomerInvoicesAddGstFields : Migration
{
    public override void Up()
    {
        Alter.Table("CustomerInvoices").InSchema("PMS")
            .AddColumn("InvoiceNumber").AsString(50).Nullable()
            .AddColumn("TotalCgst").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("TotalSgst").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("TotalIgst").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("BranchId").AsGuid().Nullable()
            .AddColumn("BilledBy").AsString(100).Nullable()
            .AddColumn("PharmacyGstin").AsString(20).Nullable()
            .AddColumn("PatientGstin").AsString(20).Nullable();
    }

    public override void Down()
    {
        Delete.Column("InvoiceNumber").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("TotalCgst").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("TotalSgst").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("TotalIgst").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("BranchId").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("BilledBy").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("PharmacyGstin").FromTable("CustomerInvoices").InSchema("PMS");
        Delete.Column("PatientGstin").FromTable("CustomerInvoices").InSchema("PMS");
    }
}
