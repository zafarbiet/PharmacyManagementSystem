using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260325000003)]
public class AddMissingMenuItems : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, [Key], Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(NEWID(), '/settings/manufacturers',        'Manufacturers',        'BankOutlined',     'settings',    6,  GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/promotions',           'Promotions',           'TagsOutlined',     'settings',    7,  GETUTCDATE(), 'system', 1),
(NEWID(), '/quotations/vendor-responses',   'Vendor Responses',     'TeamOutlined',     'quotations',  3,  GETUTCDATE(), 'system', 1)
");
    }

    public override void Down()
    {
        Execute.Sql(@"
DELETE FROM PMS.MenuItems WHERE [Key] IN (
    '/settings/manufacturers',
    '/settings/promotions',
    '/quotations/vendor-responses'
)
");
    }
}
