using Dapper;
using PharmacyManagementSystem.Common.DisposalRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DisposalRecord;

public static class DisposalRecordDatabaseCommandText
{
    private const string SelectColumns = "Id, ExpiryRecordId, DisposedAt, QuantityDisposed, DisposalMethod, DisposedBy, WitnessedBy, RegulatoryReferenceNumber, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DisposalRecordFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DisposalRecords WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.ExpiryRecordId.HasValue && filter.ExpiryRecordId.Value != Guid.Empty)
        {
            sql += " AND ExpiryRecordId = @ExpiryRecordId";
            parameters.Add("ExpiryRecordId", filter.ExpiryRecordId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DisposalRecords WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DisposalRecord.DisposalRecord disposalRecord)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        var parameters = new DynamicParameters();
        parameters.Add("ExpiryRecordId", disposalRecord.ExpiryRecordId);
        parameters.Add("DisposedAt", disposalRecord.DisposedAt);
        parameters.Add("QuantityDisposed", disposalRecord.QuantityDisposed);
        parameters.Add("DisposalMethod", disposalRecord.DisposalMethod);
        parameters.Add("DisposedBy", disposalRecord.DisposedBy);
        parameters.Add("WitnessedBy", disposalRecord.WitnessedBy);
        parameters.Add("RegulatoryReferenceNumber", disposalRecord.RegulatoryReferenceNumber);
        parameters.Add("Notes", disposalRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", disposalRecord.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DisposalRecords (Id, ExpiryRecordId, DisposedAt, QuantityDisposed, DisposalMethod, DisposedBy, WitnessedBy, RegulatoryReferenceNumber, Notes, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @ExpiryRecordId, @DisposedAt, @QuantityDisposed, @DisposalMethod, @DisposedBy, @WitnessedBy, @RegulatoryReferenceNumber, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DisposalRecord.DisposalRecord disposalRecord)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        var parameters = new DynamicParameters();
        parameters.Add("Id", disposalRecord.Id);
        parameters.Add("ExpiryRecordId", disposalRecord.ExpiryRecordId);
        parameters.Add("DisposedAt", disposalRecord.DisposedAt);
        parameters.Add("QuantityDisposed", disposalRecord.QuantityDisposed);
        parameters.Add("DisposalMethod", disposalRecord.DisposalMethod);
        parameters.Add("DisposedBy", disposalRecord.DisposedBy);
        parameters.Add("WitnessedBy", disposalRecord.WitnessedBy);
        parameters.Add("RegulatoryReferenceNumber", disposalRecord.RegulatoryReferenceNumber);
        parameters.Add("Notes", disposalRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", disposalRecord.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DisposalRecords
                             SET ExpiryRecordId = @ExpiryRecordId, DisposedAt = @DisposedAt, QuantityDisposed = @QuantityDisposed,
                                 DisposalMethod = @DisposalMethod, DisposedBy = @DisposedBy, WitnessedBy = @WitnessedBy,
                                 RegulatoryReferenceNumber = @RegulatoryReferenceNumber, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.DisposalRecords
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
