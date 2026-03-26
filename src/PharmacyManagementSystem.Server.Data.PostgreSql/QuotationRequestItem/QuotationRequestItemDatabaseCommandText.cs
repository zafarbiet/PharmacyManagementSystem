using Dapper;
using PharmacyManagementSystem.Common.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequestItem;

public static class QuotationRequestItemDatabaseCommandText
{
    private const string SelectColumns = "Id, QuotationRequestId, DrugId, QuantityRequired, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(QuotationRequestItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.QuotationRequestItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.QuotationRequestId.HasValue && filter.QuotationRequestId.Value != Guid.Empty)
        {
            sql += " AND QuotationRequestId = @QuotationRequestId";
            parameters.Add("QuotationRequestId", filter.QuotationRequestId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.QuotationRequestItems WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.QuotationRequestItem.QuotationRequestItem quotationRequestItem)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        var parameters = new DynamicParameters();
        parameters.Add("QuotationRequestId", quotationRequestItem.QuotationRequestId);
        parameters.Add("DrugId", quotationRequestItem.DrugId);
        parameters.Add("QuantityRequired", quotationRequestItem.QuantityRequired);
        parameters.Add("Notes", quotationRequestItem.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationRequestItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.QuotationRequestItems (Id, QuotationRequestId, DrugId, QuantityRequired, Notes, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @QuotationRequestId, @DrugId, @QuantityRequired, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.QuotationRequestItem.QuotationRequestItem quotationRequestItem)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", quotationRequestItem.Id);
        parameters.Add("QuotationRequestId", quotationRequestItem.QuotationRequestId);
        parameters.Add("DrugId", quotationRequestItem.DrugId);
        parameters.Add("QuantityRequired", quotationRequestItem.QuantityRequired);
        parameters.Add("Notes", quotationRequestItem.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationRequestItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.QuotationRequestItems
                             SET QuotationRequestId = @QuotationRequestId, DrugId = @DrugId,
                                 QuantityRequired = @QuantityRequired, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.QuotationRequestItems
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
