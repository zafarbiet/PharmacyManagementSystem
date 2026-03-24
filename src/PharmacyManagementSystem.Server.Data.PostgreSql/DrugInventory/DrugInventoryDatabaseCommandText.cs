using Dapper;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugInventory;

public static class DrugInventoryDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugId, BatchNumber, ExpirationDate, QuantityInStock, StorageConditions, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugInventoryFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DrugInventory WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DrugId.HasValue && filter.DrugId.Value != Guid.Empty)
        {
            sql += " AND DrugId = @DrugId";
            parameters.Add("DrugId", filter.DrugId);
        }

        if (!string.IsNullOrWhiteSpace(filter.BatchNumber))
        {
            sql += " AND BatchNumber LIKE @BatchNumber";
            parameters.Add("BatchNumber", $"%{filter.BatchNumber}%");
        }

        if (filter.ExpiresBeforeDate.HasValue)
        {
            sql += " AND ExpirationDate <= @ExpiresBeforeDate";
            parameters.Add("ExpiresBeforeDate", filter.ExpiresBeforeDate.Value);
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

        sql += " AND IsActive = true";

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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DrugInventory WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DrugInventory.DrugInventory drugInventory)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        var parameters = new DynamicParameters();
        parameters.Add("DrugId", drugInventory.DrugId);
        parameters.Add("BatchNumber", drugInventory.BatchNumber);
        parameters.Add("ExpirationDate", drugInventory.ExpirationDate);
        parameters.Add("QuantityInStock", drugInventory.QuantityInStock);
        parameters.Add("StorageConditions", drugInventory.StorageConditions);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugInventory.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DrugInventory (Id, DrugId, BatchNumber, ExpirationDate, QuantityInStock, StorageConditions, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @DrugId, @BatchNumber, @ExpirationDate, @QuantityInStock, @StorageConditions, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DrugInventory.DrugInventory drugInventory)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        var parameters = new DynamicParameters();
        parameters.Add("Id", drugInventory.Id);
        parameters.Add("DrugId", drugInventory.DrugId);
        parameters.Add("BatchNumber", drugInventory.BatchNumber);
        parameters.Add("ExpirationDate", drugInventory.ExpirationDate);
        parameters.Add("QuantityInStock", drugInventory.QuantityInStock);
        parameters.Add("StorageConditions", drugInventory.StorageConditions);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugInventory.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DrugInventory
                             SET DrugId = @DrugId, BatchNumber = @BatchNumber, ExpirationDate = @ExpirationDate,
                                 QuantityInStock = @QuantityInStock, StorageConditions = @StorageConditions,
                                 UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
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
            SqlStatement = @"UPDATE PMS.DrugInventory
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
