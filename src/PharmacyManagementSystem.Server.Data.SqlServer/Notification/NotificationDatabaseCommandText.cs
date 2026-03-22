using Dapper;
using PharmacyManagementSystem.Common.Notification;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Notification;

public static class NotificationDatabaseCommandText
{
    private const string SelectColumns = "Id, NotificationType, Channel, RecipientType, RecipientId, RecipientContact, Subject, Body, ReferenceId, ReferenceType, ScheduledAt, SentAt, Status, FailureReason, RetryCount, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(NotificationFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Notifications WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.NotificationType))
        {
            sql += " AND NotificationType = @NotificationType";
            parameters.Add("NotificationType", filter.NotificationType);
        }

        if (!string.IsNullOrWhiteSpace(filter.Channel))
        {
            sql += " AND Channel = @Channel";
            parameters.Add("Channel", filter.Channel);
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            sql += " AND Status = @Status";
            parameters.Add("Status", filter.Status);
        }

        if (filter.RecipientId != Guid.Empty)
        {
            sql += " AND RecipientId = @RecipientId";
            parameters.Add("RecipientId", filter.RecipientId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Notifications WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Notification.Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var parameters = new DynamicParameters();
        parameters.Add("NotificationType", notification.NotificationType);
        parameters.Add("Channel", notification.Channel);
        parameters.Add("RecipientType", notification.RecipientType);
        parameters.Add("RecipientId", notification.RecipientId);
        parameters.Add("RecipientContact", notification.RecipientContact);
        parameters.Add("Subject", notification.Subject);
        parameters.Add("Body", notification.Body);
        parameters.Add("ReferenceId", notification.ReferenceId);
        parameters.Add("ReferenceType", notification.ReferenceType);
        parameters.Add("ScheduledAt", notification.ScheduledAt);
        parameters.Add("SentAt", notification.SentAt);
        parameters.Add("Status", notification.Status);
        parameters.Add("FailureReason", notification.FailureReason);
        parameters.Add("RetryCount", notification.RetryCount);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", notification.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Notifications (Id, NotificationType, Channel, RecipientType, RecipientId, RecipientContact, Subject, Body, ReferenceId, ReferenceType, ScheduledAt, SentAt, Status, FailureReason, RetryCount, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @NotificationType, @Channel, @RecipientType, @RecipientId, @RecipientContact, @Subject, @Body, @ReferenceId, @ReferenceType, @ScheduledAt, @SentAt, @Status, @FailureReason, @RetryCount, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Notification.Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var parameters = new DynamicParameters();
        parameters.Add("Id", notification.Id);
        parameters.Add("NotificationType", notification.NotificationType);
        parameters.Add("Channel", notification.Channel);
        parameters.Add("RecipientType", notification.RecipientType);
        parameters.Add("RecipientId", notification.RecipientId);
        parameters.Add("RecipientContact", notification.RecipientContact);
        parameters.Add("Subject", notification.Subject);
        parameters.Add("Body", notification.Body);
        parameters.Add("ReferenceId", notification.ReferenceId);
        parameters.Add("ReferenceType", notification.ReferenceType);
        parameters.Add("ScheduledAt", notification.ScheduledAt);
        parameters.Add("SentAt", notification.SentAt);
        parameters.Add("Status", notification.Status);
        parameters.Add("FailureReason", notification.FailureReason);
        parameters.Add("RetryCount", notification.RetryCount);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", notification.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Notifications
                             SET NotificationType = @NotificationType, Channel = @Channel, RecipientType = @RecipientType,
                                 RecipientId = @RecipientId, RecipientContact = @RecipientContact,
                                 Subject = @Subject, Body = @Body, ReferenceId = @ReferenceId, ReferenceType = @ReferenceType,
                                 ScheduledAt = @ScheduledAt, SentAt = @SentAt, Status = @Status,
                                 FailureReason = @FailureReason, RetryCount = @RetryCount,
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
            SqlStatement = @"UPDATE PMS.Notifications
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
