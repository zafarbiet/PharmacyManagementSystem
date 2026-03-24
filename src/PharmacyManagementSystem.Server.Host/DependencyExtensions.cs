using System.Data;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugCategory;
using PharmacyManagementSystem.Server.Data.SqlServer.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugInventory;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugPricing;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugUsage;
using PharmacyManagementSystem.Server.Data.SqlServer.Patient;
using PharmacyManagementSystem.Server.Data.SqlServer.Prescription;
using PharmacyManagementSystem.Server.Data.SqlServer.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrder;
using PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoice;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Data.SqlServer.StockTransaction;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Data.SqlServer.Migrations;
using PharmacyManagementSystem.Server.Data.SqlServer.Vendor;
using PharmacyManagementSystem.Server.Data.SqlServer.AppUser;
using PharmacyManagementSystem.Server.Data.SqlServer.Role;
using PharmacyManagementSystem.Server.Data.SqlServer.UserRole;
using PharmacyManagementSystem.Server.Data.SqlServer.StorageZone;
using PharmacyManagementSystem.Server.Data.SqlServer.Rack;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.SqlServer.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.SqlServer.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Quotation;
using PharmacyManagementSystem.Server.Data.SqlServer.QuotationItem;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtPayment;
using PharmacyManagementSystem.Server.Data.SqlServer.DebtReminder;
using PharmacyManagementSystem.Server.Data.SqlServer.DamageRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.SqlServer.Notification;
using PharmacyManagementSystem.Server.Data.SqlServer.Branch;
using PharmacyManagementSystem.Server.Data.SqlServer.AuditLog;
using PharmacyManagementSystem.Server.Data.SqlServer.PaymentLedger;
using PharmacyManagementSystem.Server.Data.SqlServer.Report;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.DrugPricing;
using PharmacyManagementSystem.Server.DrugUsage;
using PharmacyManagementSystem.Server.Patient;
using PharmacyManagementSystem.Server.Prescription;
using PharmacyManagementSystem.Server.PrescriptionItem;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.CustomerInvoice;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.StockTransaction;
using PharmacyManagementSystem.Server.Vendor;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.Role;
using PharmacyManagementSystem.Server.UserRole;
using PharmacyManagementSystem.Server.StorageZone;
using PharmacyManagementSystem.Server.Rack;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.ExpiryRecord;
using PharmacyManagementSystem.Server.DisposalRecord;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.QuotationRequest;
using PharmacyManagementSystem.Server.QuotationRequestItem;
using PharmacyManagementSystem.Server.Quotation;
using PharmacyManagementSystem.Server.QuotationItem;
using PharmacyManagementSystem.Server.CustomerSubscription;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.DebtRecord;
using PharmacyManagementSystem.Server.DebtPayment;
using PharmacyManagementSystem.Server.DebtReminder;
using PharmacyManagementSystem.Server.DamageRecord;
using PharmacyManagementSystem.Server.DamageDisposalRecord;
using PharmacyManagementSystem.Server.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.Branch;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.GstCalculation;
using PharmacyManagementSystem.Server.Report;
using PharmacyManagementSystem.Server.Auth;

namespace PharmacyManagementSystem.Server.Host;

public static class DependencyExtensions
{
    public static IServiceCollection AddPharmacyManagementServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        // Database connection - Scoped
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

        // Infrastructure - Scoped
        services.AddScoped<ISqlServerDbClient, SqlServerDbClient>();

        // DrugCategory - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugCategoryStorageClient, SqlServerDrugCategoryStorageClient>();
        services.AddScoped<IDrugCategoryRepository, DrugCategoryRepository>();
        services.AddScoped<IGetDrugCategoryAction, GetDrugCategoryAction>();
        services.AddScoped<ISaveDrugCategoryAction, SaveDrugCategoryAction>();

        // Drug - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugStorageClient, SqlServerDrugStorageClient>();
        services.AddScoped<IDrugRepository, DrugRepository>();
        services.AddScoped<IGetDrugAction, GetDrugAction>();
        services.AddScoped<ISaveDrugAction, SaveDrugAction>();

        // Vendor - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IVendorStorageClient, SqlServerVendorStorageClient>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IGetVendorAction, GetVendorAction>();
        services.AddScoped<ISaveVendorAction, SaveVendorAction>();

        // DrugInventory - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugInventoryStorageClient, SqlServerDrugInventoryStorageClient>();
        services.AddScoped<IDrugInventoryRepository, DrugInventoryRepository>();
        services.AddScoped<IGetDrugInventoryAction, GetDrugInventoryAction>();
        services.AddScoped<ISaveDrugInventoryAction, SaveDrugInventoryAction>();

