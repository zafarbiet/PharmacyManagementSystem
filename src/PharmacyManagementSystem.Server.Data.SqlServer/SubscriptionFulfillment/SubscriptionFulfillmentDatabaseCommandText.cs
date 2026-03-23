using Dapper;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.SubscriptionFulfillment;

public static class SubscriptionFulfillmentDatabaseCommandText
{
    private const string SelectColumns = "Id, SubscriptionId, FulfillmentDate, Status, InvoiceId, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(SubscriptionFulfillmentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.SubscriptionFulfillments WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.SubscriptionId != Guid.Empty)
        {
            sql += " AND SubscriptionId = @SubscriptionId";
            parameters.Add("SubscriptionId", filter.SubscriptionId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.SubscriptionFulfillments WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.SubscriptionFulfillment.SubscriptionFulfillment subscriptionFulfillment)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        var parameters = new DynamicParameters();
        parameters.Add("SubscriptionId", subscriptionFulfillment.SubscriptionId);
        parameters.Add("FulfillmentDate", subscriptionFulfillment.FulfillmentDate);
        parameters.Add("Status", subscriptionFulfillment.Status);
        parameters.Add("InvoiceId", subscriptionFulfillment.InvoiceId);
        parameters.Add("Notes", subscriptionFulfillment.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", subscriptionFulfillment.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.SubscriptionFulfillments (Id, SubscriptionId, FulfillmentDate, Status, InvoiceId, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @SubscriptionId, @FulfillmentDate, @Status, @InvoiceId, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.SubscriptionFulfillment.SubscriptionFulfillment subscriptionFulfillment)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        var parameters = new DynamicParameters();
        parameters.Add("Id", subscriptionFulfillment.Id);
        parameters.Add("SubscriptionId", subscriptionFulfillment.SubscriptionId);
        parameters.Add("FulfillmentDate", subscriptionFulfillment.FulfillmentDate);
        parameters.Add("Status", subscriptionFulfillment.Status);
        parameters.Add("InvoiceId", subscriptionFulfillment.InvoiceId);
        parameters.Add("Notes", subscriptionFulfillment.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", subscriptionFulfillment.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.SubscriptionFulfillments
                             SET SubscriptionId = @SubscriptionId, FulfillmentDate = @FulfillmentDate,
                                 Status = @Status, InvoiceId = @InvoiceId, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.SubscriptionFulfillments
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
