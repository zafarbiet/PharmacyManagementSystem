using Dapper;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DrugInventoryRackAssignment;

public static class DrugInventoryRackAssignmentDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugInventoryId, RackId, QuantityPlaced, PlacedAt, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugInventoryRackAssignmentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DrugInventoryRackAssignment WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DrugInventoryId.HasValue && filter.DrugInventoryId.Value != Guid.Empty)
        {
            sql += " AND DrugInventoryId = @DrugInventoryId";
            parameters.Add("DrugInventoryId", filter.DrugInventoryId);
        }

        if (filter.RackId.HasValue && filter.RackId.Value != Guid.Empty)
        {
            sql += " AND RackId = @RackId";
            parameters.Add("RackId", filter.RackId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DrugInventoryRackAssignment WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment drugInventoryRackAssignment)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        var parameters = new DynamicParameters();
        parameters.Add("DrugInventoryId", drugInventoryRackAssignment.DrugInventoryId);
        parameters.Add("RackId", drugInventoryRackAssignment.RackId);
        parameters.Add("QuantityPlaced", drugInventoryRackAssignment.QuantityPlaced);
        parameters.Add("PlacedAt", drugInventoryRackAssignment.PlacedAt);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugInventoryRackAssignment.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DrugInventoryRackAssignment (Id, DrugInventoryId, RackId, QuantityPlaced, PlacedAt, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DrugInventoryId, @RackId, @QuantityPlaced, @PlacedAt, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment drugInventoryRackAssignment)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        var parameters = new DynamicParameters();
        parameters.Add("Id", drugInventoryRackAssignment.Id);
        parameters.Add("DrugInventoryId", drugInventoryRackAssignment.DrugInventoryId);
        parameters.Add("RackId", drugInventoryRackAssignment.RackId);
        parameters.Add("QuantityPlaced", drugInventoryRackAssignment.QuantityPlaced);
        parameters.Add("PlacedAt", drugInventoryRackAssignment.PlacedAt);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugInventoryRackAssignment.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DrugInventoryRackAssignment
                             SET DrugInventoryId = @DrugInventoryId, RackId = @RackId, QuantityPlaced = @QuantityPlaced,
                                 PlacedAt = @PlacedAt, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.DrugInventoryRackAssignment
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
