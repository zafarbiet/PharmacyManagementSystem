using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

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
INSERT INTO PMS.""MenuItems"" (""Id"", ""Key"", ""Label"", ""Icon"", ""ParentKey"", ""OrderIndex"", ""UpdatedAt"", ""UpdatedBy"", ""IsActive"") VALUES
(gen_random_uuid(), '/',                       'Dashboard',          'BarChartOutlined',    NULL,          1,  NOW(), 'system', true),
(gen_random_uuid(), '/inventory/drugs',        'Drug Inventory',     'MedicineBoxOutlined', NULL,          2,  NOW(), 'system', true),
(gen_random_uuid(), '/drugs',                  'Drug Master',        'MedicineBoxOutlined', NULL,          3,  NOW(), 'system', true),
(gen_random_uuid(), '/purchase-orders',        'Purchase Orders',    'ShoppingCartOutlined',NULL,          4,  NOW(), 'system', true),
(gen_random_uuid(), '/patients',               'Patients',           'ContactsOutlined',    NULL,          5,  NOW(), 'system', true),
(gen_random_uuid(), '/debt',                   'Debt Management',    'CreditCardOutlined',  NULL,          6,  NOW(), 'system', true),
(gen_random_uuid(), '/invoices',               'Invoices',           'FileTextOutlined',    NULL,          7,  NOW(), 'system', true),
(gen_random_uuid(), '/expired-drugs',          'Expiry Management',  'WarningOutlined',     NULL,          8,  NOW(), 'system', true),
(gen_random_uuid(), '/inventory/damage',       'Damage Records',     'AlertOutlined',       NULL,          9,  NOW(), 'system', true),
(gen_random_uuid(), '/subscriptions',          'Subscriptions',      'ExperimentOutlined',  NULL,          10, NOW(), 'system', true),
(gen_random_uuid(), '/notifications',          'Notifications',      'BellOutlined',        NULL,          11, NOW(), 'system', true),
(gen_random_uuid(), '/vendors',                'Vendors',            'TeamOutlined',        NULL,          12, NOW(), 'system', true),
(gen_random_uuid(), 'quotations',              'Quotations',         'SolutionOutlined',    NULL,          13, NOW(), 'system', true),
(gen_random_uuid(), '/quotations/rfq',         'RFQ',                'SendOutlined',        'quotations',  1,  NOW(), 'system', true),
(gen_random_uuid(), '/quotations',             'Received',           'FileTextOutlined',    'quotations',  2,  NOW(), 'system', true),
(gen_random_uuid(), '/reports',                'Reports',            'BarChartOutlined',    NULL,          14, NOW(), 'system', true),
(gen_random_uuid(), '/users',                  'Users',              'UserOutlined',        NULL,          15, NOW(), 'system', true),
(gen_random_uuid(), 'settings',                'Settings',           'SettingOutlined',     NULL,          16, NOW(), 'system', true),
(gen_random_uuid(), '/settings/branches',      'Branches',           'BankOutlined',        'settings',    1,  NOW(), 'system', true),
(gen_random_uuid(), '/settings/drug-categories','Drug Categories',   'TagsOutlined',        'settings',    2,  NOW(), 'system', true),
(gen_random_uuid(), '/settings/roles',         'Roles',              'UserOutlined',        'settings',    3,  NOW(), 'system', true),
(gen_random_uuid(), '/settings/storage',       'Storage & Racks',    'InboxOutlined',       'settings',    4,  NOW(), 'system', true),
(gen_random_uuid(), '/settings/menu',          'Menu Config',        'MenuOutlined',        'settings',    5,  NOW(), 'system', true)
");
    }

    public override void Down()
    {
        Delete.Table("RoleMenuItems").InSchema("PMS");
        Delete.Table("MenuItems").InSchema("PMS");
    }
}
