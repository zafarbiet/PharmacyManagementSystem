using FluentMigrator.Runner;
using PharmacyManagementSystem.Server.Data.SqlServer;
using PharmacyManagementSystem.Server.Data.PostgreSql;
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
using PharmacyManagementSystem.Server.Auth;

namespace PharmacyManagementSystem.Server.Host;

public static class DependencyExtensions
{
    public static IServiceCollection AddPharmacyManagementServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        var databaseProvider = configuration["DatabaseProvider"] ?? "SqlServer";

        if (databaseProvider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase))
        {
            services.AddPostgreSqlDataLayer(connectionString);
        }
        else
        {
            services.AddSqlServerDataLayer(connectionString);
        }

        // DrugCategory - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugCategoryRepository, DrugCategoryRepository>();
        services.AddScoped<IGetDrugCategoryAction, GetDrugCategoryAction>();
        services.AddScoped<ISaveDrugCategoryAction, SaveDrugCategoryAction>();

        // Drug - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugRepository, DrugRepository>();
        services.AddScoped<IGetDrugAction, GetDrugAction>();
        services.AddScoped<ISaveDrugAction, SaveDrugAction>();

        // Vendor - Repository: Scoped; Actions: Scoped
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IGetVendorAction, GetVendorAction>();
        services.AddScoped<ISaveVendorAction, SaveVendorAction>();

        // DrugInventory - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugInventoryRepository, DrugInventoryRepository>();
        services.AddScoped<IGetDrugInventoryAction, GetDrugInventoryAction>();
        services.AddScoped<ISaveDrugInventoryAction, SaveDrugInventoryAction>();

        // DrugPricing - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugPricingRepository, DrugPricingRepository>();
        services.AddScoped<IGetDrugPricingAction, GetDrugPricingAction>();
        services.AddScoped<ISaveDrugPricingAction, SaveDrugPricingAction>();

        // DrugUsage - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugUsageRepository, DrugUsageRepository>();
        services.AddScoped<IGetDrugUsageAction, GetDrugUsageAction>();
        services.AddScoped<ISaveDrugUsageAction, SaveDrugUsageAction>();

        // Patient - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IGetPatientAction, GetPatientAction>();
        services.AddScoped<ISavePatientAction, SavePatientAction>();

        // Prescription - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IGetPrescriptionAction, GetPrescriptionAction>();
        services.AddScoped<ISavePrescriptionAction, SavePrescriptionAction>();

        // PrescriptionItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
        services.AddScoped<IGetPrescriptionItemAction, GetPrescriptionItemAction>();
        services.AddScoped<ISavePrescriptionItemAction, SavePrescriptionItemAction>();

        // PurchaseOrder - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IGetPurchaseOrderAction, GetPurchaseOrderAction>();
        services.AddScoped<ISavePurchaseOrderAction, SavePurchaseOrderAction>();

        // PurchaseOrderItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
        services.AddScoped<IGetPurchaseOrderItemAction, GetPurchaseOrderItemAction>();
        services.AddScoped<ISavePurchaseOrderItemAction, SavePurchaseOrderItemAction>();

        // GstCalculationService - Singleton (pure math, no DB)
        services.AddSingleton<IGstCalculationService, GstCalculationService>();

        // CustomerInvoice - Repository: Scoped; Actions: Scoped
        services.AddScoped<ICustomerInvoiceRepository, CustomerInvoiceRepository>();
        services.AddScoped<IGetCustomerInvoiceAction, GetCustomerInvoiceAction>();
        services.AddScoped<ISaveCustomerInvoiceAction, SaveCustomerInvoiceAction>();

        // CustomerInvoiceItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<ICustomerInvoiceItemRepository, CustomerInvoiceItemRepository>();
        services.AddScoped<IGetCustomerInvoiceItemAction, GetCustomerInvoiceItemAction>();
        services.AddScoped<ISaveCustomerInvoiceItemAction, SaveCustomerInvoiceItemAction>();

        // StockTransaction - Repository: Scoped; Actions: Scoped
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddScoped<IGetStockTransactionAction, GetStockTransactionAction>();
        services.AddScoped<ISaveStockTransactionAction, SaveStockTransactionAction>();

        // AppUser - Repository: Scoped; Actions: Scoped
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IGetAppUserAction, GetAppUserAction>();
        services.AddScoped<ISaveAppUserAction, SaveAppUserAction>();
        services.AddScoped<ILoginAction, LoginAction>();

        // Role - Repository: Scoped; Actions: Scoped
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IGetRoleAction, GetRoleAction>();
        services.AddScoped<ISaveRoleAction, SaveRoleAction>();

        // UserRole - Repository: Scoped; Actions: Scoped
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IGetUserRoleAction, GetUserRoleAction>();
        services.AddScoped<ISaveUserRoleAction, SaveUserRoleAction>();

        // StorageZone - Repository: Scoped; Actions: Scoped
        services.AddScoped<IStorageZoneRepository, StorageZoneRepository>();
        services.AddScoped<IGetStorageZoneAction, GetStorageZoneAction>();
        services.AddScoped<ISaveStorageZoneAction, SaveStorageZoneAction>();

        // Rack - Repository: Scoped; Actions: Scoped
        services.AddScoped<IRackRepository, RackRepository>();
        services.AddScoped<IGetRackAction, GetRackAction>();
        services.AddScoped<ISaveRackAction, SaveRackAction>();

        // DrugInventoryRackAssignment - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDrugInventoryRackAssignmentRepository, DrugInventoryRackAssignmentRepository>();
        services.AddScoped<IGetDrugInventoryRackAssignmentAction, GetDrugInventoryRackAssignmentAction>();
        services.AddScoped<ISaveDrugInventoryRackAssignmentAction, SaveDrugInventoryRackAssignmentAction>();

        // ExpiryAlertConfiguration - Repository: Scoped; Actions: Scoped
        services.AddScoped<IExpiryAlertConfigurationRepository, ExpiryAlertConfigurationRepository>();
        services.AddScoped<IGetExpiryAlertConfigurationAction, GetExpiryAlertConfigurationAction>();
        services.AddScoped<ISaveExpiryAlertConfigurationAction, SaveExpiryAlertConfigurationAction>();

        // ExpiryRecord - Repository: Scoped; Actions: Scoped
        services.AddScoped<IExpiryRecordRepository, ExpiryRecordRepository>();
        services.AddScoped<IGetExpiryRecordAction, GetExpiryRecordAction>();
        services.AddScoped<ISaveExpiryRecordAction, SaveExpiryRecordAction>();

        // DisposalRecord - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDisposalRecordRepository, DisposalRecordRepository>();
        services.AddScoped<IGetDisposalRecordAction, GetDisposalRecordAction>();
        services.AddScoped<ISaveDisposalRecordAction, SaveDisposalRecordAction>();

        // VendorExpiryReturnRequest - Repository: Scoped; Actions: Scoped
        services.AddScoped<IVendorExpiryReturnRequestRepository, VendorExpiryReturnRequestRepository>();
        services.AddScoped<IGetVendorExpiryReturnRequestAction, GetVendorExpiryReturnRequestAction>();
        services.AddScoped<ISaveVendorExpiryReturnRequestAction, SaveVendorExpiryReturnRequestAction>();

        // QuotationRequest - Repository: Scoped; Actions: Scoped
        services.AddScoped<IQuotationRequestRepository, QuotationRequestRepository>();
        services.AddScoped<IGetQuotationRequestAction, GetQuotationRequestAction>();
        services.AddScoped<ISaveQuotationRequestAction, SaveQuotationRequestAction>();

        // QuotationRequestItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<IQuotationRequestItemRepository, QuotationRequestItemRepository>();
        services.AddScoped<IGetQuotationRequestItemAction, GetQuotationRequestItemAction>();
        services.AddScoped<ISaveQuotationRequestItemAction, SaveQuotationRequestItemAction>();

        // Quotation - Repository: Scoped; Actions: Scoped
        services.AddScoped<IQuotationRepository, QuotationRepository>();
        services.AddScoped<IGetQuotationAction, GetQuotationAction>();
        services.AddScoped<ISaveQuotationAction, SaveQuotationAction>();

        // QuotationItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<IQuotationItemRepository, QuotationItemRepository>();
        services.AddScoped<IGetQuotationItemAction, GetQuotationItemAction>();
        services.AddScoped<ISaveQuotationItemAction, SaveQuotationItemAction>();

        // CustomerSubscription - Repository: Scoped; Actions: Scoped
        services.AddScoped<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
        services.AddScoped<IGetCustomerSubscriptionAction, GetCustomerSubscriptionAction>();
        services.AddScoped<ISaveCustomerSubscriptionAction, SaveCustomerSubscriptionAction>();

        // CustomerSubscriptionItem - Repository: Scoped; Actions: Scoped
        services.AddScoped<ICustomerSubscriptionItemRepository, CustomerSubscriptionItemRepository>();
        services.AddScoped<IGetCustomerSubscriptionItemAction, GetCustomerSubscriptionItemAction>();
        services.AddScoped<ISaveCustomerSubscriptionItemAction, SaveCustomerSubscriptionItemAction>();

        // SubscriptionFulfillment - Repository: Scoped; Actions: Scoped
        services.AddScoped<ISubscriptionFulfillmentRepository, SubscriptionFulfillmentRepository>();
        services.AddScoped<IGetSubscriptionFulfillmentAction, GetSubscriptionFulfillmentAction>();
        services.AddScoped<ISaveSubscriptionFulfillmentAction, SaveSubscriptionFulfillmentAction>();

        // DebtRecord - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDebtRecordRepository, DebtRecordRepository>();
        services.AddScoped<IGetDebtRecordAction, GetDebtRecordAction>();
        services.AddScoped<ISaveDebtRecordAction, SaveDebtRecordAction>();

        // DebtPayment - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDebtPaymentRepository, DebtPaymentRepository>();
        services.AddScoped<IGetDebtPaymentAction, GetDebtPaymentAction>();
        services.AddScoped<ISaveDebtPaymentAction, SaveDebtPaymentAction>();

        // DebtReminder - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDebtReminderRepository, DebtReminderRepository>();
        services.AddScoped<IGetDebtReminderAction, GetDebtReminderAction>();
        services.AddScoped<ISaveDebtReminderAction, SaveDebtReminderAction>();

        // DamageRecord - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDamageRecordRepository, DamageRecordRepository>();
        services.AddScoped<IGetDamageRecordAction, GetDamageRecordAction>();
        services.AddScoped<ISaveDamageRecordAction, SaveDamageRecordAction>();

        // DamageDisposalRecord - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDamageDisposalRecordRepository, DamageDisposalRecordRepository>();
        services.AddScoped<IGetDamageDisposalRecordAction, GetDamageDisposalRecordAction>();
        services.AddScoped<ISaveDamageDisposalRecordAction, SaveDamageDisposalRecordAction>();

        // DailyDiaryEntry - Repository: Scoped; Actions: Scoped
        services.AddScoped<IDailyDiaryEntryRepository, DailyDiaryEntryRepository>();
        services.AddScoped<IGetDailyDiaryEntryAction, GetDailyDiaryEntryAction>();
        services.AddScoped<ISaveDailyDiaryEntryAction, SaveDailyDiaryEntryAction>();

        // Notification - Repository: Scoped; Actions: Scoped
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IGetNotificationAction, GetNotificationAction>();
        services.AddScoped<ISaveNotificationAction, SaveNotificationAction>();

        // Branch - Repository: Scoped; Actions: Scoped
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IGetBranchAction, GetBranchAction>();
        services.AddScoped<ISaveBranchAction, SaveBranchAction>();

        // PaymentLedger - Repository: Scoped; Actions: Scoped
        services.AddScoped<IPaymentLedgerRepository, PaymentLedgerRepository>();
        services.AddScoped<IGetPaymentLedgerAction, GetPaymentLedgerAction>();
        services.AddScoped<ISavePaymentLedgerAction, SavePaymentLedgerAction>();

        // AuditLog - Repository: Scoped; Actions: Scoped
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IGetAuditLogAction, GetAuditLogAction>();
        services.AddScoped<ISaveAuditLogAction, SaveAuditLogAction>();

        return services;
    }

    public static void RunMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