        // DrugPricing - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugPricingStorageClient, SqlServerDrugPricingStorageClient>();
        services.AddScoped<IDrugPricingRepository, DrugPricingRepository>();
        services.AddScoped<IGetDrugPricingAction, GetDrugPricingAction>();
        services.AddScoped<ISaveDrugPricingAction, SaveDrugPricingAction>();

        // DrugUsage - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugUsageStorageClient, SqlServerDrugUsageStorageClient>();
        services.AddScoped<IDrugUsageRepository, DrugUsageRepository>();
        services.AddScoped<IGetDrugUsageAction, GetDrugUsageAction>();
        services.AddScoped<ISaveDrugUsageAction, SaveDrugUsageAction>();

        // Patient - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPatientStorageClient, SqlServerPatientStorageClient>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IGetPatientAction, GetPatientAction>();
        services.AddScoped<ISavePatientAction, SavePatientAction>();

        // Prescription - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPrescriptionStorageClient, SqlServerPrescriptionStorageClient>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IGetPrescriptionAction, GetPrescriptionAction>();
        services.AddScoped<ISavePrescriptionAction, SavePrescriptionAction>();

        // PrescriptionItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPrescriptionItemStorageClient, SqlServerPrescriptionItemStorageClient>();
        services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
        services.AddScoped<IGetPrescriptionItemAction, GetPrescriptionItemAction>();
        services.AddScoped<ISavePrescriptionItemAction, SavePrescriptionItemAction>();

        // PurchaseOrder - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPurchaseOrderStorageClient, SqlServerPurchaseOrderStorageClient>();
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IGetPurchaseOrderAction, GetPurchaseOrderAction>();
        services.AddScoped<ISavePurchaseOrderAction, SavePurchaseOrderAction>();

        // PurchaseOrderItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPurchaseOrderItemStorageClient, SqlServerPurchaseOrderItemStorageClient>();
        services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
        services.AddScoped<IGetPurchaseOrderItemAction, GetPurchaseOrderItemAction>();
        services.AddScoped<ISavePurchaseOrderItemAction, SavePurchaseOrderItemAction>();

        // GstCalculationService - Singleton (pure math, no DB)
        services.AddSingleton<IGstCalculationService, GstCalculationService>();

        // CustomerInvoice - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerInvoiceStorageClient, SqlServerCustomerInvoiceStorageClient>();
        services.AddScoped<ICustomerInvoiceRepository, CustomerInvoiceRepository>();
        services.AddScoped<IGetCustomerInvoiceAction, GetCustomerInvoiceAction>();
        services.AddScoped<ISaveCustomerInvoiceAction, SaveCustomerInvoiceAction>();

        // CustomerInvoiceItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerInvoiceItemStorageClient, SqlServerCustomerInvoiceItemStorageClient>();
        services.AddScoped<ICustomerInvoiceItemRepository, CustomerInvoiceItemRepository>();
        services.AddScoped<IGetCustomerInvoiceItemAction, GetCustomerInvoiceItemAction>();
        services.AddScoped<ISaveCustomerInvoiceItemAction, SaveCustomerInvoiceItemAction>();

        // StockTransaction - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IStockTransactionStorageClient, SqlServerStockTransactionStorageClient>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddScoped<IGetStockTransactionAction, GetStockTransactionAction>();
        services.AddScoped<ISaveStockTransactionAction, SaveStockTransactionAction>();

        // AppUser - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IAppUserStorageClient, SqlServerAppUserStorageClient>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IGetAppUserAction, GetAppUserAction>();
        services.AddScoped<ISaveAppUserAction, SaveAppUserAction>();
        services.AddScoped<ILoginAction, LoginAction>();

        // Role - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IRoleStorageClient, SqlServerRoleStorageClient>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IGetRoleAction, GetRoleAction>();
        services.AddScoped<ISaveRoleAction, SaveRoleAction>();

        // UserRole - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IUserRoleStorageClient, SqlServerUserRoleStorageClient>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IGetUserRoleAction, GetUserRoleAction>();
        services.AddScoped<ISaveUserRoleAction, SaveUserRoleAction>();

        // StorageZone - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IStorageZoneStorageClient, SqlServerStorageZoneStorageClient>();
        services.AddScoped<IStorageZoneRepository, StorageZoneRepository>();
        services.AddScoped<IGetStorageZoneAction, GetStorageZoneAction>();
        services.AddScoped<ISaveStorageZoneAction, SaveStorageZoneAction>();

