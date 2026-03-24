using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Common.DrugCategory;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Common.Vendor;
using PharmacyManagementSystem.Common.AppUser;
using PharmacyManagementSystem.Common.Role;
using PharmacyManagementSystem.Common.UserRole;
using PharmacyManagementSystem.Common.StorageZone;
using PharmacyManagementSystem.Common.Rack;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Common.ExpiryRecord;
using PharmacyManagementSystem.Common.DisposalRecord;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Common.QuotationRequest;
using PharmacyManagementSystem.Common.QuotationRequestItem;
using PharmacyManagementSystem.Common.Quotation;
using PharmacyManagementSystem.Common.QuotationItem;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;
using PharmacyManagementSystem.Common.DebtRecord;
using PharmacyManagementSystem.Common.DebtPayment;
using PharmacyManagementSystem.Common.DebtReminder;
using PharmacyManagementSystem.Common.DamageRecord;
using PharmacyManagementSystem.Common.DamageDisposalRecord;
using PharmacyManagementSystem.Common.DailyDiaryEntry;
using PharmacyManagementSystem.Common.Notification;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.Vendor;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.Role;
using PharmacyManagementSystem.Server.UserRole;
using PharmacyManagementSystem.Server.StorageZone;
using PharmacyManagementSystem.Server.Rack;
using PharmacyManagementSystem.Server.DrugInventory;
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
using PharmacyManagementSystem.Common.AuditLog;
using PharmacyManagementSystem.Common.Branch;
using PharmacyManagementSystem.Common.PaymentLedger;
using PharmacyManagementSystem.Common.PurchaseOrder;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Branch;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Report;
using PharmacyManagementSystem.Server.Auth;
using PharmacyManagementSystem.Common.Auth;
using PharmacyManagementSystem.Common.CustomerInvoice;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.CustomerInvoice;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.Host;

public static class ControllerExtensions
{
    public static WebApplication AddMinimalApis(this WebApplication app)
    {
        app.AddDrugCategoryApis();
        app.AddDrugApis();
        app.AddVendorApis();
        app.AddAppUserApis();
        app.AddRoleApis();
        app.AddUserRoleApis();
        app.AddStorageZoneApis();
        app.AddRackApis();
        app.AddDrugInventoryApis();
        app.AddDrugInventoryRackAssignmentApis();
        app.AddExpiryAlertConfigurationApis();
        app.AddExpiryRecordApis();
        app.AddDisposalRecordApis();
        app.AddVendorExpiryReturnRequestApis();
        app.AddQuotationRequestApis();
        app.AddQuotationRequestItemApis();
        app.AddQuotationApis();
        app.AddQuotationItemApis();
        app.AddCustomerSubscriptionApis();
        app.AddCustomerSubscriptionItemApis();
        app.AddSubscriptionFulfillmentApis();
        app.AddDebtRecordApis();
        app.AddDebtPaymentApis();
        app.AddDebtReminderApis();
        app.AddDamageRecordApis();
        app.AddDamageDisposalRecordApis();
        app.AddDailyDiaryEntryApis();
        app.AddNotificationApis();
        app.AddBranchApis();
        app.AddPaymentLedgerApis();
        app.AddPurchaseOrderApis();
        app.AddPurchaseOrderItemApis();
        app.AddAuditLogApis();
        app.AddReportApis();
        app.AddCustomerInvoiceApis();
        app.AddCustomerInvoiceItemApis();
        app.AddAuthApis();
        return app;
    }

    private static void AddDrugCategoryApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/drug-categories").WithTags("DrugCategories");

