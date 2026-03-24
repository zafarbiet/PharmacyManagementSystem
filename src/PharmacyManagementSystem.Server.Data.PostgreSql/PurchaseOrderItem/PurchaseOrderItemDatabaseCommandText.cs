using Dapper;
using PharmacyManagementSystem.Common.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.PurchaseOrderItem;

public static class PurchaseOrderItemDatabaseCommandText
{
    private const string SelectColumns = "Id, PurchaseOrderId, DrugId, QuantityOrdered, QuantityReceived, UnitPrice, BatchNumber, ExpirationDate, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(PurchaseOrderItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.PurchaseOrderItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.PurchaseOrderId != Guid.Empty)
        {
            sql += " AND PurchaseOrderId = @PurchaseOrderId";
            parameters.Add("PurchaseOrderId", filter.PurchaseOrderId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.PurchaseOrderItems WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.PurchaseOrderItem.PurchaseOrderItem purchaseOrderItem)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        var parameters = new DynamicParameters();
        parameters.Add("PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
        parameters.Add("DrugId", purchaseOrderItem.DrugId);
        parameters.Add("QuantityOrdered", purchaseOrderItem.QuantityOrdered);
        parameters.Add("QuantityReceived", purchaseOrderItem.QuantityReceived);
        parameters.Add("UnitPrice", purchaseOrderItem.UnitPrice);
        parameters.Add("BatchNumber", purchaseOrderItem.BatchNumber);
        parameters.Add("ExpirationDate", purchaseOrderItem.ExpirationDate);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", purchaseOrderItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.PurchaseOrderItems (Id, PurchaseOrderId, DrugId, QuantityOrdered, QuantityReceived, UnitPrice, BatchNumber, ExpirationDate, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @PurchaseOrderId, @DrugId, @QuantityOrdered, @QuantityReceived, @UnitPrice, @BatchNumber, @ExpirationDate, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.PurchaseOrderItem.PurchaseOrderItem purchaseOrderItem)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", purchaseOrderItem.Id);
        parameters.Add("PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
        parameters.Add("DrugId", purchaseOrderItem.DrugId);
        parameters.Add("QuantityOrdered", purchaseOrderItem.QuantityOrdered);
        parameters.Add("QuantityReceived", purchaseOrderItem.QuantityReceived);
        parameters.Add("UnitPrice", purchaseOrderItem.UnitPrice);
        parameters.Add("BatchNumber", purchaseOrderItem.BatchNumber);
        parameters.Add("ExpirationDate", purchaseOrderItem.ExpirationDate);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", purchaseOrderItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.PurchaseOrderItems
                             SET PurchaseOrderId = @PurchaseOrderId, DrugId = @DrugId, QuantityOrdered = @QuantityOrdered,
                                 QuantityReceived = @QuantityReceived, UnitPrice = @UnitPrice, BatchNumber = @BatchNumber,
                                 ExpirationDate = @ExpirationDate, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.PurchaseOrderItems
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
