using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000017)]
public class AddUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Username").AsString(100).Nullable()
            .WithColumn("FullName").AsString(200).Nullable()
            .WithColumn("Email").AsString(200).Nullable()
            .WithColumn("Phone").AsString(20).Nullable()
            .WithColumn("PasswordHash").AsString(500).Nullable()
            .WithColumn("IsLocked").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("LastLoginAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("UX_Users_Username")
            .OnTable("Users").InSchema("PMS")
            .OnColumn("Username").Ascending()
            .WithOptions().Unique();

        Create.Index("IX_Users_Email")
            .OnTable("Users").InSchema("PMS")
            .OnColumn("Email").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Users_Email").OnTable("Users").InSchema("PMS");
        Delete.Index("UX_Users_Username").OnTable("Users").InSchema("PMS");
        Delete.Table("Users").InSchema("PMS");
    }
}
