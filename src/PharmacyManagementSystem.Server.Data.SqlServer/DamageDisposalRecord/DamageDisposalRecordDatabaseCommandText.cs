using Dapper;
using PharmacyManagementSystem.Common.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DamageDisposalRecord;

public static class DamageDisposalRecordDatabaseCommandText
{
    private const string SelectColumns = "Id, DamageRecordId, DisposedAt, DisposalMethod, DisposedBy, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DamageDisposalRecordFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DamageDisposalRecords WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DamageRecordId != Guid.Empty)
        {
            sql += " AND DamageRecordId = @DamageRecordId";
            parameters.Add("DamageRecordId", filter.DamageRecordId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DamageDisposalRecords WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DamageDisposalRecord.DamageDisposalRecord damageDisposalRecord)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        var parameters = new DynamicParameters();
        parameters.Add("DamageRecordId", damageDisposalRecord.DamageRecordId);
        parameters.Add("DisposedAt", damageDisposalRecord.DisposedAt);
        parameters.Add("DisposalMethod", damageDisposalRecord.DisposalMethod);
        parameters.Add("DisposedBy", damageDisposalRecord.DisposedBy);
        parameters.Add("Notes", damageDisposalRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", damageDisposalRecord.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DamageDisposalRecords (Id, DamageRecordId, DisposedAt, DisposalMethod, DisposedBy, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DamageRecordId, @DisposedAt, @DisposalMethod, @DisposedBy, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DamageDisposalRecord.DamageDisposalRecord damageDisposalRecord)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        var parameters = new DynamicParameters();
        parameters.Add("Id", damageDisposalRecord.Id);
        parameters.Add("DamageRecordId", damageDisposalRecord.DamageRecordId);
        parameters.Add("DisposedAt", damageDisposalRecord.DisposedAt);
        parameters.Add("DisposalMethod", damageDisposalRecord.DisposalMethod);
        parameters.Add("DisposedBy", damageDisposalRecord.DisposedBy);
        parameters.Add("Notes", damageDisposalRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", damageDisposalRecord.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DamageDisposalRecords
                             SET DamageRecordId = @DamageRecordId, DisposedAt = @DisposedAt,
                                 DisposalMethod = @DisposalMethod, DisposedBy = @DisposedBy, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.DamageDisposalRecords
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
