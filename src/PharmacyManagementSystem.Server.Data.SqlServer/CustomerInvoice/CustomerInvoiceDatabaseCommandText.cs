using Dapper;
using PharmacyManagementSystem.Common.CustomerInvoice;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoice;

public static class CustomerInvoiceDatabaseCommandText
{
    private const string SelectColumns = "Id, PatientId, InvoiceDate, SubTotal, DiscountAmount, GstAmount, NetAmount, PaymentMethod, Status, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(CustomerInvoiceFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.CustomerInvoices WHERE 1=1";
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

        if (filter.InvoiceDateFrom.HasValue)
        {
            sql += " AND InvoiceDate >= @InvoiceDateFrom";
            parameters.Add("InvoiceDateFrom", filter.InvoiceDateFrom.Value);
        }

        if (filter.InvoiceDateTo.HasValue)
        {
            sql += " AND InvoiceDate <= @InvoiceDateTo";
            parameters.Add("InvoiceDateTo", filter.InvoiceDateTo.Value);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.CustomerInvoices WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.CustomerInvoice.CustomerInvoice customerInvoice)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        var parameters = new DynamicParameters();
        parameters.Add("PatientId", customerInvoice.PatientId);
        parameters.Add("InvoiceDate", customerInvoice.InvoiceDate);
        parameters.Add("SubTotal", customerInvoice.SubTotal);
        parameters.Add("DiscountAmount", customerInvoice.DiscountAmount);
        parameters.Add("GstAmount", customerInvoice.GstAmount);
        parameters.Add("NetAmount", customerInvoice.NetAmount);
        parameters.Add("PaymentMethod", customerInvoice.PaymentMethod);
        parameters.Add("Status", customerInvoice.Status);
        parameters.Add("Notes", customerInvoice.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerInvoice.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.CustomerInvoices (Id, PatientId, InvoiceDate, SubTotal, DiscountAmount, GstAmount, NetAmount, PaymentMethod, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @PatientId, @InvoiceDate, @SubTotal, @DiscountAmount, @GstAmount, @NetAmount, @PaymentMethod, @Status, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.CustomerInvoice.CustomerInvoice customerInvoice)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        var parameters = new DynamicParameters();
        parameters.Add("Id", customerInvoice.Id);
        parameters.Add("PatientId", customerInvoice.PatientId);
        parameters.Add("InvoiceDate", customerInvoice.InvoiceDate);
        parameters.Add("SubTotal", customerInvoice.SubTotal);
        parameters.Add("DiscountAmount", customerInvoice.DiscountAmount);
        parameters.Add("GstAmount", customerInvoice.GstAmount);
        parameters.Add("NetAmount", customerInvoice.NetAmount);
        parameters.Add("PaymentMethod", customerInvoice.PaymentMethod);
        parameters.Add("Status", customerInvoice.Status);
        parameters.Add("Notes", customerInvoice.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", customerInvoice.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.CustomerInvoices
                             SET PatientId = @PatientId, InvoiceDate = @InvoiceDate, SubTotal = @SubTotal,
                                 DiscountAmount = @DiscountAmount, GstAmount = @GstAmount, NetAmount = @NetAmount,
                                 PaymentMethod = @PaymentMethod, Status = @Status, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.CustomerInvoices
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