        group.MapGet("/", async (
            [FromServices] IGetDrugCategoryAction action,
            [AsParameters] DrugCategoryFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDrugCategories");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDrugCategoryAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDrugCategoryById");

        group.MapPost("/", async (
            [FromBody] Common.DrugCategory.DrugCategory category,
            [FromServices] ISaveDrugCategoryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(category, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/drug-categories/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDrugCategory");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DrugCategory.DrugCategory category,
            [FromServices] ISaveDrugCategoryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                category.Id = id;
                var result = await action.UpdateAsync(category, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDrugCategory");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDrugCategoryAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDrugCategory");
    }

    private static void AddDrugApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/drugs").WithTags("Drugs");

        group.MapGet("/", async (
            [FromServices] IGetDrugAction action,
            [AsParameters] DrugFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDrugs");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDrugAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDrugById");

        group.MapGet("/alternatives", async (
            string composition,
            [FromServices] IGetDrugAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(
                new DrugFilter { Composition = composition },
                cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetAlternativeDrugs");

        group.MapPost("/", async (
            [FromBody] Common.Drug.Drug drug,
            [FromServices] ISaveDrugAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(drug, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/drugs/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDrug");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Drug.Drug drug,
            [FromServices] ISaveDrugAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                drug.Id = id;
                var result = await action.UpdateAsync(drug, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDrug");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDrugAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDrug");
    }

    private static void AddVendorApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/vendors").WithTags("Vendors");

        group.MapGet("/", async (
            [FromServices] IGetVendorAction action,
            [AsParameters] VendorFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetVendors");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetVendorAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetVendorById");

        group.MapPost("/", async (
            [FromBody] Common.Vendor.Vendor vendor,
            [FromServices] ISaveVendorAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(vendor, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/vendors/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateVendor");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Vendor.Vendor vendor,
            [FromServices] ISaveVendorAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                vendor.Id = id;
                var result = await action.UpdateAsync(vendor, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateVendor");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveVendorAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteVendor");
    }

    private static void AddAppUserApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/app-users").WithTags("AppUsers");

        group.MapGet("/", async (
            [FromServices] IGetAppUserAction action,
            [AsParameters] AppUserFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetAppUsers");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetAppUserAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetAppUserById");

        group.MapPost("/", async (
            [FromBody] Common.AppUser.AppUser appUser,
            [FromServices] ISaveAppUserAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(appUser, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/app-users/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateAppUser");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.AppUser.AppUser appUser,
            [FromServices] ISaveAppUserAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                appUser.Id = id;
                var result = await action.UpdateAsync(appUser, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateAppUser");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveAppUserAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteAppUser");
    }

    private static void AddRoleApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/roles").WithTags("Roles");

        group.MapGet("/", async (
            [FromServices] IGetRoleAction action,
            [AsParameters] RoleFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetRoles");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetRoleAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetRoleById");

        group.MapPost("/", async (
            [FromBody] Common.Role.Role role,
            [FromServices] ISaveRoleAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(role, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/roles/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateRole");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Role.Role role,
            [FromServices] ISaveRoleAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                role.Id = id;
                var result = await action.UpdateAsync(role, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateRole");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveRoleAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteRole");
    }

    private static void AddUserRoleApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/user-roles").WithTags("UserRoles");

        group.MapGet("/", async (
            [FromServices] IGetUserRoleAction action,
            [AsParameters] UserRoleFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetUserRoles");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetUserRoleAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetUserRoleById");

        group.MapPost("/", async (
            [FromBody] Common.UserRole.UserRole userRole,
            [FromServices] ISaveUserRoleAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(userRole, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/user-roles/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateUserRole");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.UserRole.UserRole userRole,
            [FromServices] ISaveUserRoleAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                userRole.Id = id;
                var result = await action.UpdateAsync(userRole, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateUserRole");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveUserRoleAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteUserRole");
    }

    private static void AddStorageZoneApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/storage-zones").WithTags("StorageZones");

        group.MapGet("/", async (
            [FromServices] IGetStorageZoneAction action,
            [AsParameters] StorageZoneFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetStorageZones");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetStorageZoneAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetStorageZoneById");

        group.MapPost("/", async (
            [FromBody] Common.StorageZone.StorageZone storageZone,
            [FromServices] ISaveStorageZoneAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(storageZone, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/storage-zones/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateStorageZone");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.StorageZone.StorageZone storageZone,
            [FromServices] ISaveStorageZoneAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                storageZone.Id = id;
                var result = await action.UpdateAsync(storageZone, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateStorageZone");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveStorageZoneAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteStorageZone");
    }

    private static void AddRackApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/racks").WithTags("Racks");

        group.MapGet("/", async (
            [FromServices] IGetRackAction action,
            [AsParameters] RackFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetRacks");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetRackAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetRackById");

        group.MapPost("/", async (
            [FromBody] Common.Rack.Rack rack,
            [FromServices] ISaveRackAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(rack, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/racks/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateRack");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Rack.Rack rack,
            [FromServices] ISaveRackAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                rack.Id = id;
                var result = await action.UpdateAsync(rack, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateRack");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveRackAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteRack");
    }

    private static void AddDrugInventoryApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/drug-inventories").WithTags("DrugInventories");

        group.MapGet("/", async (
            [FromServices] IGetDrugInventoryAction action,
            [AsParameters] DrugInventoryFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDrugInventories");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDrugInventoryAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDrugInventoryById");

        group.MapPost("/", async (
            [FromBody] Common.DrugInventory.DrugInventory inventory,
            [FromServices] ISaveDrugInventoryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(inventory, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/drug-inventories/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDrugInventory");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DrugInventory.DrugInventory inventory,
            [FromServices] ISaveDrugInventoryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                inventory.Id = id;
                var result = await action.UpdateAsync(inventory, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDrugInventory");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDrugInventoryAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDrugInventory");
    }

    private static void AddDrugInventoryRackAssignmentApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/drug-inventory-rack-assignments").WithTags("DrugInventoryRackAssignments");

        group.MapGet("/", async (
            [FromServices] IGetDrugInventoryRackAssignmentAction action,
            [AsParameters] DrugInventoryRackAssignmentFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDrugInventoryRackAssignments");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDrugInventoryRackAssignmentAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDrugInventoryRackAssignmentById");

        group.MapPost("/", async (
            [FromBody] Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment drugInventoryRackAssignment,
            [FromServices] ISaveDrugInventoryRackAssignmentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/drug-inventory-rack-assignments/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDrugInventoryRackAssignment");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment drugInventoryRackAssignment,
            [FromServices] ISaveDrugInventoryRackAssignmentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                drugInventoryRackAssignment.Id = id;
                var result = await action.UpdateAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDrugInventoryRackAssignment");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDrugInventoryRackAssignmentAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDrugInventoryRackAssignment");
    }

    private static void AddExpiryAlertConfigurationApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/expiry-alert-configurations").WithTags("ExpiryAlertConfigurations");

        group.MapGet("/", async (
            [FromServices] IGetExpiryAlertConfigurationAction action,
            [AsParameters] ExpiryAlertConfigurationFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetExpiryAlertConfigurations");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetExpiryAlertConfigurationAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetExpiryAlertConfigurationById");

        group.MapPost("/", async (
            [FromBody] Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expiryAlertConfiguration,
            [FromServices] ISaveExpiryAlertConfigurationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/expiry-alert-configurations/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateExpiryAlertConfiguration");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expiryAlertConfiguration,
            [FromServices] ISaveExpiryAlertConfigurationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                expiryAlertConfiguration.Id = id;
                var result = await action.UpdateAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateExpiryAlertConfiguration");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveExpiryAlertConfigurationAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteExpiryAlertConfiguration");
    }

    private static void AddExpiryRecordApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/expiry-records").WithTags("ExpiryRecords");

        group.MapGet("/", async (
            [FromServices] IGetExpiryRecordAction action,
            [AsParameters] ExpiryRecordFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetExpiryRecords");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetExpiryRecordAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetExpiryRecordById");

        group.MapPost("/", async (
            [FromBody] Common.ExpiryRecord.ExpiryRecord expiryRecord,
            [FromServices] ISaveExpiryRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(expiryRecord, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/expiry-records/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateExpiryRecord");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.ExpiryRecord.ExpiryRecord expiryRecord,
            [FromServices] ISaveExpiryRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                expiryRecord.Id = id;
                var result = await action.UpdateAsync(expiryRecord, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateExpiryRecord");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveExpiryRecordAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteExpiryRecord");
    }

    private static void AddDisposalRecordApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/disposal-records").WithTags("DisposalRecords");

        group.MapGet("/", async (
            [FromServices] IGetDisposalRecordAction action,
            [AsParameters] DisposalRecordFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDisposalRecords");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDisposalRecordById");

        group.MapPost("/", async (
            [FromBody] Common.DisposalRecord.DisposalRecord disposalRecord,
            [FromServices] ISaveDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(disposalRecord, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/disposal-records/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDisposalRecord");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DisposalRecord.DisposalRecord disposalRecord,
            [FromServices] ISaveDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                disposalRecord.Id = id;
                var result = await action.UpdateAsync(disposalRecord, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDisposalRecord");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDisposalRecord");
    }

    private static void AddVendorExpiryReturnRequestApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/vendor-expiry-return-requests").WithTags("VendorExpiryReturnRequests");

        group.MapGet("/", async (
            [FromServices] IGetVendorExpiryReturnRequestAction action,
            [AsParameters] VendorExpiryReturnRequestFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetVendorExpiryReturnRequests");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetVendorExpiryReturnRequestAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetVendorExpiryReturnRequestById");

        group.MapPost("/", async (
            [FromBody] Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest vendorExpiryReturnRequest,
            [FromServices] ISaveVendorExpiryReturnRequestAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/vendor-expiry-return-requests/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateVendorExpiryReturnRequest");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest vendorExpiryReturnRequest,
            [FromServices] ISaveVendorExpiryReturnRequestAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                vendorExpiryReturnRequest.Id = id;
                var result = await action.UpdateAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateVendorExpiryReturnRequest");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveVendorExpiryReturnRequestAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteVendorExpiryReturnRequest");
    }

    private static void AddQuotationRequestApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/quotation-requests").WithTags("QuotationRequests");

        group.MapGet("/", async (
            [FromServices] IGetQuotationRequestAction action,
            [AsParameters] QuotationRequestFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetQuotationRequests");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetQuotationRequestAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetQuotationRequestById");

        group.MapPost("/", async (
            [FromBody] Common.QuotationRequest.QuotationRequest quotationRequest,
            [FromServices] ISaveQuotationRequestAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(quotationRequest, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/quotation-requests/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateQuotationRequest");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.QuotationRequest.QuotationRequest quotationRequest,
            [FromServices] ISaveQuotationRequestAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                quotationRequest.Id = id;
                var result = await action.UpdateAsync(quotationRequest, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateQuotationRequest");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveQuotationRequestAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteQuotationRequest");

        group.MapPost("/{id}/dispatch", async (
            Guid id,
            [FromBody] IReadOnlyList<Guid> vendorIds,
            [FromServices] ISaveQuotationRequestAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.DispatchToVendorsAsync(id, vendorIds, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("DispatchRfqToVendors");
    }

    private static void AddQuotationRequestItemApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/quotation-request-items").WithTags("QuotationRequestItems");

        group.MapGet("/", async (
            [FromServices] IGetQuotationRequestItemAction action,
            [AsParameters] QuotationRequestItemFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetQuotationRequestItems");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetQuotationRequestItemAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetQuotationRequestItemById");

        group.MapPost("/", async (
            [FromBody] Common.QuotationRequestItem.QuotationRequestItem quotationRequestItem,
            [FromServices] ISaveQuotationRequestItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/quotation-request-items/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateQuotationRequestItem");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.QuotationRequestItem.QuotationRequestItem quotationRequestItem,
            [FromServices] ISaveQuotationRequestItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                quotationRequestItem.Id = id;
                var result = await action.UpdateAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateQuotationRequestItem");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveQuotationRequestItemAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteQuotationRequestItem");
    }

    private static void AddQuotationApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/quotations").WithTags("Quotations");

        group.MapGet("/", async (
            [FromServices] IGetQuotationAction action,
            [AsParameters] QuotationFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetQuotations");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetQuotationAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetQuotationById");

        group.MapPost("/", async (
            [FromBody] Common.Quotation.Quotation quotation,
            [FromServices] ISaveQuotationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(quotation, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/quotations/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateQuotation");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Quotation.Quotation quotation,
            [FromServices] ISaveQuotationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                quotation.Id = id;
                var result = await action.UpdateAsync(quotation, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateQuotation");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveQuotationAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteQuotation");

        group.MapPost("/{id}/accept", async (
            Guid id,
            Guid? branchId,
            [FromServices] ISaveQuotationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AcceptAsync(id, branchId, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Created($"/api/purchase-orders/{result.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("AcceptQuotation");
    }

    private static void AddQuotationItemApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/quotation-items").WithTags("QuotationItems");

        group.MapGet("/", async (
            [FromServices] IGetQuotationItemAction action,
            [AsParameters] QuotationItemFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetQuotationItems");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetQuotationItemAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetQuotationItemById");

        group.MapPost("/", async (
            [FromBody] Common.QuotationItem.QuotationItem quotationItem,
            [FromServices] ISaveQuotationItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(quotationItem, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/quotation-items/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateQuotationItem");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.QuotationItem.QuotationItem quotationItem,
            [FromServices] ISaveQuotationItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                quotationItem.Id = id;
                var result = await action.UpdateAsync(quotationItem, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateQuotationItem");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveQuotationItemAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteQuotationItem");
    }

    private static void AddCustomerSubscriptionApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/customer-subscriptions").WithTags("CustomerSubscriptions");

        group.MapGet("/", async (
            [FromServices] IGetCustomerSubscriptionAction action,
            [AsParameters] CustomerSubscriptionFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetCustomerSubscriptions");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetCustomerSubscriptionById");

        group.MapPost("/", async (
            [FromBody] Common.CustomerSubscription.CustomerSubscription customerSubscription,
            [FromServices] ISaveCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(customerSubscription, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/customer-subscriptions/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateCustomerSubscription");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.CustomerSubscription.CustomerSubscription customerSubscription,
            [FromServices] ISaveCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                customerSubscription.Id = id;
                var result = await action.UpdateAsync(customerSubscription, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateCustomerSubscription");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteCustomerSubscription");

        group.MapPost("/{id}/approve", async (
            Guid id,
            string approvedBy,
            [FromServices] ISaveCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.ApproveAsync(id, approvedBy, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("ApproveCustomerSubscription");

        group.MapPost("/approve-batch", async (
            string approvedBy,
            [FromServices] ISaveCustomerSubscriptionAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.ApproveBatchAsync(approvedBy, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("ApproveBatchCustomerSubscriptions");
    }

    private static void AddCustomerSubscriptionItemApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/customer-subscription-items").WithTags("CustomerSubscriptionItems");

        group.MapGet("/", async (
            [FromServices] IGetCustomerSubscriptionItemAction action,
            [AsParameters] CustomerSubscriptionItemFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetCustomerSubscriptionItems");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetCustomerSubscriptionItemAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetCustomerSubscriptionItemById");

        group.MapPost("/", async (
            [FromBody] Common.CustomerSubscriptionItem.CustomerSubscriptionItem customerSubscriptionItem,
            [FromServices] ISaveCustomerSubscriptionItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/customer-subscription-items/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateCustomerSubscriptionItem");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.CustomerSubscriptionItem.CustomerSubscriptionItem customerSubscriptionItem,
            [FromServices] ISaveCustomerSubscriptionItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                customerSubscriptionItem.Id = id;
                var result = await action.UpdateAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateCustomerSubscriptionItem");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveCustomerSubscriptionItemAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteCustomerSubscriptionItem");
    }

    private static void AddSubscriptionFulfillmentApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/subscription-fulfillments").WithTags("SubscriptionFulfillments");

        group.MapGet("/", async (
            [FromServices] IGetSubscriptionFulfillmentAction action,
            [AsParameters] SubscriptionFulfillmentFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetSubscriptionFulfillments");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetSubscriptionFulfillmentAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetSubscriptionFulfillmentById");

        group.MapPost("/", async (
            [FromBody] Common.SubscriptionFulfillment.SubscriptionFulfillment subscriptionFulfillment,
            [FromServices] ISaveSubscriptionFulfillmentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/subscription-fulfillments/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateSubscriptionFulfillment");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.SubscriptionFulfillment.SubscriptionFulfillment subscriptionFulfillment,
            [FromServices] ISaveSubscriptionFulfillmentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                subscriptionFulfillment.Id = id;
                var result = await action.UpdateAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateSubscriptionFulfillment");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveSubscriptionFulfillmentAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteSubscriptionFulfillment");
    }

    private static void AddDebtRecordApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/debt-records").WithTags("DebtRecords");

        group.MapGet("/", async (
            [FromServices] IGetDebtRecordAction action,
            [AsParameters] DebtRecordFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDebtRecords");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDebtRecordAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDebtRecordById");

        group.MapPost("/", async (
            [FromBody] Common.DebtRecord.DebtRecord debtRecord,
            [FromServices] ISaveDebtRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(debtRecord, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/debt-records/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDebtRecord");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DebtRecord.DebtRecord debtRecord,
            [FromServices] ISaveDebtRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                debtRecord.Id = id;
                var result = await action.UpdateAsync(debtRecord, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDebtRecord");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDebtRecordAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDebtRecord");
    }

    private static void AddDebtPaymentApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/debt-payments").WithTags("DebtPayments");

        group.MapGet("/", async (
            [FromServices] IGetDebtPaymentAction action,
            [AsParameters] DebtPaymentFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDebtPayments");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDebtPaymentAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDebtPaymentById");

        group.MapPost("/", async (
            [FromBody] Common.DebtPayment.DebtPayment debtPayment,
            [FromServices] ISaveDebtPaymentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(debtPayment, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/debt-payments/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDebtPayment");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DebtPayment.DebtPayment debtPayment,
            [FromServices] ISaveDebtPaymentAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                debtPayment.Id = id;
                var result = await action.UpdateAsync(debtPayment, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDebtPayment");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDebtPaymentAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDebtPayment");
    }

    private static void AddDebtReminderApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/debt-reminders").WithTags("DebtReminders");

        group.MapGet("/", async (
            [FromServices] IGetDebtReminderAction action,
            [AsParameters] DebtReminderFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDebtReminders");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDebtReminderAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDebtReminderById");

        group.MapPost("/", async (
            [FromBody] Common.DebtReminder.DebtReminder debtReminder,
            [FromServices] ISaveDebtReminderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(debtReminder, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/debt-reminders/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDebtReminder");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DebtReminder.DebtReminder debtReminder,
            [FromServices] ISaveDebtReminderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                debtReminder.Id = id;
                var result = await action.UpdateAsync(debtReminder, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDebtReminder");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDebtReminderAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDebtReminder");
    }

    private static void AddDamageRecordApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/damage-records").WithTags("DamageRecords");

        group.MapGet("/", async (
            [FromServices] IGetDamageRecordAction action,
            [AsParameters] DamageRecordFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDamageRecords");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDamageRecordAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDamageRecordById");

        group.MapPost("/", async (
            [FromBody] Common.DamageRecord.DamageRecord damageRecord,
            [FromServices] ISaveDamageRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(damageRecord, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/damage-records/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDamageRecord");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DamageRecord.DamageRecord damageRecord,
            [FromServices] ISaveDamageRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                damageRecord.Id = id;
                var result = await action.UpdateAsync(damageRecord, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDamageRecord");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDamageRecordAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDamageRecord");
    }

    private static void AddDamageDisposalRecordApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/damage-disposal-records").WithTags("DamageDisposalRecords");

        group.MapGet("/", async (
            [FromServices] IGetDamageDisposalRecordAction action,
            [AsParameters] DamageDisposalRecordFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDamageDisposalRecords");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDamageDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDamageDisposalRecordById");

        group.MapPost("/", async (
            [FromBody] Common.DamageDisposalRecord.DamageDisposalRecord damageDisposalRecord,
            [FromServices] ISaveDamageDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/damage-disposal-records/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDamageDisposalRecord");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DamageDisposalRecord.DamageDisposalRecord damageDisposalRecord,
            [FromServices] ISaveDamageDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                damageDisposalRecord.Id = id;
                var result = await action.UpdateAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDamageDisposalRecord");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDamageDisposalRecordAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDamageDisposalRecord");
    }

    private static void AddDailyDiaryEntryApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/daily-diary-entries").WithTags("DailyDiaryEntries");

        group.MapGet("/", async (
            [FromServices] IGetDailyDiaryEntryAction action,
            [AsParameters] DailyDiaryEntryFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDailyDiaryEntries");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetDailyDiaryEntryAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDailyDiaryEntryById");

        group.MapPost("/", async (
            [FromBody] Common.DailyDiaryEntry.DailyDiaryEntry dailyDiaryEntry,
            [FromServices] ISaveDailyDiaryEntryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/daily-diary-entries/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateDailyDiaryEntry");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.DailyDiaryEntry.DailyDiaryEntry dailyDiaryEntry,
            [FromServices] ISaveDailyDiaryEntryAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                dailyDiaryEntry.Id = id;
                var result = await action.UpdateAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateDailyDiaryEntry");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveDailyDiaryEntryAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteDailyDiaryEntry");
    }

    private static void AddNotificationApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/notifications").WithTags("Notifications");

        group.MapGet("/", async (
            [FromServices] IGetNotificationAction action,
            [AsParameters] NotificationFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetNotifications");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetNotificationAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetNotificationById");

        group.MapPost("/", async (
            [FromBody] Common.Notification.Notification notification,
            [FromServices] ISaveNotificationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(notification, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/notifications/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateNotification");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Notification.Notification notification,
            [FromServices] ISaveNotificationAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                notification.Id = id;
                var result = await action.UpdateAsync(notification, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateNotification");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveNotificationAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteNotification");
    }

    private static void AddBranchApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/branches").WithTags("Branches");

        group.MapGet("/", async (
            [FromServices] IGetBranchAction action,
            [AsParameters] BranchFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetBranches");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetBranchAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetBranchById");

        group.MapPost("/", async (
            [FromBody] Common.Branch.Branch branch,
            [FromServices] ISaveBranchAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(branch, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/branches/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateBranch");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.Branch.Branch branch,
            [FromServices] ISaveBranchAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                branch.Id = id;
                var result = await action.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateBranch");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveBranchAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteBranch");
    }

    private static void AddPaymentLedgerApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/payment-ledger").WithTags("PaymentLedger");

        group.MapGet("/", async (
            [FromServices] IGetPaymentLedgerAction action,
            [AsParameters] PaymentLedgerFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetPaymentLedgers");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetPaymentLedgerAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetPaymentLedgerById");

        group.MapPost("/", async (
            [FromBody] Common.PaymentLedger.PaymentLedger paymentLedger,
            [FromServices] ISavePaymentLedgerAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(paymentLedger, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/payment-ledger/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreatePaymentLedger");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.PaymentLedger.PaymentLedger paymentLedger,
            [FromServices] ISavePaymentLedgerAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                paymentLedger.Id = id;
                var result = await action.UpdateAsync(paymentLedger, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdatePaymentLedger");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISavePaymentLedgerAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeletePaymentLedger");

        group.MapPost("/{id}/record-payment", async (
            Guid id,
            decimal amount,
            [FromServices] ISavePaymentLedgerAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.RecordPaymentAsync(id, amount, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("RecordPaymentLedgerPayment");
    }

    private static void AddPurchaseOrderApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/purchase-orders").WithTags("PurchaseOrders");

        group.MapGet("/", async (
            [FromServices] IGetPurchaseOrderAction action,
            [AsParameters] PurchaseOrderFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetPurchaseOrders");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetPurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetPurchaseOrderById");

        group.MapPost("/", async (
            [FromBody] Common.PurchaseOrder.PurchaseOrder purchaseOrder,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/purchase-orders/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreatePurchaseOrder");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.PurchaseOrder.PurchaseOrder purchaseOrder,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                purchaseOrder.Id = id;
                var result = await action.UpdateAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdatePurchaseOrder");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeletePurchaseOrder");

        group.MapPost("/{id}/approve", async (
            Guid id,
            string approvedBy,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.ApprovePurchaseOrderAsync(id, approvedBy, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("ApprovePurchaseOrder");

        group.MapPost("/{id}/reject", async (
            Guid id,
            string rejectedBy,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.RejectPurchaseOrderAsync(id, rejectedBy, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("RejectPurchaseOrder");

        group.MapPost("/{id}/receive", async (
            Guid id,
            [FromBody] IReadOnlyList<Common.PurchaseOrderItem.PurchaseOrderItem> receivedItems,
            [FromServices] ISavePurchaseOrderAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.VerifyConsignmentAsync(id, receivedItems, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(ex.Message);
            }
        }).WithName("ReceivePurchaseOrderConsignment");
    }

    private static void AddPurchaseOrderItemApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/purchase-order-items").WithTags("PurchaseOrderItems");

        group.MapGet("/", async (
            [FromServices] IGetPurchaseOrderItemAction action,
            [AsParameters] Common.PurchaseOrderItem.PurchaseOrderItemFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetPurchaseOrderItems");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetPurchaseOrderItemAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetPurchaseOrderItemById");

        group.MapPost("/", async (
            [FromBody] Common.PurchaseOrderItem.PurchaseOrderItem purchaseOrderItem,
            [FromServices] ISavePurchaseOrderItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/purchase-order-items/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreatePurchaseOrderItem");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.PurchaseOrderItem.PurchaseOrderItem purchaseOrderItem,
            [FromServices] ISavePurchaseOrderItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                purchaseOrderItem.Id = id;
                var result = await action.UpdateAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdatePurchaseOrderItem");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISavePurchaseOrderItemAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeletePurchaseOrderItem");
    }

    private static void AddAuditLogApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/audit-logs").WithTags("AuditLogs");

        group.MapGet("/", async (
            [FromServices] IGetAuditLogAction action,
            [AsParameters] AuditLogFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetAuditLogs");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetAuditLogAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetAuditLogById");
    }

    private static void AddReportApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/reports").WithTags("Reports");

        group.MapGet("/daily-sales", async (
            DateOnly date,
            [FromServices] IReportService reportService,
            CancellationToken cancellationToken) =>
        {
            var result = await reportService.GetDailySalesReportAsync(date, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetDailySalesReport");
    }

    private static void AddCustomerInvoiceApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/customer-invoices").WithTags("CustomerInvoices");

        group.MapGet("/", async (
            [FromServices] IGetCustomerInvoiceAction action,
            [AsParameters] CustomerInvoiceFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetCustomerInvoices");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetCustomerInvoiceAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetCustomerInvoiceById");

        group.MapPost("/", async (
            [FromBody] Common.CustomerInvoice.CustomerInvoice customerInvoice,
            [FromServices] ISaveCustomerInvoiceAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(customerInvoice, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/customer-invoices/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateCustomerInvoice");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.CustomerInvoice.CustomerInvoice customerInvoice,
            [FromServices] ISaveCustomerInvoiceAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                customerInvoice.Id = id;
                var result = await action.UpdateAsync(customerInvoice, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateCustomerInvoice");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveCustomerInvoiceAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteCustomerInvoice");
    }

    private static void AddCustomerInvoiceItemApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/customer-invoice-items").WithTags("CustomerInvoiceItems");

        group.MapGet("/", async (
            [FromServices] IGetCustomerInvoiceItemAction action,
            [AsParameters] CustomerInvoiceItemFilter filter,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
            return Results.Ok(result);
        }).WithName("GetCustomerInvoiceItems");

        group.MapGet("/{id}", async (
            string id,
            [FromServices] IGetCustomerInvoiceItemAction action,
            CancellationToken cancellationToken) =>
        {
            var result = await action.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetCustomerInvoiceItemById");

        group.MapPost("/", async (
            [FromBody] Common.CustomerInvoiceItem.CustomerInvoiceItem item,
            [FromServices] ISaveCustomerInvoiceItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.AddAsync(item, cancellationToken).ConfigureAwait(false);
                return Results.Created($"/api/customer-invoice-items/{result?.Id}", result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("CreateCustomerInvoiceItem");

        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] Common.CustomerInvoiceItem.CustomerInvoiceItem item,
            [FromServices] ISaveCustomerInvoiceItemAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                item.Id = id;
                var result = await action.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("UpdateCustomerInvoiceItem");

        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISaveCustomerInvoiceItemAction action,
            CancellationToken cancellationToken) =>
        {
            await action.RemoveAsync(id, "system", cancellationToken).ConfigureAwait(false);
            return Results.NoContent();
        }).WithName("DeleteCustomerInvoiceItem");
    }

    private static void AddAuthApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (
            [FromBody] LoginRequest request,
            [FromServices] ILoginAction action,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await action.LoginAsync(request, cancellationToken).ConfigureAwait(false);
                if (result is null)
                    return Results.Unauthorized();

                return Results.Ok(result);
            }
            catch (BadRequestException ex)
            {
                return Results.UnprocessableEntity(ex.Message);
            }
        }).WithName("Login").AllowAnonymous();
    }
}
