using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260325000001)]
public class AddMenuItemsAndRoleMenuItemsTables : Migration
{
    public override void Up()
    {
        Create.Table("MenuItems").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Key").AsString(200).NotNullable()
            .WithColumn("Label").AsString(200).NotNullable()
            .WithColumn("Icon").AsString(100).Nullable()
            .WithColumn("ParentKey").AsString(200).Nullable()
            .WithColumn("OrderIndex").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(200).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_MenuItems_Key").OnTable("MenuItems").InSchema("PMS")
            .OnColumn("Key").Ascending();

        Create.Table("RoleMenuItems").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("RoleId").AsGuid().NotNullable()
            .WithColumn("MenuItemId").AsGuid().NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(200).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_RoleMenuItems_Roles_RoleId")
            .FromTable("RoleMenuItems").InSchema("PMS").ForeignColumn("RoleId")
            .ToTable("Roles").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_RoleMenuItems_MenuItems_MenuItemId")
            .FromTable("RoleMenuItems").InSchema("PMS").ForeignColumn("MenuItemId")
            .ToTable("MenuItems").InSchema("PMS").PrimaryColumn("Id");

        Create.Index("IX_RoleMenuItems_RoleId").OnTable("RoleMenuItems").InSchema("PMS")
            .OnColumn("RoleId").Ascending();

        // Seed menu items
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, [Key], Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(NEWID(), '/',                      'Dashboard',          'BarChartOutlined',    NULL,          1,  GETUTCDATE(), 'system', 1),
(NEWID(), '/inventory/drugs',       'Drug Inventory',     'MedicineBoxOutlined', NULL,          2,  GETUTCDATE(), 'system', 1),
(NEWID(), '/drugs',                 'Drug Master',        'MedicineBoxOutlined', NULL,          3,  GETUTCDATE(), 'system', 1),
(NEWID(), '/purchase-orders',       'Purchase Orders',    'ShoppingCartOutlined',NULL,          4,  GETUTCDATE(), 'system', 1),
(NEWID(), '/patients',              'Patients',           'ContactsOutlined',    NULL,          5,  GETUTCDATE(), 'system', 1),
(NEWID(), '/debt',                  'Debt Management',    'CreditCardOutlined',  NULL,          6,  GETUTCDATE(), 'system', 1),
(NEWID(), '/invoices',              'Invoices',           'FileTextOutlined',    NULL,          7,  GETUTCDATE(), 'system', 1),
(NEWID(), '/expired-drugs',         'Expiry Management',  'WarningOutlined',     NULL,          8,  GETUTCDATE(), 'system', 1),
(NEWID(), '/inventory/damage',      'Damage Records',     'AlertOutlined',       NULL,          9,  GETUTCDATE(), 'system', 1),
(NEWID(), '/subscriptions',         'Subscriptions',      'ExperimentOutlined',  NULL,          10, GETUTCDATE(), 'system', 1),
(NEWID(), '/notifications',         'Notifications',      'BellOutlined',        NULL,          11, GETUTCDATE(), 'system', 1),
(NEWID(), '/vendors',               'Vendors',            'TeamOutlined',        NULL,          12, GETUTCDATE(), 'system', 1),
(NEWID(), 'quotations',             'Quotations',         'SolutionOutlined',    NULL,          13, GETUTCDATE(), 'system', 1),
(NEWID(), '/quotations/rfq',        'RFQ',                'SendOutlined',        'quotations',  1,  GETUTCDATE(), 'system', 1),
(NEWID(), '/quotations',            'Received',           'FileTextOutlined',    'quotations',  2,  GETUTCDATE(), 'system', 1),
(NEWID(), '/reports',               'Reports',            'BarChartOutlined',    NULL,          14, GETUTCDATE(), 'system', 1),
(NEWID(), '/users',                 'Users',              'UserOutlined',        NULL,          15, GETUTCDATE(), 'system', 1),
(NEWID(), 'settings',               'Settings',           'SettingOutlined',     NULL,          16, GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/branches',     'Branches',           'BankOutlined',        'settings',    1,  GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/drug-categories','Drug Categories',  'TagsOutlined',        'settings',    2,  GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/roles',        'Roles',              'UserOutlined',        'settings',    3,  GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/storage',      'Storage & Racks',    'InboxOutlined',       'settings',    4,  GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/menu',         'Menu Config',        'MenuOutlined',        'settings',    5,  GETUTCDATE(), 'system', 1)
");
    }

    public override void Down()
    {
        Delete.Table("RoleMenuItems").InSchema("PMS");
        Delete.Table("MenuItems").InSchema("PMS");
    }
}
