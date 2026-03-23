using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000053)]
public class AlterCustomerInvoicesAddPrescriptionId : Migration
{
    public override void Up()
    {
        Alter.Table("CustomerInvoices").InSchema("PMS")
            .AddColumn("PrescriptionId").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Column("PrescriptionId").FromTable("CustomerInvoices").InSchema("PMS");
    }
}
