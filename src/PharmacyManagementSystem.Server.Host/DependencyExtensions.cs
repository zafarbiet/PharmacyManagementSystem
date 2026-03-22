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
        services.AddSingleton<IGetDrugCategoryAction, GetDrugCategoryAction>();
        services.AddSingleton<ISaveDrugCategoryAction, SaveDrugCategoryAction>();

        // Drug - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugStorageClient, SqlServerDrugStorageClient>();
        services.AddScoped<IDrugRepository, DrugRepository>();
        services.AddSingleton<IGetDrugAction, GetDrugAction>();
        services.AddSingleton<ISaveDrugAction, SaveDrugAction>();

        // Vendor - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IVendorStorageClient, SqlServerVendorStorageClient>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddSingleton<IGetVendorAction, GetVendorAction>();
        services.AddSingleton<ISaveVendorAction, SaveVendorAction>();

        // DrugInventory - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugInventoryStorageClient, SqlServerDrugInventoryStorageClient>();
        services.AddScoped<IDrugInventoryRepository, DrugInventoryRepository>();
        services.AddSingleton<IGetDrugInventoryAction, GetDrugInventoryAction>();
        services.AddSingleton<ISaveDrugInventoryAction, SaveDrugInventoryAction>();

        // DrugPricing - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugPricingStorageClient, SqlServerDrugPricingStorageClient>();
        services.AddScoped<IDrugPricingRepository, DrugPricingRepository>();
        services.AddSingleton<IGetDrugPricingAction, GetDrugPricingAction>();
        services.AddSingleton<ISaveDrugPricingAction, SaveDrugPricingAction>();

        // DrugUsage - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugUsageStorageClient, SqlServerDrugUsageStorageClient>();
        services.AddScoped<IDrugUsageRepository, DrugUsageRepository>();
        services.AddSingleton<IGetDrugUsageAction, GetDrugUsageAction>();
        services.AddSingleton<ISaveDrugUsageAction, SaveDrugUsageAction>();

        // Patient - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPatientStorageClient, SqlServerPatientStorageClient>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddSingleton<IGetPatientAction, GetPatientAction>();
        services.AddSingleton<ISavePatientAction, SavePatientAction>();

        // Prescription - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPrescriptionStorageClient, SqlServerPrescriptionStorageClient>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddSingleton<IGetPrescriptionAction, GetPrescriptionAction>();
        services.AddSingleton<ISavePrescriptionAction, SavePrescriptionAction>();

        // PrescriptionItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPrescriptionItemStorageClient, SqlServerPrescriptionItemStorageClient>();
        services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
        services.AddSingleton<IGetPrescriptionItemAction, GetPrescriptionItemAction>();
        services.AddSingleton<ISavePrescriptionItemAction, SavePrescriptionItemAction>();

        // PurchaseOrder - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPurchaseOrderStorageClient, SqlServerPurchaseOrderStorageClient>();
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddSingleton<IGetPurchaseOrderAction, GetPurchaseOrderAction>();
        services.AddSingleton<ISavePurchaseOrderAction, SavePurchaseOrderAction>();

        // PurchaseOrderItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IPurchaseOrderItemStorageClient, SqlServerPurchaseOrderItemStorageClient>();
        services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
        services.AddSingleton<IGetPurchaseOrderItemAction, GetPurchaseOrderItemAction>();
        services.AddSingleton<ISavePurchaseOrderItemAction, SavePurchaseOrderItemAction>();

        // CustomerInvoice - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerInvoiceStorageClient, SqlServerCustomerInvoiceStorageClient>();
        services.AddScoped<ICustomerInvoiceRepository, CustomerInvoiceRepository>();
        services.AddSingleton<IGetCustomerInvoiceAction, GetCustomerInvoiceAction>();
        services.AddSingleton<ISaveCustomerInvoiceAction, SaveCustomerInvoiceAction>();

        // CustomerInvoiceItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerInvoiceItemStorageClient, SqlServerCustomerInvoiceItemStorageClient>();
        services.AddScoped<ICustomerInvoiceItemRepository, CustomerInvoiceItemRepository>();
        services.AddSingleton<IGetCustomerInvoiceItemAction, GetCustomerInvoiceItemAction>();
        services.AddSingleton<ISaveCustomerInvoiceItemAction, SaveCustomerInvoiceItemAction>();

        // StockTransaction - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IStockTransactionStorageClient, SqlServerStockTransactionStorageClient>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddSingleton<IGetStockTransactionAction, GetStockTransactionAction>();
        services.AddSingleton<ISaveStockTransactionAction, SaveStockTransactionAction>();

        // AppUser - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IAppUserStorageClient, SqlServerAppUserStorageClient>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddSingleton<IGetAppUserAction, GetAppUserAction>();
        services.AddSingleton<ISaveAppUserAction, SaveAppUserAction>();

        // Role - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IRoleStorageClient, SqlServerRoleStorageClient>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddSingleton<IGetRoleAction, GetRoleAction>();
        services.AddSingleton<ISaveRoleAction, SaveRoleAction>();

        // UserRole - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IUserRoleStorageClient, SqlServerUserRoleStorageClient>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddSingleton<IGetUserRoleAction, GetUserRoleAction>();
        services.AddSingleton<ISaveUserRoleAction, SaveUserRoleAction>();

        // StorageZone - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IStorageZoneStorageClient, SqlServerStorageZoneStorageClient>();
        services.AddScoped<IStorageZoneRepository, StorageZoneRepository>();
        services.AddSingleton<IGetStorageZoneAction, GetStorageZoneAction>();
        services.AddSingleton<ISaveStorageZoneAction, SaveStorageZoneAction>();

        // Rack - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IRackStorageClient, SqlServerRackStorageClient>();
        services.AddScoped<IRackRepository, RackRepository>();
        services.AddSingleton<IGetRackAction, GetRackAction>();
        services.AddSingleton<ISaveRackAction, SaveRackAction>();

        // DrugInventoryRackAssignment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDrugInventoryRackAssignmentStorageClient, SqlServerDrugInventoryRackAssignmentStorageClient>();
        services.AddScoped<IDrugInventoryRackAssignmentRepository, DrugInventoryRackAssignmentRepository>();
        services.AddSingleton<IGetDrugInventoryRackAssignmentAction, GetDrugInventoryRackAssignmentAction>();
        services.AddSingleton<ISaveDrugInventoryRackAssignmentAction, SaveDrugInventoryRackAssignmentAction>();

        // ExpiryAlertConfiguration - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IExpiryAlertConfigurationStorageClient, SqlServerExpiryAlertConfigurationStorageClient>();
        services.AddScoped<IExpiryAlertConfigurationRepository, ExpiryAlertConfigurationRepository>();
        services.AddSingleton<IGetExpiryAlertConfigurationAction, GetExpiryAlertConfigurationAction>();
        services.AddSingleton<ISaveExpiryAlertConfigurationAction, SaveExpiryAlertConfigurationAction>();

        // ExpiryRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IExpiryRecordStorageClient, SqlServerExpiryRecordStorageClient>();
        services.AddScoped<IExpiryRecordRepository, ExpiryRecordRepository>();
        services.AddSingleton<IGetExpiryRecordAction, GetExpiryRecordAction>();
        services.AddSingleton<ISaveExpiryRecordAction, SaveExpiryRecordAction>();

        // DisposalRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDisposalRecordStorageClient, SqlServerDisposalRecordStorageClient>();
        services.AddScoped<IDisposalRecordRepository, DisposalRecordRepository>();
        services.AddSingleton<IGetDisposalRecordAction, GetDisposalRecordAction>();
        services.AddSingleton<ISaveDisposalRecordAction, SaveDisposalRecordAction>();

        // VendorExpiryReturnRequest - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IVendorExpiryReturnRequestStorageClient, SqlServerVendorExpiryReturnRequestStorageClient>();
        services.AddScoped<IVendorExpiryReturnRequestRepository, VendorExpiryReturnRequestRepository>();
        services.AddSingleton<IGetVendorExpiryReturnRequestAction, GetVendorExpiryReturnRequestAction>();
        services.AddSingleton<ISaveVendorExpiryReturnRequestAction, SaveVendorExpiryReturnRequestAction>();

        // QuotationRequest - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationRequestStorageClient, SqlServerQuotationRequestStorageClient>();
        services.AddScoped<IQuotationRequestRepository, QuotationRequestRepository>();
        services.AddSingleton<IGetQuotationRequestAction, GetQuotationRequestAction>();
        services.AddSingleton<ISaveQuotationRequestAction, SaveQuotationRequestAction>();

        // QuotationRequestItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationRequestItemStorageClient, SqlServerQuotationRequestItemStorageClient>();
        services.AddScoped<IQuotationRequestItemRepository, QuotationRequestItemRepository>();
        services.AddSingleton<IGetQuotationRequestItemAction, GetQuotationRequestItemAction>();
        services.AddSingleton<ISaveQuotationRequestItemAction, SaveQuotationRequestItemAction>();

        // Quotation - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationStorageClient, SqlServerQuotationStorageClient>();
        services.AddScoped<IQuotationRepository, QuotationRepository>();
        services.AddSingleton<IGetQuotationAction, GetQuotationAction>();
        services.AddSingleton<ISaveQuotationAction, SaveQuotationAction>();

        // QuotationItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IQuotationItemStorageClient, SqlServerQuotationItemStorageClient>();
        services.AddScoped<IQuotationItemRepository, QuotationItemRepository>();
        services.AddSingleton<IGetQuotationItemAction, GetQuotationItemAction>();
        services.AddSingleton<ISaveQuotationItemAction, SaveQuotationItemAction>();

        // CustomerSubscription - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerSubscriptionStorageClient, SqlServerCustomerSubscriptionStorageClient>();
        services.AddScoped<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
        services.AddSingleton<IGetCustomerSubscriptionAction, GetCustomerSubscriptionAction>();
        services.AddSingleton<ISaveCustomerSubscriptionAction, SaveCustomerSubscriptionAction>();

        // CustomerSubscriptionItem - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ICustomerSubscriptionItemStorageClient, SqlServerCustomerSubscriptionItemStorageClient>();
        services.AddScoped<ICustomerSubscriptionItemRepository, CustomerSubscriptionItemRepository>();
        services.AddSingleton<IGetCustomerSubscriptionItemAction, GetCustomerSubscriptionItemAction>();
        services.AddSingleton<ISaveCustomerSubscriptionItemAction, SaveCustomerSubscriptionItemAction>();

        // SubscriptionFulfillment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<ISubscriptionFulfillmentStorageClient, SqlServerSubscriptionFulfillmentStorageClient>();
        services.AddScoped<ISubscriptionFulfillmentRepository, SubscriptionFulfillmentRepository>();
        services.AddSingleton<IGetSubscriptionFulfillmentAction, GetSubscriptionFulfillmentAction>();
        services.AddSingleton<ISaveSubscriptionFulfillmentAction, SaveSubscriptionFulfillmentAction>();

        // DebtRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtRecordStorageClient, SqlServerDebtRecordStorageClient>();
        services.AddScoped<IDebtRecordRepository, DebtRecordRepository>();
        services.AddSingleton<IGetDebtRecordAction, GetDebtRecordAction>();
        services.AddSingleton<ISaveDebtRecordAction, SaveDebtRecordAction>();

        // DebtPayment - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtPaymentStorageClient, SqlServerDebtPaymentStorageClient>();
        services.AddScoped<IDebtPaymentRepository, DebtPaymentRepository>();
        services.AddSingleton<IGetDebtPaymentAction, GetDebtPaymentAction>();
        services.AddSingleton<ISaveDebtPaymentAction, SaveDebtPaymentAction>();

        // DebtReminder - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDebtReminderStorageClient, SqlServerDebtReminderStorageClient>();
        services.AddScoped<IDebtReminderRepository, DebtReminderRepository>();
        services.AddSingleton<IGetDebtReminderAction, GetDebtReminderAction>();
        services.AddSingleton<ISaveDebtReminderAction, SaveDebtReminderAction>();

        // DamageRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDamageRecordStorageClient, SqlServerDamageRecordStorageClient>();
        services.AddScoped<IDamageRecordRepository, DamageRecordRepository>();
        services.AddSingleton<IGetDamageRecordAction, GetDamageRecordAction>();
        services.AddSingleton<ISaveDamageRecordAction, SaveDamageRecordAction>();

        // DamageDisposalRecord - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDamageDisposalRecordStorageClient, SqlServerDamageDisposalRecordStorageClient>();
        services.AddScoped<IDamageDisposalRecordRepository, DamageDisposalRecordRepository>();
        services.AddSingleton<IGetDamageDisposalRecordAction, GetDamageDisposalRecordAction>();
        services.AddSingleton<ISaveDamageDisposalRecordAction, SaveDamageDisposalRecordAction>();

        // DailyDiaryEntry - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<IDailyDiaryEntryStorageClient, SqlServerDailyDiaryEntryStorageClient>();
        services.AddScoped<IDailyDiaryEntryRepository, DailyDiaryEntryRepository>();
        services.AddSingleton<IGetDailyDiaryEntryAction, GetDailyDiaryEntryAction>();
        services.AddSingleton<ISaveDailyDiaryEntryAction, SaveDailyDiaryEntryAction>();

        // Notification - Storage Client + Repository: Scoped; Actions: Singleton
        services.AddScoped<INotificationStorageClient, SqlServerNotificationStorageClient>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddSingleton<IGetNotificationAction, GetNotificationAction>();
        services.AddSingleton<ISaveNotificationAction, SaveNotificationAction>();

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
