using Dapper;
using PharmacyManagementSystem.Common.Vendor;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Vendor;

public static class VendorDatabaseCommandText
{
    public static Task<DatabaseSqlWithParameters> GetSelectSql(VendorFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = "SELECT Id, Name, ContactPerson, Phone, Email, Address, GstNumber, UpdatedAt, UpdatedBy, IsActive FROM PMS.Vendors WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            sql += " AND Name LIKE @Name";
            parameters.Add("Name", $"%{filter.Name}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            sql += " AND Email LIKE @Email";
            parameters.Add("Email", $"%{filter.Email}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.GstNumber))
        {
            sql += " AND GstNumber = @GstNumber";
            parameters.Add("GstNumber", filter.GstNumber);
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
            SqlStatement = "SELECT Id, Name, ContactPerson, Phone, Email, Address, GstNumber, UpdatedAt, UpdatedBy, IsActive FROM PMS.Vendors WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Vendor.Vendor vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        var parameters = new DynamicParameters();
        parameters.Add("Name", vendor.Name);
        parameters.Add("ContactPerson", vendor.ContactPerson);
        parameters.Add("Phone", vendor.Phone);
        parameters.Add("Email", vendor.Email);
        parameters.Add("Address", vendor.Address);
        parameters.Add("GstNumber", vendor.GstNumber);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", vendor.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Vendors (Id, Name, ContactPerson, Phone, Email, Address, GstNumber, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @Name, @ContactPerson, @Phone, @Email, @Address, @GstNumber, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Vendor.Vendor vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        var parameters = new DynamicParameters();
        parameters.Add("Id", vendor.Id);
        parameters.Add("Name", vendor.Name);
        parameters.Add("ContactPerson", vendor.ContactPerson);
        parameters.Add("Phone", vendor.Phone);
        parameters.Add("Email", vendor.Email);
        parameters.Add("Address", vendor.Address);
        parameters.Add("GstNumber", vendor.GstNumber);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", vendor.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Vendors
                             SET Name = @Name, ContactPerson = @ContactPerson, Phone = @Phone,
                                 Email = @Email, Address = @Address, GstNumber = @GstNumber,
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
            SqlStatement = @"UPDATE PMS.Vendors
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
