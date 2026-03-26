using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260325000003)]
public class AddMissingMenuItems : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, ""Key"", Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(gen_random_uuid(), '/settings/manufacturers',      'Manufacturers',    'BankOutlined',     'settings',    6,  NOW(), 'system', true),
(gen_random_uuid(), '/settings/promotions',         'Promotions',       'TagsOutlined',     'settings',    7,  NOW(), 'system', true),
(gen_random_uuid(), '/quotations/vendor-responses', 'Vendor Responses', 'TeamOutlined',     'quotations',  3,  NOW(), 'system', true)
");
    }

    public override void Down()
    {
        Execute.Sql(@"
DELETE FROM PMS.MenuItems WHERE ""Key"" IN (
    '/settings/manufacturers',
    '/settings/promotions',
    '/quotations/vendor-responses'
)
");
    }
}
