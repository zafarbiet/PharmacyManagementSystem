using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260325000004)]
public class AddFrontendMenuItems : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, ""Key"", Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(gen_random_uuid(), '/payments',                      'Payment Ledger',      'WalletOutlined',      NULL,        10, NOW(), 'system', true),
(gen_random_uuid(), '/diary',                         'Daily Diary',         'BookOutlined',        NULL,        11, NOW(), 'system', true),
(gen_random_uuid(), '/audit-logs',                    'Audit Logs',          'AuditOutlined',       NULL,        12, NOW(), 'system', true),
(gen_random_uuid(), '/settings/racks',               'Racks',               'InboxOutlined',       'settings',   8, NOW(), 'system', true),
(gen_random_uuid(), '/settings/drug-pricing',        'Drug Pricing',        'DollarOutlined',      'settings',   9, NOW(), 'system', true),
(gen_random_uuid(), '/inventory/stock-transactions', 'Stock Transactions',  'SwapOutlined',        'inventory',  4, NOW(), 'system', true)
");
    }

    public override void Down()
    {
        Execute.Sql(@"
DELETE FROM PMS.MenuItems WHERE ""Key"" IN (
    '/payments',
    '/diary',
    '/audit-logs',
    '/settings/racks',
    '/settings/drug-pricing',
    '/inventory/stock-transactions'
)
");
    }
}
