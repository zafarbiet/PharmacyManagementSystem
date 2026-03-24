using Dapper;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.CustomerInvoiceItem;

public static class CustomerInvoiceItemDatabaseCommandText
{
    private const string SelectColumns = "Id, InvoiceId, DrugId, BatchNumber, Quantity, UnitPrice, DiscountPercent, GstRate, Amount, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(CustomerInvoiceItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.CustomerInvoiceItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.InvoiceId != Guid.Empty)
        {
            sql += " AND InvoiceId = @InvoiceId";
            parameters.Add("InvoiceId", filter.InvoiceId);
        }

        if (filter.DrugId != Guid.Empty)
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.CustomerInvoiceItems WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.CustomerInvoiceItem.CustomerInvoiceItem customerInvoiceItem)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        var parameters = new DynamicParameters();
        parameters.Add("InvoiceId", customerInvoiceItem.InvoiceId);
        parameters.Add("DrugId", customerInvoiceItem.DrugId);
        parameters.Add("BatchNumber", customerInvoiceItem.BatchNumber);
        parameters.Add("Quantity", customerInvoiceItem.Quantity);
        parameters.Add("UnitPrice", customerInvoiceItem.UnitPrice);
        parameters.Add("DiscountPercent", customerInvoiceItem.DiscountPercent);
        parameters.Add("GstRate", customerInvoiceItem.GstRate);
        parameters.Add("Amount", customerInvoiceItem.Amount);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerInvoiceItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.CustomerInvoiceItems (Id, InvoiceId, DrugId, BatchNumber, Quantity, UnitPrice, DiscountPercent, GstRate, Amount, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @InvoiceId, @DrugId, @BatchNumber, @Quantity, @UnitPrice, @DiscountPercent, @GstRate, @Amount, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.CustomerInvoiceItem.CustomerInvoiceItem customerInvoiceItem)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", customerInvoiceItem.Id);
        parameters.Add("InvoiceId", customerInvoiceItem.InvoiceId);
        parameters.Add("DrugId", customerInvoiceItem.DrugId);
        parameters.Add("BatchNumber", customerInvoiceItem.BatchNumber);
        parameters.Add("Quantity", customerInvoiceItem.Quantity);
        parameters.Add("UnitPrice", customerInvoiceItem.UnitPrice);
        parameters.Add("DiscountPercent", customerInvoiceItem.DiscountPercent);
        parameters.Add("GstRate", customerInvoiceItem.GstRate);
        parameters.Add("Amount", customerInvoiceItem.Amount);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerInvoiceItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.CustomerInvoiceItems
                             SET InvoiceId = @InvoiceId, DrugId = @DrugId, BatchNumber = @BatchNumber,
                                 Quantity = @Quantity, UnitPrice = @UnitPrice, DiscountPercent = @DiscountPercent,
                                 GstRate = @GstRate, Amount = @Amount,
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
            SqlStatement = @"UPDATE PMS.CustomerInvoiceItems
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
