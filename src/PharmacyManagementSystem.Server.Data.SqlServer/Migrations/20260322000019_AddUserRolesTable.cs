using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000019)]
public class AddUserRolesTable : Migration
{
    public override void Up()
    {
        Create.Table("UserRoles")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
            .WithColumn("RoleId").AsGuid().NotNullable()
            .WithColumn("AssignedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_UserRoles_UserId")
            .OnTable("UserRoles").InSchema("PMS")
            .OnColumn("UserId").Ascending();

        Create.Index("IX_UserRoles_RoleId")
            .OnTable("UserRoles").InSchema("PMS")
            .OnColumn("RoleId").Ascending();

        Create.Index("UX_UserRoles_UserId_RoleId")
            .OnTable("UserRoles").InSchema("PMS")
            .OnColumn("UserId").Ascending()
            .OnColumn("RoleId").Ascending()
            .WithOptions().Unique();

        Create.ForeignKey("FK_UserRoles_Users_UserId")
            .FromTable("UserRoles").InSchema("PMS").ForeignColumn("UserId")
            .ToTable("Users").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_UserRoles_Roles_RoleId")
            .FromTable("UserRoles").InSchema("PMS").ForeignColumn("RoleId")
            .ToTable("Roles").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_UserRoles_Roles_RoleId").OnTable("UserRoles").InSchema("PMS");
        Delete.ForeignKey("FK_UserRoles_Users_UserId").OnTable("UserRoles").InSchema("PMS");
        Delete.Index("UX_UserRoles_UserId_RoleId").OnTable("UserRoles").InSchema("PMS");
        Delete.Index("IX_UserRoles_RoleId").OnTable("UserRoles").InSchema("PMS");
        Delete.Index("IX_UserRoles_UserId").OnTable("UserRoles").InSchema("PMS");
        Delete.Table("UserRoles").InSchema("PMS");
    }
}
