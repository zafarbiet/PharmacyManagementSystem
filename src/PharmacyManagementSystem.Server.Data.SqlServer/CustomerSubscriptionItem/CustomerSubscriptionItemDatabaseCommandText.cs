using Dapper;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscriptionItem;

public static class CustomerSubscriptionItemDatabaseCommandText
{
    private const string SelectColumns = "Id, SubscriptionId, DrugId, QuantityPerCycle, PrescriptionId, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(CustomerSubscriptionItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.CustomerSubscriptionItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.SubscriptionId.HasValue && filter.SubscriptionId.Value != Guid.Empty)
        {
            sql += " AND SubscriptionId = @SubscriptionId";
            parameters.Add("SubscriptionId", filter.SubscriptionId);
        }

        if (filter.DrugId.HasValue && filter.DrugId.Value != Guid.Empty)
        {
            sql += " AND DrugId = @DrugId";
            parameters.Add("DrugId", filter.DrugId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.CustomerSubscriptionItems WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.CustomerSubscriptionItem.CustomerSubscriptionItem customerSubscriptionItem)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        var parameters = new DynamicParameters();
        parameters.Add("SubscriptionId", customerSubscriptionItem.SubscriptionId);
        parameters.Add("DrugId", customerSubscriptionItem.DrugId);
        parameters.Add("QuantityPerCycle", customerSubscriptionItem.QuantityPerCycle);
        parameters.Add("PrescriptionId", customerSubscriptionItem.PrescriptionId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerSubscriptionItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.CustomerSubscriptionItems (Id, SubscriptionId, DrugId, QuantityPerCycle, PrescriptionId, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @SubscriptionId, @DrugId, @QuantityPerCycle, @PrescriptionId, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.CustomerSubscriptionItem.CustomerSubscriptionItem customerSubscriptionItem)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", customerSubscriptionItem.Id);
        parameters.Add("SubscriptionId", customerSubscriptionItem.SubscriptionId);
        parameters.Add("DrugId", customerSubscriptionItem.DrugId);
        parameters.Add("QuantityPerCycle", customerSubscriptionItem.QuantityPerCycle);
        parameters.Add("PrescriptionId", customerSubscriptionItem.PrescriptionId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerSubscriptionItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.CustomerSubscriptionItems
                             SET SubscriptionId = @SubscriptionId, DrugId = @DrugId,
                                 QuantityPerCycle = @QuantityPerCycle, PrescriptionId = @PrescriptionId,
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
            SqlStatement = @"UPDATE PMS.CustomerSubscriptionItems
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
