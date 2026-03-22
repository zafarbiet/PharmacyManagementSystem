using Dapper;
using PharmacyManagementSystem.Common.StorageZone;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.StorageZone;

public static class StorageZoneDatabaseCommandText
{
    private const string SelectColumns = "Id, Name, ZoneType, Description, TemperatureRangeMin, TemperatureRangeMax, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(StorageZoneFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.StorageZones WHERE 1=1";
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

        if (!string.IsNullOrWhiteSpace(filter.ZoneType))
        {
            sql += " AND ZoneType = @ZoneType";
            parameters.Add("ZoneType", filter.ZoneType);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.StorageZones WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.StorageZone.StorageZone storageZone)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        var parameters = new DynamicParameters();
        parameters.Add("Name", storageZone.Name);
        parameters.Add("ZoneType", storageZone.ZoneType);
        parameters.Add("Description", storageZone.Description);
        parameters.Add("TemperatureRangeMin", storageZone.TemperatureRangeMin);
        parameters.Add("TemperatureRangeMax", storageZone.TemperatureRangeMax);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", storageZone.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.StorageZones (Id, Name, ZoneType, Description, TemperatureRangeMin, TemperatureRangeMax, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @Name, @ZoneType, @Description, @TemperatureRangeMin, @TemperatureRangeMax, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.StorageZone.StorageZone storageZone)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        var parameters = new DynamicParameters();
        parameters.Add("Id", storageZone.Id);
        parameters.Add("Name", storageZone.Name);
        parameters.Add("ZoneType", storageZone.ZoneType);
        parameters.Add("Description", storageZone.Description);
        parameters.Add("TemperatureRangeMin", storageZone.TemperatureRangeMin);
        parameters.Add("TemperatureRangeMax", storageZone.TemperatureRangeMax);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", storageZone.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.StorageZones
                             SET Name = @Name, ZoneType = @ZoneType, Description = @Description,
                                 TemperatureRangeMin = @TemperatureRangeMin, TemperatureRangeMax = @TemperatureRangeMax,
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
            SqlStatement = @"UPDATE PMS.StorageZones
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
