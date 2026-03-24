using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000004)]
public class AddVendorsTable : Migration
{
    public override void Up()
    {
        Create.Table("Vendors")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).Nullable()
            .WithColumn("ContactPerson").AsString(200).Nullable()
            .WithColumn("Phone").AsString(20).Nullable()
            .WithColumn("Email").AsString(200).Nullable()
            .WithColumn("Address").AsString(500).Nullable()
            .WithColumn("GstNumber").AsString(50).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Vendors_Name")
            .OnTable("Vendors").InSchema("PMS")
            .OnColumn("Name").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Vendors_Name").OnTable("Vendors").InSchema("PMS");
        Delete.Table("Vendors").InSchema("PMS");
    }
}
