using Dapper;
using PharmacyManagementSystem.Common.Quotation;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Quotation;

public static class QuotationDatabaseCommandText
{
    private const string SelectColumns = "Id, QuotationRequestId, VendorId, QuotationDate, ValidUntil, Status, TotalAmount, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(QuotationFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Quotations WHERE 1=1";
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

        if (filter.VendorId.HasValue && filter.VendorId.Value != Guid.Empty)
        {
            sql += " AND VendorId = @VendorId";
            parameters.Add("VendorId", filter.VendorId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Quotations WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Quotation.Quotation quotation)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        var parameters = new DynamicParameters();
        parameters.Add("QuotationRequestId", quotation.QuotationRequestId);
        parameters.Add("VendorId", quotation.VendorId);
        parameters.Add("QuotationDate", quotation.QuotationDate);
        parameters.Add("ValidUntil", quotation.ValidUntil);
        parameters.Add("Status", quotation.Status);
        parameters.Add("TotalAmount", quotation.TotalAmount);
        parameters.Add("Notes", quotation.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotation.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Quotations (Id, QuotationRequestId, VendorId, QuotationDate, ValidUntil, Status, TotalAmount, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @QuotationRequestId, @VendorId, @QuotationDate, @ValidUntil, @Status, @TotalAmount, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Quotation.Quotation quotation)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        var parameters = new DynamicParameters();
        parameters.Add("Id", quotation.Id);
        parameters.Add("QuotationRequestId", quotation.QuotationRequestId);
        parameters.Add("VendorId", quotation.VendorId);
        parameters.Add("QuotationDate", quotation.QuotationDate);
        parameters.Add("ValidUntil", quotation.ValidUntil);
        parameters.Add("Status", quotation.Status);
        parameters.Add("TotalAmount", quotation.TotalAmount);
        parameters.Add("Notes", quotation.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotation.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Quotations
                             SET QuotationRequestId = @QuotationRequestId, VendorId = @VendorId, QuotationDate = @QuotationDate,
                                 ValidUntil = @ValidUntil, Status = @Status, TotalAmount = @TotalAmount,
                                 Notes = @Notes, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.Quotations
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
