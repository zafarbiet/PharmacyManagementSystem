using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Common.DrugCategory;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Common.Vendor;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.Vendor;

namespace PharmacyManagementSystem.Server.Host;

public static class ControllerExtensions
{
    public static WebApplication AddMinimalApis(this WebApplication app)
    {
        app.AddDrugCategoryApis();
        app.AddDrugApis();
        app.AddVendorApis();
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
}
