using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000045)]
public class AlterCustomerInvoiceItemsAddGstFields : Migration
{
    public override void Up()
    {
        Alter.Table("CustomerInvoiceItems").InSchema("PMS")
            .AddColumn("HsnCode").AsString(20).Nullable()
            .AddColumn("TaxableValue").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("CgstAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("SgstAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("IgstAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("HsnCode").FromTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.Column("TaxableValue").FromTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.Column("CgstAmount").FromTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.Column("SgstAmount").FromTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.Column("IgstAmount").FromTable("CustomerInvoiceItems").InSchema("PMS");
    }
}
