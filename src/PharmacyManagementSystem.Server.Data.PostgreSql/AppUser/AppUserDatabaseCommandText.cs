using Dapper;
using PharmacyManagementSystem.Common.AppUser;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.AppUser;

public static class AppUserDatabaseCommandText
{
    private const string SelectColumns = "Id, Username, FullName, Email, Phone, PasswordHash, IsLocked, LastLoginAt, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(AppUserFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Users WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            sql += " AND Username LIKE @Username";
            parameters.Add("Username", $"%{filter.Username}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            sql += " AND Email LIKE @Email";
            parameters.Add("Email", $"%{filter.Email}%");
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Users WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByUsernameSql(string username)
    {
        ArgumentNullException.ThrowIfNull(username);

        var parameters = new DynamicParameters();
        parameters.Add("Username", username);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Users WHERE Username = @Username AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.AppUser.AppUser appUser)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        var parameters = new DynamicParameters();
        parameters.Add("Username", appUser.Username);
        parameters.Add("FullName", appUser.FullName);
        parameters.Add("Email", appUser.Email);
        parameters.Add("Phone", appUser.Phone);
        parameters.Add("PasswordHash", appUser.PasswordHash);
        parameters.Add("IsLocked", appUser.IsLocked);
        parameters.Add("LastLoginAt", appUser.LastLoginAt);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", appUser.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Users (Id, Username, FullName, Email, Phone, PasswordHash, IsLocked, LastLoginAt, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @Username, @FullName, @Email, @Phone, @PasswordHash, @IsLocked, @LastLoginAt, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.AppUser.AppUser appUser)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        var parameters = new DynamicParameters();
        parameters.Add("Id", appUser.Id);
        parameters.Add("Username", appUser.Username);
        parameters.Add("FullName", appUser.FullName);
        parameters.Add("Email", appUser.Email);
        parameters.Add("Phone", appUser.Phone);
        parameters.Add("PasswordHash", appUser.PasswordHash);
        parameters.Add("IsLocked", appUser.IsLocked);
        parameters.Add("LastLoginAt", appUser.LastLoginAt);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", appUser.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Users
                             SET Username = @Username, FullName = @FullName, Email = @Email,
                                 Phone = @Phone, PasswordHash = @PasswordHash, IsLocked = @IsLocked,
                                 LastLoginAt = @LastLoginAt, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.Users
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
