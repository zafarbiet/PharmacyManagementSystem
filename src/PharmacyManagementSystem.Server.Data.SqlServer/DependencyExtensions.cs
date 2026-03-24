using System.Data;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Branch;
using PharmacyManagementSystem.Server.CustomerInvoice;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.CustomerSubscription;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.DailyDiaryEntry;
using PharmacyManagementSystem.Server.DamageDisposalRecord;
using PharmacyManagementSystem.Server.DamageRecord;
using PharmacyManagementSystem.Server.DebtPayment;
using PharmacyManagementSystem.Server.DebtRecord;
using PharmacyManagementSystem.Server.DebtReminder;
using PharmacyManagementSystem.Server.DisposalRecord;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.DrugPricing;
using PharmacyManagementSystem.Server.DrugUsage;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.ExpiryRecord;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.Patient;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.Prescription;
using PharmacyManagementSystem.Server.PrescriptionItem;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Quotation;
using PharmacyManagementSystem.Server.QuotationItem;
using PharmacyManagementSystem.Server.QuotationRequest;
using PharmacyManagementSystem.Server.QuotationRequestItem;
using PharmacyManagementSystem.Server.Rack;
using PharmacyManagementSystem.Server.Report;
using PharmacyManagementSystem.Server.Role;
using PharmacyManagementSystem.Server.StockTransaction;
using PharmacyManagementSystem.Server.StorageZone;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.UserRole;
using PharmacyManagementSystem.Server.Vendor;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.AppUser;
using PharmacyManagementSystem.Server.Data.SqlServer.AuditLog;
using PharmacyManagementSystem.Server.Data.SqlServer.Branch;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoice;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.SqlServer.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DamageRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtPayment;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtReminder;
using PharmacyManagementSystem.Server.Data.SqlServer.DisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugCategory;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugInventory;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugPricing;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugUsage;
using PharmacyManagementSystem.Server.Data.SqlServer.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.SqlServer.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Data.SqlServer.Migrations;
using PharmacyManagementSystem.Server.Data.SqlServer.Notification;
using PharmacyManagementSystem.Server.Data.SqlServer.Patient;
using PharmacyManagementSystem.Server.Data.SqlServer.PaymentLedger;
using PharmacyManagementSystem.Server.Data.SqlServer.Prescription;
using PharmacyManagementSystem.Server.Data.SqlServer.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrder;
using PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Quotation;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationItem;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Rack;
using PharmacyManagementSystem.Server.Data.SqlServer.Report;
using PharmacyManagementSystem.Server.Data.SqlServer.Role;
using PharmacyManagementSystem.Server.Data.SqlServer.StockTransaction;
using PharmacyManagementSystem.Server.Data.SqlServer.StorageZone;
using PharmacyManagementSystem.Server.Data.SqlServer.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.SqlServer.UserRole;
using PharmacyManagementSystem.Server.Data.SqlServer.Vendor;
using PharmacyManagementSystem.Server.Data.SqlServer.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.Data.SqlServer;

