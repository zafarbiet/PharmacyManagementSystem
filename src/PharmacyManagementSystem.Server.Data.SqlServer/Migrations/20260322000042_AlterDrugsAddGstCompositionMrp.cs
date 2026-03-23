using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000042)]
public class AlterDrugsAddGstCompositionMrp : Migration
{
    public override void Up()
    {
        Alter.Table("Drugs").InSchema("PMS")
            .AddColumn("HsnCode").AsString(20).Nullable()
            .AddColumn("GstSlab").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("Composition").AsString(500).Nullable()
            .AddColumn("Mrp").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("HsnCode").FromTable("Drugs").InSchema("PMS");
        Delete.Column("GstSlab").FromTable("Drugs").InSchema("PMS");
        Delete.Column("Composition").FromTable("Drugs").InSchema("PMS");
        Delete.Column("Mrp").FromTable("Drugs").InSchema("PMS");
    }
}
