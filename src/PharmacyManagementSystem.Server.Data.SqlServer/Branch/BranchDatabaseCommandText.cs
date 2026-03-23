using Dapper;
using PharmacyManagementSystem.Common.Branch;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Branch;

public static class BranchDatabaseCommandText
{
    private const string SelectColumns = "Id, Name, Address, Gstin, PharmacyLicenseNumber, Phone, Email, ManagerUserId, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(BranchFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Branches WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            sql += " AND Name LIKE @Name";
            parameters.Add("Name", $"%{filter.Name}%");
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Branches WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Branch.Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);

        var parameters = new DynamicParameters();
        parameters.Add("Name", branch.Name);
        parameters.Add("Address", branch.Address);
        parameters.Add("Gstin", branch.Gstin);
        parameters.Add("PharmacyLicenseNumber", branch.PharmacyLicenseNumber);
        parameters.Add("Phone", branch.Phone);
        parameters.Add("Email", branch.Email);
        parameters.Add("ManagerUserId", branch.ManagerUserId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", branch.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Branches (Id, Name, Address, Gstin, PharmacyLicenseNumber, Phone, Email, ManagerUserId, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @Name, @Address, @Gstin, @PharmacyLicenseNumber, @Phone, @Email, @ManagerUserId, @UpdatedAt, @UpdatedBy, 1)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Branch.Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);

        var parameters = new DynamicParameters();
        parameters.Add("Id", branch.Id);
        parameters.Add("Name", branch.Name);
        parameters.Add("Address", branch.Address);
        parameters.Add("Gstin", branch.Gstin);
        parameters.Add("PharmacyLicenseNumber", branch.PharmacyLicenseNumber);
        parameters.Add("Phone", branch.Phone);
        parameters.Add("Email", branch.Email);
        parameters.Add("ManagerUserId", branch.ManagerUserId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", branch.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Branches
                             SET Name = @Name, Address = @Address, Gstin = @Gstin, PharmacyLicenseNumber = @PharmacyLicenseNumber,
                                 Phone = @Phone, Email = @Email, ManagerUserId = @ManagerUserId,
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
            SqlStatement = @"UPDATE PMS.Branches SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
