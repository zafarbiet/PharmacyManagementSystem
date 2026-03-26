using System.Data;
using FluentMigrator.Runner;
using Npgsql;
using PharmacyManagementSystem.Server.Data.PostgreSql.AppUser;
using PharmacyManagementSystem.Server.Data.PostgreSql.AuditLog;
using PharmacyManagementSystem.Server.Data.PostgreSql.Branch;
using PharmacyManagementSystem.Server.Data.PostgreSql.Manufacturer;
using PharmacyManagementSystem.Server.Data.PostgreSql.Promotion;
using PharmacyManagementSystem.Server.Data.PostgreSql.QuotationVendorResponse;
using PharmacyManagementSystem.Server.Data.PostgreSql.MenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.RoleMenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.CustomerInvoice;
using PharmacyManagementSystem.Server.Data.PostgreSql.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.PostgreSql.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.PostgreSql.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.DamageRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.DebtPayment;
using PharmacyManagementSystem.Server.Data.PostgreSql.DebtRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.DebtReminder;
using PharmacyManagementSystem.Server.Data.PostgreSql.DisposalRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Drug;
using PharmacyManagementSystem.Server.Data.PostgreSql.DrugCategory;
using PharmacyManagementSystem.Server.Data.PostgreSql.DrugInventory;
using PharmacyManagementSystem.Server.Data.PostgreSql.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.PostgreSql.DrugPricing;
using PharmacyManagementSystem.Server.Data.PostgreSql.DrugUsage;
using PharmacyManagementSystem.Server.Data.PostgreSql.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.PostgreSql.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;
using PharmacyManagementSystem.Server.Data.PostgreSql.Notification;
using PharmacyManagementSystem.Server.Data.PostgreSql.Patient;
using PharmacyManagementSystem.Server.Data.PostgreSql.PaymentLedger;
using PharmacyManagementSystem.Server.Data.PostgreSql.Prescription;
using PharmacyManagementSystem.Server.Data.PostgreSql.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.PurchaseOrder;
using PharmacyManagementSystem.Server.Data.PostgreSql.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Quotation;
using PharmacyManagementSystem.Server.Data.PostgreSql.QuotationItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequest;
using PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Rack;
using PharmacyManagementSystem.Server.Data.PostgreSql.Report;
using PharmacyManagementSystem.Server.Data.PostgreSql.Role;
using PharmacyManagementSystem.Server.Data.PostgreSql.StockTransaction;
using PharmacyManagementSystem.Server.Data.PostgreSql.StorageZone;
using PharmacyManagementSystem.Server.Data.PostgreSql.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.PostgreSql.UserRole;
using PharmacyManagementSystem.Server.Data.PostgreSql.Vendor;
using PharmacyManagementSystem.Server.Data.PostgreSql.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Branch;
using PharmacyManagementSystem.Server.Manufacturer;
using PharmacyManagementSystem.Server.Promotion;
using PharmacyManagementSystem.Server.QuotationVendorResponse;
using PharmacyManagementSystem.Server.MenuItem;
using PharmacyManagementSystem.Server.RoleMenuItem;
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
using Microsoft.Extensions.DependencyInjection;

namespace PharmacyManagementSystem.Server.Data.PostgreSql;