public static class DependencyExtensions
{
    public static IServiceCollection AddSqlServerDataLayer(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        // Database connection - Scoped
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        // Infrastructure - Scoped
        services.AddScoped<ISqlServerDbClient, SqlServerDbClient>();

        // DrugCategory - Storage Client: Scoped
        services.AddScoped<IDrugCategoryStorageClient, SqlServerDrugCategoryStorageClient>();

        // Drug - Storage Client: Scoped
        services.AddScoped<IDrugStorageClient, SqlServerDrugStorageClient>();

        // Vendor - Storage Client: Scoped
        services.AddScoped<IVendorStorageClient, SqlServerVendorStorageClient>();

        // DrugInventory - Storage Client: Scoped
        services.AddScoped<IDrugInventoryStorageClient, SqlServerDrugInventoryStorageClient>();

        // DrugPricing - Storage Client: Scoped
        services.AddScoped<IDrugPricingStorageClient, SqlServerDrugPricingStorageClient>();

        // DrugUsage - Storage Client: Scoped
        services.AddScoped<IDrugUsageStorageClient, SqlServerDrugUsageStorageClient>();

        // Patient - Storage Client: Scoped
        services.AddScoped<IPatientStorageClient, SqlServerPatientStorageClient>();

        // Prescription - Storage Client: Scoped
        services.AddScoped<IPrescriptionStorageClient, SqlServerPrescriptionStorageClient>();

        // PrescriptionItem - Storage Client: Scoped
        services.AddScoped<IPrescriptionItemStorageClient, SqlServerPrescriptionItemStorageClient>();

        // PurchaseOrder - Storage Client: Scoped
        services.AddScoped<IPurchaseOrderStorageClient, SqlServerPurchaseOrderStorageClient>();

        // PurchaseOrderItem - Storage Client: Scoped
        services.AddScoped<IPurchaseOrderItemStorageClient, SqlServerPurchaseOrderItemStorageClient>();

        // CustomerInvoice - Storage Client: Scoped
        services.AddScoped<ICustomerInvoiceStorageClient, SqlServerCustomerInvoiceStorageClient>();

        // CustomerInvoiceItem - Storage Client: Scoped
        services.AddScoped<ICustomerInvoiceItemStorageClient, SqlServerCustomerInvoiceItemStorageClient>();

        // StockTransaction - Storage Client: Scoped
        services.AddScoped<IStockTransactionStorageClient, SqlServerStockTransactionStorageClient>();

        // AppUser - Storage Client: Scoped
        services.AddScoped<IAppUserStorageClient, SqlServerAppUserStorageClient>();

        // Role - Storage Client: Scoped
        services.AddScoped<IRoleStorageClient, SqlServerRoleStorageClient>();

        // UserRole - Storage Client: Scoped
        services.AddScoped<IUserRoleStorageClient, SqlServerUserRoleStorageClient>();

        // StorageZone - Storage Client: Scoped
        services.AddScoped<IStorageZoneStorageClient, SqlServerStorageZoneStorageClient>();

        // Rack - Storage Client: Scoped
        services.AddScoped<IRackStorageClient, SqlServerRackStorageClient>();

        // DrugInventoryRackAssignment - Storage Client: Scoped
        services.AddScoped<IDrugInventoryRackAssignmentStorageClient, SqlServerDrugInventoryRackAssignmentStorageClient>();

        // ExpiryAlertConfiguration - Storage Client: Scoped
        services.AddScoped<IExpiryAlertConfigurationStorageClient, SqlServerExpiryAlertConfigurationStorageClient>();

        // ExpiryRecord - Storage Client: Scoped
        services.AddScoped<IExpiryRecordStorageClient, SqlServerExpiryRecordStorageClient>();

        // DisposalRecord - Storage Client: Scoped
        services.AddScoped<IDisposalRecordStorageClient, SqlServerDisposalRecordStorageClient>();

        // VendorExpiryReturnRequest - Storage Client: Scoped
        services.AddScoped<IVendorExpiryReturnRequestStorageClient, SqlServerVendorExpiryReturnRequestStorageClient>();

        // QuotationRequest - Storage Client: Scoped
        services.AddScoped<IQuotationRequestStorageClient, SqlServerQuotationRequestStorageClient>();

        // QuotationRequestItem - Storage Client: Scoped
        services.AddScoped<IQuotationRequestItemStorageClient, SqlServerQuotationRequestItemStorageClient>();

        // Quotation - Storage Client: Scoped
        services.AddScoped<IQuotationStorageClient, SqlServerQuotationStorageClient>();

        // QuotationItem - Storage Client: Scoped
        services.AddScoped<IQuotationItemStorageClient, SqlServerQuotationItemStorageClient>();

        // CustomerSubscription - Storage Client: Scoped
        services.AddScoped<ICustomerSubscriptionStorageClient, SqlServerCustomerSubscriptionStorageClient>();

        // CustomerSubscriptionItem - Storage Client: Scoped
        services.AddScoped<ICustomerSubscriptionItemStorageClient, SqlServerCustomerSubscriptionItemStorageClient>();

        // SubscriptionFulfillment - Storage Client: Scoped
        services.AddScoped<ISubscriptionFulfillmentStorageClient, SqlServerSubscriptionFulfillmentStorageClient>();

        // DebtRecord - Storage Client: Scoped
        services.AddScoped<IDebtRecordStorageClient, SqlServerDebtRecordStorageClient>();

        // DebtPayment - Storage Client: Scoped
        services.AddScoped<IDebtPaymentStorageClient, SqlServerDebtPaymentStorageClient>();

        // DebtReminder - Storage Client: Scoped
        services.AddScoped<IDebtReminderStorageClient, SqlServerDebtReminderStorageClient>();

        // DamageRecord - Storage Client: Scoped
        services.AddScoped<IDamageRecordStorageClient, SqlServerDamageRecordStorageClient>();

        // DamageDisposalRecord - Storage Client: Scoped
        services.AddScoped<IDamageDisposalRecordStorageClient, SqlServerDamageDisposalRecordStorageClient>();

        // DailyDiaryEntry - Storage Client: Scoped
        services.AddScoped<IDailyDiaryEntryStorageClient, SqlServerDailyDiaryEntryStorageClient>();

        // Notification - Storage Client: Scoped
        services.AddScoped<INotificationStorageClient, SqlServerNotificationStorageClient>();

        // Branch - Storage Client: Scoped
        services.AddScoped<IBranchStorageClient, SqlServerBranchStorageClient>();

        // PaymentLedger - Storage Client: Scoped
        services.AddScoped<IPaymentLedgerStorageClient, SqlServerPaymentLedgerStorageClient>();

        // AuditLog - Storage Client: Scoped
        services.AddScoped<IAuditLogStorageClient, SqlServerAuditLogStorageClient>();

        // Report - Scoped
        services.AddScoped<IReportService, SqlServerReportService>();

        // FluentMigrator
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreatePmsSchema).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }
}
