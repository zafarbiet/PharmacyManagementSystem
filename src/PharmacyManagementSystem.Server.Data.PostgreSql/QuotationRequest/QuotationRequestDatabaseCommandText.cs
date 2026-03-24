using Dapper;
using PharmacyManagementSystem.Common.QuotationRequest;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequest;

public static class QuotationRequestDatabaseCommandText
{
    private const string SelectColumns = "Id, RequestDate, RequiredByDate, Status, Notes, RequestedBy, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(QuotationRequestFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.QuotationRequests WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.QuotationRequests WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.QuotationRequest.QuotationRequest quotationRequest)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        var parameters = new DynamicParameters();
        parameters.Add("RequestDate", quotationRequest.RequestDate);
        parameters.Add("RequiredByDate", quotationRequest.RequiredByDate);
        parameters.Add("Status", quotationRequest.Status);
        parameters.Add("Notes", quotationRequest.Notes);
        parameters.Add("RequestedBy", quotationRequest.RequestedBy);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationRequest.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.QuotationRequests (Id, RequestDate, RequiredByDate, Status, Notes, RequestedBy, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @RequestDate, @RequiredByDate, @Status, @Notes, @RequestedBy, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.QuotationRequest.QuotationRequest quotationRequest)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        var parameters = new DynamicParameters();
        parameters.Add("Id", quotationRequest.Id);
        parameters.Add("RequestDate", quotationRequest.RequestDate);
        parameters.Add("RequiredByDate", quotationRequest.RequiredByDate);
        parameters.Add("Status", quotationRequest.Status);
        parameters.Add("Notes", quotationRequest.Notes);
        parameters.Add("RequestedBy", quotationRequest.RequestedBy);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", quotationRequest.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.QuotationRequests
                             SET RequestDate = @RequestDate, RequiredByDate = @RequiredByDate, Status = @Status,
                                 Notes = @Notes, RequestedBy = @RequestedBy,
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
            SqlStatement = @"UPDATE PMS.QuotationRequests
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