        // Rack - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IRackStorageClient, SqlServerRackStorageClient>();
        services.AddScoped<IRackRepository, RackRepository>();
        services.AddScoped<IGetRackAction, GetRackAction>();
        services.AddScoped<ISaveRackAction, SaveRackAction>();

        // DrugInventoryRackAssignment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugInventoryRackAssignmentStorageClient, SqlServerDrugInventoryRackAssignmentStorageClient>();
        services.AddScoped<IDrugInventoryRackAssignmentRepository, DrugInventoryRackAssignmentRepository>();
        services.AddScoped<IGetDrugInventoryRackAssignmentAction, GetDrugInventoryRackAssignmentAction>();
        services.AddScoped<ISaveDrugInventoryRackAssignmentAction, SaveDrugInventoryRackAssignmentAction>();

        // ExpiryAlertConfiguration - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IExpiryAlertConfigurationStorageClient, SqlServerExpiryAlertConfigurationStorageClient>();
        services.AddScoped<IExpiryAlertConfigurationRepository, ExpiryAlertConfigurationRepository>();
        services.AddScoped<IGetExpiryAlertConfigurationAction, GetExpiryAlertConfigurationAction>();
        services.AddScoped<ISaveExpiryAlertConfigurationAction, SaveExpiryAlertConfigurationAction>();

        // ExpiryRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IExpiryRecordStorageClient, SqlServerExpiryRecordStorageClient>();
        services.AddScoped<IExpiryRecordRepository, ExpiryRecordRepository>();
        services.AddScoped<IGetExpiryRecordAction, GetExpiryRecordAction>();
        services.AddScoped<ISaveExpiryRecordAction, SaveExpiryRecordAction>();

        // DisposalRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDisposalRecordStorageClient, SqlServerDisposalRecordStorageClient>();
        services.AddScoped<IDisposalRecordRepository, DisposalRecordRepository>();
        services.AddScoped<IGetDisposalRecordAction, GetDisposalRecordAction>();
        services.AddScoped<ISaveDisposalRecordAction, SaveDisposalRecordAction>();

        // VendorExpiryReturnRequest - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IVendorExpiryReturnRequestStorageClient, SqlServerVendorExpiryReturnRequestStorageClient>();
        services.AddScoped<IVendorExpiryReturnRequestRepository, VendorExpiryReturnRequestRepository>();
        services.AddScoped<IGetVendorExpiryReturnRequestAction, GetVendorExpiryReturnRequestAction>();
        services.AddScoped<ISaveVendorExpiryReturnRequestAction, SaveVendorExpiryReturnRequestAction>();

        // QuotationRequest - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationRequestStorageClient, SqlServerQuotationRequestStorageClient>();
        services.AddScoped<IQuotationRequestRepository, QuotationRequestRepository>();
        services.AddScoped<IGetQuotationRequestAction, GetQuotationRequestAction>();
        services.AddScoped<ISaveQuotationRequestAction, SaveQuotationRequestAction>();

        // QuotationRequestItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationRequestItemStorageClient, SqlServerQuotationRequestItemStorageClient>();
        services.AddScoped<IQuotationRequestItemRepository, QuotationRequestItemRepository>();
        services.AddScoped<IGetQuotationRequestItemAction, GetQuotationRequestItemAction>();
        services.AddScoped<ISaveQuotationRequestItemAction, SaveQuotationRequestItemAction>();

        // Quotation - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationStorageClient, SqlServerQuotationStorageClient>();
        services.AddScoped<IQuotationRepository, QuotationRepository>();
        services.AddScoped<IGetQuotationAction, GetQuotationAction>();
        services.AddScoped<ISaveQuotationAction, SaveQuotationAction>();

        // QuotationItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationItemStorageClient, SqlServerQuotationItemStorageClient>();
        services.AddScoped<IQuotationItemRepository, QuotationItemRepository>();
        services.AddScoped<IGetQuotationItemAction, GetQuotationItemAction>();
        services.AddScoped<ISaveQuotationItemAction, SaveQuotationItemAction>();

        // CustomerSubscription - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerSubscriptionStorageClient, SqlServerCustomerSubscriptionStorageClient>();
        services.AddScoped<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
        services.AddScoped<IGetCustomerSubscriptionAction, GetCustomerSubscriptionAction>();
        services.AddScoped<ISaveCustomerSubscriptionAction, SaveCustomerSubscriptionAction>();

        // CustomerSubscriptionItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerSubscriptionItemStorageClient, SqlServerCustomerSubscriptionItemStorageClient>();
        services.AddScoped<ICustomerSubscriptionItemRepository, CustomerSubscriptionItemRepository>();
        services.AddScoped<IGetCustomerSubscriptionItemAction, GetCustomerSubscriptionItemAction>();
        services.AddScoped<ISaveCustomerSubscriptionItemAction, SaveCustomerSubscriptionItemAction>();

        // SubscriptionFulfillment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ISubscriptionFulfillmentStorageClient, SqlServerSubscriptionFulfillmentStorageClient>();
        services.AddScoped<ISubscriptionFulfillmentRepository, SubscriptionFulfillmentRepository>();
        services.AddScoped<IGetSubscriptionFulfillmentAction, GetSubscriptionFulfillmentAction>();
        services.AddScoped<ISaveSubscriptionFulfillmentAction, SaveSubscriptionFulfillmentAction>();

        // DebtRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtRecordStorageClient, SqlServerDebtRecordStorageClient>();
        services.AddScoped<IDebtRecordRepository, DebtRecordRepository>();
        services.AddScoped<IGetDebtRecordAction, GetDebtRecordAction>();
        services.AddScoped<ISaveDebtRecordAction, SaveDebtRecordAction>();

        // DebtPayment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtPaymentStorageClient, SqlServerDebtPaymentStorageClient>();
        services.AddScoped<IDebtPaymentRepository, DebtPaymentRepository>();
        services.AddScoped<IGetDebtPaymentAction, GetDebtPaymentAction>();
        services.AddScoped<ISaveDebtPaymentAction, SaveDebtPaymentAction>();

        // DebtReminder - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtReminderStorageClient, SqlServerDebtReminderStorageClient>();
        services.AddScoped<IDebtReminderRepository, DebtReminderRepository>();
        services.AddScoped<IGetDebtReminderAction, GetDebtReminderAction>();
        services.AddScoped<ISaveDebtReminderAction, SaveDebtReminderAction>();

        // DamageRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDamageRecordStorageClient, SqlServerDamageRecordStorageClient>();
        services.AddScoped<IDamageRecordRepository, DamageRecordRepository>();
        services.AddScoped<IGetDamageRecordAction, GetDamageRecordAction>();
        services.AddScoped<ISaveDamageRecordAction, SaveDamageRecordAction>();

        // DamageDisposalRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDamageDisposalRecordStorageClient, SqlServerDamageDisposalRecordStorageClient>();
        services.AddScoped<IDamageDisposalRecordRepository, DamageDisposalRecordRepository>();
        services.AddScoped<IGetDamageDisposalRecordAction, GetDamageDisposalRecordAction>();
        services.AddScoped<ISaveDamageDisposalRecordAction, SaveDamageDisposalRecordAction>();

        // DailyDiaryEntry - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDailyDiaryEntryStorageClient, SqlServerDailyDiaryEntryStorageClient>();
        services.AddScoped<IDailyDiaryEntryRepository, DailyDiaryEntryRepository>();
        services.AddScoped<IGetDailyDiaryEntryAction, GetDailyDiaryEntryAction>();
        services.AddScoped<ISaveDailyDiaryEntryAction, SaveDailyDiaryEntryAction>();

        // Notification - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<INotificationStorageClient, SqlServerNotificationStorageClient>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IGetNotificationAction, GetNotificationAction>();
        services.AddScoped<ISaveNotificationAction, SaveNotificationAction>();

        // Branch - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IBranchStorageClient, SqlServerBranchStorageClient>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IGetBranchAction, GetBranchAction>();
        services.AddScoped<ISaveBranchAction, SaveBranchAction>();

        // PaymentLedger - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPaymentLedgerStorageClient, SqlServerPaymentLedgerStorageClient>();
        services.AddScoped<IPaymentLedgerRepository, PaymentLedgerRepository>();
        services.AddScoped<IGetPaymentLedgerAction, GetPaymentLedgerAction>();
        services.AddScoped<ISavePaymentLedgerAction, SavePaymentLedgerAction>();

        // AuditLog - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IAuditLogStorageClient, SqlServerAuditLogStorageClient>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IGetAuditLogAction, GetAuditLogAction>();
        services.AddScoped<ISaveAuditLogAction, SaveAuditLogAction>();

        // Report - Scoped (uses ISqlServerDbClient)
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

    public static void RunMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
