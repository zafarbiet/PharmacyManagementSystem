using Dapper;
using PharmacyManagementSystem.Common.AuditLog;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.AuditLog;

public static class AuditLogDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugId, DrugName, ScheduleCategory, CustomerInvoiceId, PrescriptionId, PatientId, QuantityDispensed, PerformedBy, PerformedAt, RetentionUntil, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(AuditLogFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.AuditLogs WHERE 1=1";
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

        if (filter.CustomerInvoiceId.HasValue && filter.CustomerInvoiceId.Value != Guid.Empty)
        {
            sql += " AND CustomerInvoiceId = @CustomerInvoiceId";
            parameters.Add("CustomerInvoiceId", filter.CustomerInvoiceId);
        }

        if (filter.PatientId.HasValue && filter.PatientId.Value != Guid.Empty)
        {
            sql += " AND PatientId = @PatientId";
            parameters.Add("PatientId", filter.PatientId);
        }

        if (!string.IsNullOrWhiteSpace(filter.ScheduleCategory))
        {
            sql += " AND ScheduleCategory = @ScheduleCategory";
            parameters.Add("ScheduleCategory", filter.ScheduleCategory);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.AuditLogs WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.AuditLog.AuditLog auditLog)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        var parameters = new DynamicParameters();
        parameters.Add("DrugId", auditLog.DrugId);
        parameters.Add("DrugName", auditLog.DrugName);
        parameters.Add("ScheduleCategory", auditLog.ScheduleCategory);
        parameters.Add("CustomerInvoiceId", auditLog.CustomerInvoiceId);
        parameters.Add("PrescriptionId", auditLog.PrescriptionId);
        parameters.Add("PatientId", auditLog.PatientId);
        parameters.Add("QuantityDispensed", auditLog.QuantityDispensed);
        parameters.Add("PerformedBy", auditLog.PerformedBy);
        parameters.Add("PerformedAt", auditLog.PerformedAt);
        parameters.Add("RetentionUntil", auditLog.RetentionUntil);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", auditLog.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.AuditLogs (Id, DrugId, DrugName, ScheduleCategory, CustomerInvoiceId, PrescriptionId, PatientId, QuantityDispensed, PerformedBy, PerformedAt, RetentionUntil, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DrugId, @DrugName, @ScheduleCategory, @CustomerInvoiceId, @PrescriptionId, @PatientId, @QuantityDispensed, @PerformedBy, @PerformedAt, @RetentionUntil, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }
}
