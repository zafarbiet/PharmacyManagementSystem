using System.Data;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using PharmacyManagementSystem.Server.Data.SqlServer.DrugCategory;
using PharmacyManagementSystem.Server.Data.SqlServer.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Data.SqlServer.Migrations;
using PharmacyManagementSystem.Server.Data.SqlServer.Vendor;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Drug;
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
