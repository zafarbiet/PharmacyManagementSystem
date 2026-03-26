using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260325000006)]
public class AddFrontDeskMenuItem : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, ""Key"", Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(gen_random_uuid(), '/front-desk', 'Front Desk', 'ShoppingCartOutlined', NULL, 1, NOW(), 'system', true)
");
    }

    public override void Down()
    {
        Execute.Sql(@"DELETE FROM PMS.MenuItems WHERE ""Key"" = '/front-desk'");
    }
}
