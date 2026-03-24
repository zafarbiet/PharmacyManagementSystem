using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000015)]
public class AddCustomerInvoiceItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("CustomerInvoiceItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("InvoiceId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("BatchNumber").AsString(100).Nullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("UnitPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("DiscountPercent").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("GstRate").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_CustomerInvoiceItems_CustomerInvoices_InvoiceId")
            .FromTable("CustomerInvoiceItems").InSchema("PMS").ForeignColumn("InvoiceId")
            .ToTable("CustomerInvoices").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_CustomerInvoiceItems_Drugs_DrugId")
            .FromTable("CustomerInvoiceItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_CustomerInvoiceItems_Drugs_DrugId").OnTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.ForeignKey("FK_CustomerInvoiceItems_CustomerInvoices_InvoiceId").OnTable("CustomerInvoiceItems").InSchema("PMS");
        Delete.Table("CustomerInvoiceItems").InSchema("PMS");
    }
}
