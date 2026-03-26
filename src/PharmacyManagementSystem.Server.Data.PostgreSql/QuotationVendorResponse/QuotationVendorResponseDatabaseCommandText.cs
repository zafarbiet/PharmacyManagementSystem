using Dapper;
using PharmacyManagementSystem.Common.QuotationVendorResponse;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationVendorResponse;

public static class QuotationVendorResponseDatabaseCommandText
{
    private const string SelectColumns = "Id, QuotationRequestId, VendorId, Status, RespondedAt, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(QuotationVendorResponseFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.QuotationVendorResponses WHERE 1=1";
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

        sql += " AND IsActive = true";

        return Task.FromResult(new DatabaseSqlWithParameters { SqlStatement = sql, Parameters = parameters });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.QuotationVendorResponses WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.QuotationVendorResponse.QuotationVendorResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        var parameters = new DynamicParameters();
        parameters.Add("QuotationRequestId", response.QuotationRequestId);
        parameters.Add("VendorId", response.VendorId);
        parameters.Add("Status", response.Status);
        parameters.Add("RespondedAt", response.RespondedAt);
        parameters.Add("Notes", response.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", response.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.QuotationVendorResponses (Id, QuotationRequestId, VendorId, Status, RespondedAt, Notes, UpdatedAt, UpdatedBy, IsActive)
                             VALUES (gen_random_uuid(), @QuotationRequestId, @VendorId, @Status, @RespondedAt, @Notes, @UpdatedAt, @UpdatedBy, true)
                             RETURNING *",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.QuotationVendorResponse.QuotationVendorResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        var parameters = new DynamicParameters();
        parameters.Add("Id", response.Id);
        parameters.Add("QuotationRequestId", response.QuotationRequestId);
        parameters.Add("VendorId", response.VendorId);
        parameters.Add("Status", response.Status);
        parameters.Add("RespondedAt", response.RespondedAt);
        parameters.Add("Notes", response.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", response.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.QuotationVendorResponses
                             SET QuotationRequestId = @QuotationRequestId, VendorId = @VendorId,
                                 Status = @Status, RespondedAt = @RespondedAt, Notes = @Notes,
                                 UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             WHERE Id = @Id
                             RETURNING *",
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
            SqlStatement = @"UPDATE PMS.QuotationVendorResponses SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             WHERE Id = @Id
                             RETURNING *",
            Parameters = parameters
        });
    }
}
