using Dapper;
using PharmacyManagementSystem.Common.DebtRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DebtRecord;

public static class DebtRecordDatabaseCommandText
{
    private const string SelectColumns = "Id, PatientId, InvoiceId, OriginalAmount, RemainingAmount, DueDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DebtRecordFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DebtRecords WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.PatientId != Guid.Empty)
        {
            sql += " AND PatientId = @PatientId";
            parameters.Add("PatientId", filter.PatientId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DebtRecords WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DebtRecord.DebtRecord debtRecord)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        var parameters = new DynamicParameters();
        parameters.Add("PatientId", debtRecord.PatientId);
        parameters.Add("InvoiceId", debtRecord.InvoiceId);
        parameters.Add("OriginalAmount", debtRecord.OriginalAmount);
        parameters.Add("RemainingAmount", debtRecord.RemainingAmount);
        parameters.Add("DueDate", debtRecord.DueDate);
        parameters.Add("Status", debtRecord.Status);
        parameters.Add("Notes", debtRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtRecord.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DebtRecords (Id, PatientId, InvoiceId, OriginalAmount, RemainingAmount, DueDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @PatientId, @InvoiceId, @OriginalAmount, @RemainingAmount, @DueDate, @Status, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DebtRecord.DebtRecord debtRecord)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        var parameters = new DynamicParameters();
        parameters.Add("Id", debtRecord.Id);
        parameters.Add("PatientId", debtRecord.PatientId);
        parameters.Add("InvoiceId", debtRecord.InvoiceId);
        parameters.Add("OriginalAmount", debtRecord.OriginalAmount);
        parameters.Add("RemainingAmount", debtRecord.RemainingAmount);
        parameters.Add("DueDate", debtRecord.DueDate);
        parameters.Add("Status", debtRecord.Status);
        parameters.Add("Notes", debtRecord.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtRecord.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DebtRecords
                             SET PatientId = @PatientId, InvoiceId = @InvoiceId, OriginalAmount = @OriginalAmount,
                                 RemainingAmount = @RemainingAmount, DueDate = @DueDate, Status = @Status,
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
            SqlStatement = @"UPDATE PMS.DebtRecords
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
