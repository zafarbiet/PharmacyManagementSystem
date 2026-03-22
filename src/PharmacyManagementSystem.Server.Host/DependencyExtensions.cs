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