public static class DependencyExtensions
{
    public static IServiceCollection AddPostgreSqlDataLayer(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        // Database connection - Scoped
        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));

        // Infrastructure - Scoped
        services.AddScoped<INpgsqlDbClient, NpgsqlDbClient>();

        // DrugCategory - Storage Client: Scoped
        services.AddScoped<IDrugCategoryStorageClient, NpgsqlDrugCategoryStorageClient>();

        // Drug - Storage Client: Scoped
        services.AddScoped<IDrugStorageClient, NpgsqlDrugStorageClient>();

        // Vendor - Storage Client: Scoped
        services.AddScoped<IVendorStorageClient, NpgsqlVendorStorageClient>();

        // DrugInventory - Storage Client: Scoped
        services.AddScoped<IDrugInventoryStorageClient, NpgsqlDrugInventoryStorageClient>();

        // DrugPricing - Storage Client: Scoped
        services.AddScoped<IDrugPricingStorageClient, NpgsqlDrugPricingStorageClient>();

        // DrugUsage - Storage Client: Scoped
        services.AddScoped<IDrugUsageStorageClient, NpgsqlDrugUsageStorageClient>();

        // Patient - Storage Client: Scoped
        services.AddScoped<IPatientStorageClient, NpgsqlPatientStorageClient>();

        // Prescription - Storage Client: Scoped
        services.AddScoped<IPrescriptionStorageClient, NpgsqlPrescriptionStorageClient>();

        // PrescriptionItem - Storage Client: Scoped
        services.AddScoped<IPrescriptionItemStorageClient, NpgsqlPrescriptionItemStorageClient>();

        // PurchaseOrder - Storage Client: Scoped
        services.AddScoped<IPurchaseOrderStorageClient, NpgsqlPurchaseOrderStorageClient>();

        // PurchaseOrderItem - Storage Client: Scoped
        services.AddScoped<IPurchaseOrderItemStorageClient, NpgsqlPurchaseOrderItemStorageClient>();

        // CustomerInvoice - Storage Client: Scoped
        services.AddScoped<ICustomerInvoiceStorageClient, NpgsqlCustomerInvoiceStorageClient>();

        // CustomerInvoiceItem - Storage Client: Scoped
        services.AddScoped<ICustomerInvoiceItemStorageClient, NpgsqlCustomerInvoiceItemStorageClient>();

        // StockTransaction - Storage Client: Scoped
        services.AddScoped<IStockTransactionStorageClient, NpgsqlStockTransactionStorageClient>();

        // AppUser - Storage Client: Scoped
        services.AddScoped<IAppUserStorageClient, NpgsqlAppUserStorageClient>();

        // Role - Storage Client: Scoped
        services.AddScoped<IRoleStorageClient, NpgsqlRoleStorageClient>();

        // UserRole - Storage Client: Scoped
        services.AddScoped<IUserRoleStorageClient, NpgsqlUserRoleStorageClient>();

        // StorageZone - Storage Client: Scoped
        services.AddScoped<IStorageZoneStorageClient, NpgsqlStorageZoneStorageClient>();

        // Rack - Storage Client: Scoped
        services.AddScoped<IRackStorageClient, NpgsqlRackStorageClient>();

        // DrugInventoryRackAssignment - Storage Client: Scoped
        services.AddScoped<IDrugInventoryRackAssignmentStorageClient, NpgsqlDrugInventoryRackAssignmentStorageClient>();

        // ExpiryAlertConfiguration - Storage Client: Scoped
        services.AddScoped<IExpiryAlertConfigurationStorageClient, NpgsqlExpiryAlertConfigurationStorageClient>();

        // ExpiryRecord - Storage Client: Scoped
        services.AddScoped<IExpiryRecordStorageClient, NpgsqlExpiryRecordStorageClient>();

        // DisposalRecord - Storage Client: Scoped
        services.AddScoped<IDisposalRecordStorageClient, NpgsqlDisposalRecordStorageClient>();

        // VendorExpiryReturnRequest - Storage Client: Scoped
        services.AddScoped<IVendorExpiryReturnRequestStorageClient, NpgsqlVendorExpiryReturnRequestStorageClient>();

        // QuotationRequest - Storage Client: Scoped
        services.AddScoped<IQuotationRequestStorageClient, NpgsqlQuotationRequestStorageClient>();

        // QuotationRequestItem - Storage Client: Scoped
        services.AddScoped<IQuotationRequestItemStorageClient, NpgsqlQuotationRequestItemStorageClient>();

        // Quotation - Storage Client: Scoped
        services.AddScoped<IQuotationStorageClient, NpgsqlQuotationStorageClient>();

        // QuotationItem - Storage Client: Scoped
        services.AddScoped<IQuotationItemStorageClient, NpgsqlQuotationItemStorageClient>();

        // CustomerSubscription - Storage Client: Scoped
        services.AddScoped<ICustomerSubscriptionStorageClient, NpgsqlCustomerSubscriptionStorageClient>();

        // CustomerSubscriptionItem - Storage Client: Scoped
        services.AddScoped<ICustomerSubscriptionItemStorageClient, NpgsqlCustomerSubscriptionItemStorageClient>();

        // SubscriptionFulfillment - Storage Client: Scoped
        services.AddScoped<ISubscriptionFulfillmentStorageClient, NpgsqlSubscriptionFulfillmentStorageClient>();

        // DebtRecord - Storage Client: Scoped
        services.AddScoped<IDebtRecordStorageClient, NpgsqlDebtRecordStorageClient>();

        // DebtPayment - Storage Client: Scoped
        services.AddScoped<IDebtPaymentStorageClient, NpgsqlDebtPaymentStorageClient>();

        // DebtReminder - Storage Client: Scoped
        services.AddScoped<IDebtReminderStorageClient, NpgsqlDebtReminderStorageClient>();

        // DamageRecord - Storage Client: Scoped
        services.AddScoped<IDamageRecordStorageClient, NpgsqlDamageRecordStorageClient>();

        // DamageDisposalRecord - Storage Client: Scoped
        services.AddScoped<IDamageDisposalRecordStorageClient, NpgsqlDamageDisposalRecordStorageClient>();

        // DailyDiaryEntry - Storage Client: Scoped
        services.AddScoped<IDailyDiaryEntryStorageClient, NpgsqlDailyDiaryEntryStorageClient>();

        // Notification - Storage Client: Scoped
        services.AddScoped<INotificationStorageClient, NpgsqlNotificationStorageClient>();

        // MenuItem - Storage Client: Scoped
        services.AddScoped<IMenuItemStorageClient, NpgsqlMenuItemStorageClient>();

        // RoleMenuItem - Storage Client: Scoped
        services.AddScoped<IRoleMenuItemStorageClient, NpgsqlRoleMenuItemStorageClient>();

        // Branch - Storage Client: Scoped
        services.AddScoped<IBranchStorageClient, NpgsqlBranchStorageClient>();

        // PaymentLedger - Storage Client: Scoped
        services.AddScoped<IPaymentLedgerStorageClient, NpgsqlPaymentLedgerStorageClient>();

        // AuditLog - Storage Client: Scoped
        services.AddScoped<IAuditLogStorageClient, NpgsqlAuditLogStorageClient>();

        // Manufacturer - Storage Client: Scoped
        services.AddScoped<IManufacturerStorageClient, NpgsqlManufacturerStorageClient>();

        // QuotationVendorResponse - Storage Client: Scoped
        services.AddScoped<IQuotationVendorResponseStorageClient, NpgsqlQuotationVendorResponseStorageClient>();

        // Promotion - Storage Client: Scoped
        services.AddScoped<IPromotionStorageClient, NpgsqlPromotionStorageClient>();

        // Report - Scoped
        services.AddScoped<IReportService, NpgsqlReportService>();

        // FluentMigrator
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(AssemblyMarker).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }
}
