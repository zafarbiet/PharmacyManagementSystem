using Dapper;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Drug;

public static class DrugDatabaseCommandText
{
    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = "SELECT Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, UpdatedAt, UpdatedBy, IsActive FROM PMS.Drugs WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
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
            SqlStatement = "SELECT Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, UpdatedAt, UpdatedBy, IsActive FROM PMS.Drugs WHERE Id = @Id AND IsActive = 1",
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
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drug.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Drugs (Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @Name, @GenericName, @ManufacturerName, @CategoryId, @UnitOfMeasure, @ReorderLevel, @UpdatedAt, @UpdatedBy, @IsActive)",
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
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drug.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Drugs
                             SET Name = @Name, GenericName = @GenericName, ManufacturerName = @ManufacturerName,
                                 CategoryId = @CategoryId, UnitOfMeasure = @UnitOfMeasure, ReorderLevel = @ReorderLevel,
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
