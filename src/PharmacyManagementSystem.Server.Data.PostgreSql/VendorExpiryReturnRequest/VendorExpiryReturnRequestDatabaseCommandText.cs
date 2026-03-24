using Dapper;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.VendorExpiryReturnRequest;

public static class VendorExpiryReturnRequestDatabaseCommandText
{
    private const string SelectColumns = "Id, ExpiryRecordId, VendorId, RequestedAt, QuantityToReturn, Status, VendorResponseAt, VendorNotes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(VendorExpiryReturnRequestFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.VendorExpiryReturnRequests WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.ExpiryRecordId != Guid.Empty)
        {
            sql += " AND ExpiryRecordId = @ExpiryRecordId";
            parameters.Add("ExpiryRecordId", filter.ExpiryRecordId);
        }

        if (filter.VendorId != Guid.Empty)
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.VendorExpiryReturnRequests WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest vendorExpiryReturnRequest)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        var parameters = new DynamicParameters();
        parameters.Add("ExpiryRecordId", vendorExpiryReturnRequest.ExpiryRecordId);
        parameters.Add("VendorId", vendorExpiryReturnRequest.VendorId);
        parameters.Add("RequestedAt", vendorExpiryReturnRequest.RequestedAt);
        parameters.Add("QuantityToReturn", vendorExpiryReturnRequest.QuantityToReturn);
        parameters.Add("Status", vendorExpiryReturnRequest.Status);
        parameters.Add("VendorResponseAt", vendorExpiryReturnRequest.VendorResponseAt);
        parameters.Add("VendorNotes", vendorExpiryReturnRequest.VendorNotes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", vendorExpiryReturnRequest.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.VendorExpiryReturnRequests (Id, ExpiryRecordId, VendorId, RequestedAt, QuantityToReturn, Status, VendorResponseAt, VendorNotes, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @ExpiryRecordId, @VendorId, @RequestedAt, @QuantityToReturn, @Status, @VendorResponseAt, @VendorNotes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest vendorExpiryReturnRequest)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        var parameters = new DynamicParameters();
        parameters.Add("Id", vendorExpiryReturnRequest.Id);
        parameters.Add("ExpiryRecordId", vendorExpiryReturnRequest.ExpiryRecordId);
        parameters.Add("VendorId", vendorExpiryReturnRequest.VendorId);
        parameters.Add("RequestedAt", vendorExpiryReturnRequest.RequestedAt);
        parameters.Add("QuantityToReturn", vendorExpiryReturnRequest.QuantityToReturn);
        parameters.Add("Status", vendorExpiryReturnRequest.Status);
        parameters.Add("VendorResponseAt", vendorExpiryReturnRequest.VendorResponseAt);
        parameters.Add("VendorNotes", vendorExpiryReturnRequest.VendorNotes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", vendorExpiryReturnRequest.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.VendorExpiryReturnRequests
                             SET ExpiryRecordId = @ExpiryRecordId, VendorId = @VendorId, RequestedAt = @RequestedAt,
                                 QuantityToReturn = @QuantityToReturn, Status = @Status, VendorResponseAt = @VendorResponseAt,
                                 VendorNotes = @VendorNotes, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.VendorExpiryReturnRequests
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
