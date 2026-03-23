using Dapper;
using PharmacyManagementSystem.Common.PaymentLedger;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PaymentLedger;

public static class PaymentLedgerDatabaseCommandText
{
    private const string SelectColumns = "Id, VendorId, PurchaseOrderId, InvoicedAmount, PaidAmount, DueDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(PaymentLedgerFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.PaymentLedger WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.VendorId.HasValue)
        {
            sql += " AND VendorId = @VendorId";
            parameters.Add("VendorId", filter.VendorId.Value);
        }

        if (filter.PurchaseOrderId.HasValue)
        {
            sql += " AND PurchaseOrderId = @PurchaseOrderId";
            parameters.Add("PurchaseOrderId", filter.PurchaseOrderId.Value);
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

        return Task.FromResult(new DatabaseSqlWithParameters { SqlStatement = sql, Parameters = parameters });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.PaymentLedger WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.PaymentLedger.PaymentLedger paymentLedger)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        var parameters = new DynamicParameters();
        parameters.Add("VendorId", paymentLedger.VendorId);
        parameters.Add("PurchaseOrderId", paymentLedger.PurchaseOrderId);
        parameters.Add("InvoicedAmount", paymentLedger.InvoicedAmount);
        parameters.Add("PaidAmount", paymentLedger.PaidAmount);
        parameters.Add("DueDate", paymentLedger.DueDate);
        parameters.Add("Status", paymentLedger.Status);
        parameters.Add("Notes", paymentLedger.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", paymentLedger.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.PaymentLedger (Id, VendorId, PurchaseOrderId, InvoicedAmount, PaidAmount, DueDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @VendorId, @PurchaseOrderId, @InvoicedAmount, @PaidAmount, @DueDate, @Status, @Notes, @UpdatedAt, @UpdatedBy, 1)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.PaymentLedger.PaymentLedger paymentLedger)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        var parameters = new DynamicParameters();
        parameters.Add("Id", paymentLedger.Id);
        parameters.Add("VendorId", paymentLedger.VendorId);
        parameters.Add("PurchaseOrderId", paymentLedger.PurchaseOrderId);
        parameters.Add("InvoicedAmount", paymentLedger.InvoicedAmount);
        parameters.Add("PaidAmount", paymentLedger.PaidAmount);
        parameters.Add("DueDate", paymentLedger.DueDate);
        parameters.Add("Status", paymentLedger.Status);
        parameters.Add("Notes", paymentLedger.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", paymentLedger.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.PaymentLedger
                             SET VendorId = @VendorId, PurchaseOrderId = @PurchaseOrderId,
                                 InvoicedAmount = @InvoicedAmount, PaidAmount = @PaidAmount,
                                 DueDate = @DueDate, Status = @Status, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.PaymentLedger SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
