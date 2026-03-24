using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260324000001)]
public class AlterDebtRecordsFixColumns : Migration
{
    public override void Up()
    {
        Rename.Column("TotalAmount").OnTable("DebtRecords").InSchema("PMS").To("OriginalAmount");
        Rename.Column("DebtDate").OnTable("DebtRecords").InSchema("PMS").To("DueDate");

        Alter.Column("DueDate").OnTable("DebtRecords").InSchema("PMS")
            .AsDateTimeOffset().Nullable();

        Create.Column("InvoiceId").OnTable("DebtRecords").InSchema("PMS")
            .AsGuid().Nullable();

        Create.ForeignKey("FK_DebtRecords_CustomerInvoices_InvoiceId")
            .FromTable("DebtRecords").InSchema("PMS").ForeignColumn("InvoiceId")
            .ToTable("CustomerInvoices").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DebtRecords_CustomerInvoices_InvoiceId").OnTable("DebtRecords").InSchema("PMS");
        Delete.Column("InvoiceId").FromTable("DebtRecords").InSchema("PMS");

        Alter.Column("DueDate").OnTable("DebtRecords").InSchema("PMS")
            .AsDateTimeOffset().NotNullable();

        Rename.Column("DueDate").OnTable("DebtRecords").InSchema("PMS").To("DebtDate");
        Rename.Column("OriginalAmount").OnTable("DebtRecords").InSchema("PMS").To("TotalAmount");
    }
}
