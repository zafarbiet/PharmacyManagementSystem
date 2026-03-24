using Dapper;
using PharmacyManagementSystem.Common.DebtReminder;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DebtReminder;

public static class DebtReminderDatabaseCommandText
{
    private const string SelectColumns = "Id, DebtRecordId, SentAt, Channel, SentBy, Message, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DebtReminderFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DebtReminders WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DebtRecordId.HasValue && filter.DebtRecordId.Value != Guid.Empty)
        {
            sql += " AND DebtRecordId = @DebtRecordId";
            parameters.Add("DebtRecordId", filter.DebtRecordId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Channel))
        {
            sql += " AND Channel = @Channel";
            parameters.Add("Channel", filter.Channel);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DebtReminders WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DebtReminder.DebtReminder debtReminder)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        var parameters = new DynamicParameters();
        parameters.Add("DebtRecordId", debtReminder.DebtRecordId);
        parameters.Add("SentAt", debtReminder.SentAt);
        parameters.Add("Channel", debtReminder.Channel);
        parameters.Add("SentBy", debtReminder.SentBy);
        parameters.Add("Message", debtReminder.Message);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtReminder.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DebtReminders (Id, DebtRecordId, SentAt, Channel, SentBy, Message, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DebtRecordId, @SentAt, @Channel, @SentBy, @Message, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DebtReminder.DebtReminder debtReminder)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        var parameters = new DynamicParameters();
        parameters.Add("Id", debtReminder.Id);
        parameters.Add("DebtRecordId", debtReminder.DebtRecordId);
        parameters.Add("SentAt", debtReminder.SentAt);
        parameters.Add("Channel", debtReminder.Channel);
        parameters.Add("SentBy", debtReminder.SentBy);
        parameters.Add("Message", debtReminder.Message);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtReminder.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DebtReminders
                             SET DebtRecordId = @DebtRecordId, SentAt = @SentAt, Channel = @Channel,
                                 SentBy = @SentBy, Message = @Message,
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
            SqlStatement = @"UPDATE PMS.DebtReminders
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
