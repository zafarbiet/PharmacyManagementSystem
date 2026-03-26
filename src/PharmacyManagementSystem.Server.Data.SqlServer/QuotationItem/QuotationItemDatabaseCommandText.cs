using Dapper;
using PharmacyManagementSystem.Common.QuotationItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.QuotationItem;

public static class QuotationItemDatabaseCommandText
{
    private const string SelectColumns = "Id, QuotationId, DrugId, QuantityOffered, UnitPrice, DiscountPercent, GstRate, TotalAmount, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(QuotationItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.QuotationItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.QuotationId.HasValue && filter.QuotationId.Value != Guid.Empty)
        {
            sql += " AND QuotationId = @QuotationId";
            parameters.Add("QuotationId", filter.QuotationId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.QuotationItems WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.QuotationItem.QuotationItem quotationItem)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        var parameters = new DynamicParameters();
        parameters.Add("QuotationId", quotationItem.QuotationId);
        parameters.Add("DrugId", quotationItem.DrugId);
        parameters.Add("QuantityOffered", quotationItem.QuantityOffered);
        parameters.Add("UnitPrice", quotationItem.UnitPrice);
        parameters.Add("DiscountPercent", quotationItem.DiscountPercent);
        parameters.Add("GstRate", quotationItem.GstRate);
        parameters.Add("TotalAmount", quotationItem.TotalAmount);
        parameters.Add("Notes", quotationItem.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.QuotationItems (Id, QuotationId, DrugId, QuantityOffered, UnitPrice, DiscountPercent, GstRate, TotalAmount, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @QuotationId, @DrugId, @QuantityOffered, @UnitPrice, @DiscountPercent, @GstRate, @TotalAmount, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.QuotationItem.QuotationItem quotationItem)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", quotationItem.Id);
        parameters.Add("QuotationId", quotationItem.QuotationId);
        parameters.Add("DrugId", quotationItem.DrugId);
        parameters.Add("QuantityOffered", quotationItem.QuantityOffered);
        parameters.Add("UnitPrice", quotationItem.UnitPrice);
        parameters.Add("DiscountPercent", quotationItem.DiscountPercent);
        parameters.Add("GstRate", quotationItem.GstRate);
        parameters.Add("TotalAmount", quotationItem.TotalAmount);
        parameters.Add("Notes", quotationItem.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.QuotationItems
                             SET QuotationId = @QuotationId, DrugId = @DrugId, QuantityOffered = @QuantityOffered,
                                 UnitPrice = @UnitPrice, DiscountPercent = @DiscountPercent, GstRate = @GstRate,
                                 TotalAmount = @TotalAmount, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.QuotationItems
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
