using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260325000004)]
public class AddFrontendMenuItems : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
INSERT INTO PMS.MenuItems (Id, [Key], Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive) VALUES
(NEWID(), '/payments',                      'Payment Ledger',      'WalletOutlined',      NULL,        10, GETUTCDATE(), 'system', 1),
(NEWID(), '/diary',                         'Daily Diary',         'BookOutlined',        NULL,        11, GETUTCDATE(), 'system', 1),
(NEWID(), '/audit-logs',                    'Audit Logs',          'AuditOutlined',       NULL,        12, GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/racks',               'Racks',               'InboxOutlined',       'settings',   8, GETUTCDATE(), 'system', 1),
(NEWID(), '/settings/drug-pricing',        'Drug Pricing',        'DollarOutlined',      'settings',   9, GETUTCDATE(), 'system', 1),
(NEWID(), '/inventory/stock-transactions', 'Stock Transactions',  'SwapOutlined',        'inventory',  4, GETUTCDATE(), 'system', 1)
");
    }

    public override void Down()
    {
        Execute.Sql(@"
DELETE FROM PMS.MenuItems WHERE [Key] IN (
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
