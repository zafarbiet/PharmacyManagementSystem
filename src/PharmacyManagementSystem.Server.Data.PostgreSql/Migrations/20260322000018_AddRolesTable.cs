using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000018)]
public class AddRolesTable : Migration
{
    public override void Up()
    {
        Create.Table("Roles")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(100).Nullable()
            .WithColumn("Description").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("UX_Roles_Name")
            .OnTable("Roles").InSchema("PMS")
            .OnColumn("Name").Ascending()
            .WithOptions().Unique();
    }

    public override void Down()
    {
        Delete.Index("UX_Roles_Name").OnTable("Roles").InSchema("PMS");
        Delete.Table("Roles").InSchema("PMS");
    }
}
