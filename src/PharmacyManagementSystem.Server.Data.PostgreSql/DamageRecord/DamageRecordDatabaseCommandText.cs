using Dapper;
using PharmacyManagementSystem.Common.DamageRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DamageRecord;

public static class DamageRecordDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugInventoryId, QuantityDamaged, DamageType, DamagedAt, DiscoveredBy, Status, QuarantineRackId, StockTransactionId, ApprovedBy, ApprovedAt, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DamageRecordFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DamageRecords WHERE 1=1";
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

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            sql += " AND Status = @Status";
            parameters.Add("Status", filter.Status);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DamageRecords WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DamageRecord.DamageRecord damageRecord)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        var parameters = new DynamicParameters();
        parameters.Add("DrugInventoryId", damageRecord.DrugInventoryId);
        parameters.Add("QuantityDamaged", damageRecord.QuantityDamaged);
        parameters.Add("DamageType", damageRecord.DamageType);
        parameters.Add("DamagedAt", damageRecord.DamagedAt);
        parameters.Add("DiscoveredBy", damageRecord.DiscoveredBy);
        parameters.Add("Status", damageRecord.Status);
        parameters.Add("QuarantineRackId", damageRecord.QuarantineRackId);
        parameters.Add("StockTransactionId", damageRecord.StockTransactionId);
        parameters.Add("ApprovedBy", damageRecord.ApprovedBy);
        parameters.Add("ApprovedAt", damageRecord.ApprovedAt);
        parameters.Add("Notes", damageRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", damageRecord.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DamageRecords (Id, DrugInventoryId, QuantityDamaged, DamageType, DamagedAt, DiscoveredBy, Status, QuarantineRackId, StockTransactionId, ApprovedBy, ApprovedAt, Notes, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @DrugInventoryId, @QuantityDamaged, @DamageType, @DamagedAt, @DiscoveredBy, @Status, @QuarantineRackId, @StockTransactionId, @ApprovedBy, @ApprovedAt, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DamageRecord.DamageRecord damageRecord)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        var parameters = new DynamicParameters();
        parameters.Add("Id", damageRecord.Id);
        parameters.Add("DrugInventoryId", damageRecord.DrugInventoryId);
        parameters.Add("QuantityDamaged", damageRecord.QuantityDamaged);
        parameters.Add("DamageType", damageRecord.DamageType);
        parameters.Add("DamagedAt", damageRecord.DamagedAt);
        parameters.Add("DiscoveredBy", damageRecord.DiscoveredBy);
        parameters.Add("Status", damageRecord.Status);
        parameters.Add("QuarantineRackId", damageRecord.QuarantineRackId);
        parameters.Add("StockTransactionId", damageRecord.StockTransactionId);
        parameters.Add("ApprovedBy", damageRecord.ApprovedBy);
        parameters.Add("ApprovedAt", damageRecord.ApprovedAt);
        parameters.Add("Notes", damageRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", damageRecord.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DamageRecords
                             SET DrugInventoryId = @DrugInventoryId, QuantityDamaged = @QuantityDamaged,
                                 DamageType = @DamageType, DamagedAt = @DamagedAt, DiscoveredBy = @DiscoveredBy,
                                 Status = @Status, QuarantineRackId = @QuarantineRackId, StockTransactionId = @StockTransactionId,
                                 ApprovedBy = @ApprovedBy, ApprovedAt = @ApprovedAt, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.DamageRecords
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
