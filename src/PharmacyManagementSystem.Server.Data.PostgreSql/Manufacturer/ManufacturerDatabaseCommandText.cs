using Dapper;
using PharmacyManagementSystem.Common.Manufacturer;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Manufacturer;

public static class ManufacturerDatabaseCommandText
{
    private const string SelectColumns = "Id, Name, ContactEmail, ContactPhone, Country, Address, Gstin, DrugLicenseNumber, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(ManufacturerFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Manufacturers WHERE 1=1";
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

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            sql += " AND Country = @Country";
            parameters.Add("Country", filter.Country);
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

        return Task.FromResult(new DatabaseSqlWithParameters { SqlStatement = sql, Parameters = parameters });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Manufacturers WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Manufacturer.Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        var parameters = new DynamicParameters();
        parameters.Add("Name", manufacturer.Name);
        parameters.Add("ContactEmail", manufacturer.ContactEmail);
        parameters.Add("ContactPhone", manufacturer.ContactPhone);
        parameters.Add("Country", manufacturer.Country);
        parameters.Add("Address", manufacturer.Address);
        parameters.Add("Gstin", manufacturer.Gstin);
        parameters.Add("DrugLicenseNumber", manufacturer.DrugLicenseNumber);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", manufacturer.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Manufacturers (Id, Name, ContactEmail, ContactPhone, Country, Address, Gstin, DrugLicenseNumber, UpdatedAt, UpdatedBy, IsActive)
                             VALUES (gen_random_uuid(), @Name, @ContactEmail, @ContactPhone, @Country, @Address, @Gstin, @DrugLicenseNumber, @UpdatedAt, @UpdatedBy, true)
                             RETURNING *",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Manufacturer.Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        var parameters = new DynamicParameters();
        parameters.Add("Id", manufacturer.Id);
        parameters.Add("Name", manufacturer.Name);
        parameters.Add("ContactEmail", manufacturer.ContactEmail);
        parameters.Add("ContactPhone", manufacturer.ContactPhone);
        parameters.Add("Country", manufacturer.Country);
        parameters.Add("Address", manufacturer.Address);
        parameters.Add("Gstin", manufacturer.Gstin);
        parameters.Add("DrugLicenseNumber", manufacturer.DrugLicenseNumber);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", manufacturer.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Manufacturers
                             SET Name = @Name, ContactEmail = @ContactEmail, ContactPhone = @ContactPhone,
                                 Country = @Country, Address = @Address, Gstin = @Gstin,
                                 DrugLicenseNumber = @DrugLicenseNumber, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.Manufacturers SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             WHERE Id = @Id
                             RETURNING *",
            Parameters = parameters
        });
    }
}
