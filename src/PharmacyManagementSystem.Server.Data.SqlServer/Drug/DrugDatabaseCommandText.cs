using Dapper;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Drug;

public static class DrugDatabaseCommandText
{
    private const string SelectColumns =
        "Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, " +
        "BrandName, DosageForm, Strength, Description, DrugLicenseNumber, ApprovalDate, " +
        "ScheduleCategory, PrescriptionRequired, HsnCode, GstSlab, Composition, Mrp, " +
        "UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Drugs WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            sql += " AND Name LIKE @Name";
            parameters.Add("Name", $"%{filter.Name}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.GenericName))
        {
            sql += " AND GenericName LIKE @GenericName";
            parameters.Add("GenericName", $"%{filter.GenericName}%");
        }

        if (filter.CategoryId.HasValue && filter.CategoryId.Value != Guid.Empty)
        {
            sql += " AND CategoryId = @CategoryId";
            parameters.Add("CategoryId", filter.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Composition))
        {
            sql += " AND Composition LIKE @Composition";
            parameters.Add("Composition", $"%{filter.Composition}%");
        }

        if (filter.DateFrom.HasValue)
        {
            sql += " AND UpdatedAt >= @DateFrom";
            parameters.Add("DateFrom", filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            sql += " AND UpdatedAt <= @DateTo";
            parameters.Add("DateTo", filter.DateTo.Value);
        }

        sql += " AND IsActive = 1";

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = sql,
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Drugs WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Drug.Drug drug)
    {
        ArgumentNullException.ThrowIfNull(drug);

        var parameters = new DynamicParameters();
        parameters.Add("Name", drug.Name);
        parameters.Add("GenericName", drug.GenericName);
        parameters.Add("ManufacturerName", drug.ManufacturerName);
        parameters.Add("CategoryId", drug.CategoryId);
        parameters.Add("UnitOfMeasure", drug.UnitOfMeasure);
        parameters.Add("ReorderLevel", drug.ReorderLevel);
        parameters.Add("BrandName", drug.BrandName);
        parameters.Add("DosageForm", drug.DosageForm);
        parameters.Add("Strength", drug.Strength);
        parameters.Add("Description", drug.Description);
        parameters.Add("DrugLicenseNumber", drug.DrugLicenseNumber);
        parameters.Add("ApprovalDate", drug.ApprovalDate);
        parameters.Add("ScheduleCategory", drug.ScheduleCategory);
        parameters.Add("PrescriptionRequired", drug.PrescriptionRequired);
        parameters.Add("HsnCode", drug.HsnCode);
        parameters.Add("GstSlab", drug.GstSlab);
        parameters.Add("Composition", drug.Composition);
        parameters.Add("Mrp", drug.Mrp);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drug.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Drugs
                             (Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel,
                              BrandName, DosageForm, Strength, Description, DrugLicenseNumber, ApprovalDate,
                              ScheduleCategory, PrescriptionRequired, HsnCode, GstSlab, Composition, Mrp,
                              UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES
                             (NEWID(), @Name, @GenericName, @ManufacturerName, @CategoryId, @UnitOfMeasure, @ReorderLevel,
                              @BrandName, @DosageForm, @Strength, @Description, @DrugLicenseNumber, @ApprovalDate,
                              @ScheduleCategory, @PrescriptionRequired, @HsnCode, @GstSlab, @Composition, @Mrp,
                              @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Drug.Drug drug)
    {
        ArgumentNullException.ThrowIfNull(drug);

        var parameters = new DynamicParameters();
        parameters.Add("Id", drug.Id);
        parameters.Add("Name", drug.Name);
        parameters.Add("GenericName", drug.GenericName);
        parameters.Add("ManufacturerName", drug.ManufacturerName);
        parameters.Add("CategoryId", drug.CategoryId);
        parameters.Add("UnitOfMeasure", drug.UnitOfMeasure);
        parameters.Add("ReorderLevel", drug.ReorderLevel);
        parameters.Add("BrandName", drug.BrandName);
        parameters.Add("DosageForm", drug.DosageForm);
        parameters.Add("Strength", drug.Strength);
        parameters.Add("Description", drug.Description);
        parameters.Add("DrugLicenseNumber", drug.DrugLicenseNumber);
        parameters.Add("ApprovalDate", drug.ApprovalDate);
        parameters.Add("ScheduleCategory", drug.ScheduleCategory);
        parameters.Add("PrescriptionRequired", drug.PrescriptionRequired);
        parameters.Add("HsnCode", drug.HsnCode);
        parameters.Add("GstSlab", drug.GstSlab);
        parameters.Add("Composition", drug.Composition);
        parameters.Add("Mrp", drug.Mrp);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drug.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Drugs
                             SET Name = @Name, GenericName = @GenericName, ManufacturerName = @ManufacturerName,
                                 CategoryId = @CategoryId, UnitOfMeasure = @UnitOfMeasure, ReorderLevel = @ReorderLevel,
                                 BrandName = @BrandName, DosageForm = @DosageForm, Strength = @Strength,
                                 Description = @Description, DrugLicenseNumber = @DrugLicenseNumber, ApprovalDate = @ApprovalDate,
                                 ScheduleCategory = @ScheduleCategory, PrescriptionRequired = @PrescriptionRequired,
                                 HsnCode = @HsnCode, GstSlab = @GstSlab, Composition = @Composition, Mrp = @Mrp,
                                 UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetSoftDeleteSql(Guid id, string updatedBy)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", updatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Drugs
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
