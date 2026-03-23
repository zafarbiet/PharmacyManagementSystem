using Dapper;
using PharmacyManagementSystem.Common.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.ExpiryRecord;

public static class ExpiryRecordDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugInventoryId, DetectedAt, ExpirationDate, QuantityAffected, Status, InitiatedBy, ApprovedBy, ApprovedAt, QuarantineRackId, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(ExpiryRecordFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.ExpiryRecords WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DrugInventoryId != Guid.Empty)
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.ExpiryRecords WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.ExpiryRecord.ExpiryRecord expiryRecord)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        var parameters = new DynamicParameters();
        parameters.Add("DrugInventoryId", expiryRecord.DrugInventoryId);
        parameters.Add("DetectedAt", expiryRecord.DetectedAt);
        parameters.Add("ExpirationDate", expiryRecord.ExpirationDate);
        parameters.Add("QuantityAffected", expiryRecord.QuantityAffected);
        parameters.Add("Status", expiryRecord.Status);
        parameters.Add("InitiatedBy", expiryRecord.InitiatedBy);
        parameters.Add("ApprovedBy", expiryRecord.ApprovedBy);
        parameters.Add("ApprovedAt", expiryRecord.ApprovedAt);
        parameters.Add("QuarantineRackId", expiryRecord.QuarantineRackId);
        parameters.Add("Notes", expiryRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", expiryRecord.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.ExpiryRecords (Id, DrugInventoryId, DetectedAt, ExpirationDate, QuantityAffected, Status, InitiatedBy, ApprovedBy, ApprovedAt, QuarantineRackId, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DrugInventoryId, @DetectedAt, @ExpirationDate, @QuantityAffected, @Status, @InitiatedBy, @ApprovedBy, @ApprovedAt, @QuarantineRackId, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.ExpiryRecord.ExpiryRecord expiryRecord)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        var parameters = new DynamicParameters();
        parameters.Add("Id", expiryRecord.Id);
        parameters.Add("DrugInventoryId", expiryRecord.DrugInventoryId);
        parameters.Add("DetectedAt", expiryRecord.DetectedAt);
        parameters.Add("ExpirationDate", expiryRecord.ExpirationDate);
        parameters.Add("QuantityAffected", expiryRecord.QuantityAffected);
        parameters.Add("Status", expiryRecord.Status);
        parameters.Add("InitiatedBy", expiryRecord.InitiatedBy);
        parameters.Add("ApprovedBy", expiryRecord.ApprovedBy);
        parameters.Add("ApprovedAt", expiryRecord.ApprovedAt);
        parameters.Add("QuarantineRackId", expiryRecord.QuarantineRackId);
        parameters.Add("Notes", expiryRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", expiryRecord.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.ExpiryRecords
                             SET DrugInventoryId = @DrugInventoryId, DetectedAt = @DetectedAt, ExpirationDate = @ExpirationDate,
                                 QuantityAffected = @QuantityAffected, Status = @Status, InitiatedBy = @InitiatedBy,
                                 ApprovedBy = @ApprovedBy, ApprovedAt = @ApprovedAt, QuarantineRackId = @QuarantineRackId,
                                 Notes = @Notes, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.ExpiryRecords
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
