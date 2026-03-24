using Dapper;
using PharmacyManagementSystem.Common.StockTransaction;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.StockTransaction;

public static class StockTransactionDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugId, BatchNumber, TransactionType, Quantity, TransactionDate, ReferenceId, ReferenceType, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(StockTransactionFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.StockTransactions WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.DrugId != Guid.Empty)
        {
            sql += " AND DrugId = @DrugId";
            parameters.Add("DrugId", filter.DrugId);
        }

        if (!string.IsNullOrWhiteSpace(filter.TransactionType))
        {
            sql += " AND TransactionType = @TransactionType";
            parameters.Add("TransactionType", filter.TransactionType);
        }

        if (filter.TransactionDateFrom.HasValue)
        {
            sql += " AND TransactionDate >= @TransactionDateFrom";
            parameters.Add("TransactionDateFrom", filter.TransactionDateFrom.Value);
        }

        if (filter.TransactionDateTo.HasValue)
        {
            sql += " AND TransactionDate <= @TransactionDateTo";
            parameters.Add("TransactionDateTo", filter.TransactionDateTo.Value);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.StockTransactions WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.StockTransaction.StockTransaction stockTransaction)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        var parameters = new DynamicParameters();
        parameters.Add("DrugId", stockTransaction.DrugId);
        parameters.Add("BatchNumber", stockTransaction.BatchNumber);
        parameters.Add("TransactionType", stockTransaction.TransactionType);
        parameters.Add("Quantity", stockTransaction.Quantity);
        parameters.Add("TransactionDate", stockTransaction.TransactionDate);
        parameters.Add("ReferenceId", stockTransaction.ReferenceId);
        parameters.Add("ReferenceType", stockTransaction.ReferenceType);
        parameters.Add("Notes", stockTransaction.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", stockTransaction.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.StockTransactions (Id, DrugId, BatchNumber, TransactionType, Quantity, TransactionDate, ReferenceId, ReferenceType, Notes, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @DrugId, @BatchNumber, @TransactionType, @Quantity, @TransactionDate, @ReferenceId, @ReferenceType, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.StockTransaction.StockTransaction stockTransaction)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        var parameters = new DynamicParameters();
        parameters.Add("Id", stockTransaction.Id);
        parameters.Add("DrugId", stockTransaction.DrugId);
        parameters.Add("BatchNumber", stockTransaction.BatchNumber);
        parameters.Add("TransactionType", stockTransaction.TransactionType);
        parameters.Add("Quantity", stockTransaction.Quantity);
        parameters.Add("TransactionDate", stockTransaction.TransactionDate);
        parameters.Add("ReferenceId", stockTransaction.ReferenceId);
        parameters.Add("ReferenceType", stockTransaction.ReferenceType);
        parameters.Add("Notes", stockTransaction.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", stockTransaction.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.StockTransactions
                             SET DrugId = @DrugId, BatchNumber = @BatchNumber, TransactionType = @TransactionType,
                                 Quantity = @Quantity, TransactionDate = @TransactionDate, ReferenceId = @ReferenceId,
                                 ReferenceType = @ReferenceType, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.StockTransactions
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
