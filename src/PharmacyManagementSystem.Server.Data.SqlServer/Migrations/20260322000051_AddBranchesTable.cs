using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000051)]
public class AddBranchesTable : Migration
{
    public override void Up()
    {
        Create.Table("Branches").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Address").AsString(500).Nullable()
            .WithColumn("Gstin").AsString(20).Nullable()
            .WithColumn("PharmacyLicenseNumber").AsString(100).Nullable()
            .WithColumn("Phone").AsString(20).Nullable()
            .WithColumn("Email").AsString(200).Nullable()
            .WithColumn("ManagerUserId").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
    }

    public override void Down()
    {
        Delete.Table("Branches").InSchema("PMS");
    }
}
