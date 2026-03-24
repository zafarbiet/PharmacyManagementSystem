using Dapper;
using PharmacyManagementSystem.Common.DebtPayment;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DebtPayment;

public static class DebtPaymentDatabaseCommandText
{
    private const string SelectColumns = "Id, DebtRecordId, PaymentDate, AmountPaid, PaymentMethod, ReceivedBy, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DebtPaymentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DebtPayments WHERE 1=1";
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DebtPayments WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DebtPayment.DebtPayment debtPayment)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        var parameters = new DynamicParameters();
        parameters.Add("DebtRecordId", debtPayment.DebtRecordId);
        parameters.Add("PaymentDate", debtPayment.PaymentDate);
        parameters.Add("AmountPaid", debtPayment.AmountPaid);
        parameters.Add("PaymentMethod", debtPayment.PaymentMethod);
        parameters.Add("ReceivedBy", debtPayment.ReceivedBy);
        parameters.Add("Notes", debtPayment.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtPayment.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DebtPayments (Id, DebtRecordId, PaymentDate, AmountPaid, PaymentMethod, ReceivedBy, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @DebtRecordId, @PaymentDate, @AmountPaid, @PaymentMethod, @ReceivedBy, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DebtPayment.DebtPayment debtPayment)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        var parameters = new DynamicParameters();
        parameters.Add("Id", debtPayment.Id);
        parameters.Add("DebtRecordId", debtPayment.DebtRecordId);
        parameters.Add("PaymentDate", debtPayment.PaymentDate);
        parameters.Add("AmountPaid", debtPayment.AmountPaid);
        parameters.Add("PaymentMethod", debtPayment.PaymentMethod);
        parameters.Add("ReceivedBy", debtPayment.ReceivedBy);
        parameters.Add("Notes", debtPayment.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", debtPayment.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DebtPayments
                             SET DebtRecordId = @DebtRecordId, PaymentDate = @PaymentDate, AmountPaid = @AmountPaid,
                                 PaymentMethod = @PaymentMethod, ReceivedBy = @ReceivedBy, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.DebtPayments
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
