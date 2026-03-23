using Dapper;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscription;

public static class CustomerSubscriptionDatabaseCommandText
{
    private const string SelectColumns = "Id, PatientId, StartDate, EndDate, CycleDayOfMonth, Status, ApprovalStatus, ApprovedBy, ApprovedAt, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(CustomerSubscriptionFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.CustomerSubscriptions WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
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

        if (!string.IsNullOrWhiteSpace(filter.ApprovalStatus))
        {
            sql += " AND ApprovalStatus = @ApprovalStatus";
            parameters.Add("ApprovalStatus", filter.ApprovalStatus);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.CustomerSubscriptions WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.CustomerSubscription.CustomerSubscription customerSubscription)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        var parameters = new DynamicParameters();
        parameters.Add("PatientId", customerSubscription.PatientId);
        parameters.Add("StartDate", customerSubscription.StartDate);
        parameters.Add("EndDate", customerSubscription.EndDate);
        parameters.Add("CycleDayOfMonth", customerSubscription.CycleDayOfMonth);
        parameters.Add("Status", customerSubscription.Status);
        parameters.Add("ApprovalStatus", customerSubscription.ApprovalStatus);
        parameters.Add("ApprovedBy", customerSubscription.ApprovedBy);
        parameters.Add("ApprovedAt", customerSubscription.ApprovedAt);
        parameters.Add("Notes", customerSubscription.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerSubscription.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.CustomerSubscriptions (Id, PatientId, StartDate, EndDate, CycleDayOfMonth, Status, ApprovalStatus, ApprovedBy, ApprovedAt, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @PatientId, @StartDate, @EndDate, @CycleDayOfMonth, @Status, @ApprovalStatus, @ApprovedBy, @ApprovedAt, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.CustomerSubscription.CustomerSubscription customerSubscription)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        var parameters = new DynamicParameters();
        parameters.Add("Id", customerSubscription.Id);
        parameters.Add("PatientId", customerSubscription.PatientId);
        parameters.Add("StartDate", customerSubscription.StartDate);
        parameters.Add("EndDate", customerSubscription.EndDate);
        parameters.Add("CycleDayOfMonth", customerSubscription.CycleDayOfMonth);
        parameters.Add("Status", customerSubscription.Status);
        parameters.Add("ApprovalStatus", customerSubscription.ApprovalStatus);
        parameters.Add("ApprovedBy", customerSubscription.ApprovedBy);
        parameters.Add("ApprovedAt", customerSubscription.ApprovedAt);
        parameters.Add("Notes", customerSubscription.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerSubscription.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.CustomerSubscriptions
                             SET PatientId = @PatientId, StartDate = @StartDate, EndDate = @EndDate,
                                 CycleDayOfMonth = @CycleDayOfMonth, Status = @Status,
                                 ApprovalStatus = @ApprovalStatus,
                                 ApprovedBy = @ApprovedBy, ApprovedAt = @ApprovedAt, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.CustomerSubscriptions
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
