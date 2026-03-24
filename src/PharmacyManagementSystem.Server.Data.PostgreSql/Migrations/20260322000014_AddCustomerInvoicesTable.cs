using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000014)]
public class AddCustomerInvoicesTable : Migration
{
    public override void Up()
    {
        Create.Table("CustomerInvoices")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PatientId").AsGuid().Nullable()
            .WithColumn("InvoiceDate").AsDateTimeOffset().NotNullable()
            .WithColumn("SubTotal").AsDecimal(18, 2).NotNullable()
            .WithColumn("DiscountAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("GstAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("NetAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("PaymentMethod").AsString(50).Nullable()
            .WithColumn("Status").AsString(50).NotNullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_CustomerInvoices_PatientId")
            .OnTable("CustomerInvoices").InSchema("PMS")
            .OnColumn("PatientId").Ascending();

        Create.Index("IX_CustomerInvoices_Status")
            .OnTable("CustomerInvoices").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.Index("IX_CustomerInvoices_InvoiceDate")
            .OnTable("CustomerInvoices").InSchema("PMS")
            .OnColumn("InvoiceDate").Ascending();

        Create.ForeignKey("FK_CustomerInvoices_Patients_PatientId")
            .FromTable("CustomerInvoices").InSchema("PMS").ForeignColumn("PatientId")
            .ToTable("Patients").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_CustomerInvoices_Patients_PatientId").OnTable("CustomerInvoices").InSchema("PMS");
        Delete.Index("IX_CustomerInvoices_InvoiceDate").OnTable("CustomerInvoices").InSchema("PMS");
        Delete.Index("IX_CustomerInvoices_Status").OnTable("CustomerInvoices").InSchema("PMS");
        Delete.Index("IX_CustomerInvoices_PatientId").OnTable("CustomerInvoices").InSchema("PMS");
        Delete.Table("CustomerInvoices").InSchema("PMS");
    }
}
