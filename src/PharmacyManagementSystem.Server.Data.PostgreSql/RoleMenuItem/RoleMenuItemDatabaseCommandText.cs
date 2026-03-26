using Dapper;
using PharmacyManagementSystem.Common.RoleMenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.RoleMenuItem;

public static class RoleMenuItemDatabaseCommandText
{
    private const string SelectColumns = "Id, RoleId, MenuItemId, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(RoleMenuItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.RoleMenuItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.RoleId.HasValue && filter.RoleId.Value != Guid.Empty)
        {
            sql += " AND RoleId = @RoleId";
            parameters.Add("RoleId", filter.RoleId);
        }

        if (filter.MenuItemId.HasValue && filter.MenuItemId.Value != Guid.Empty)
        {
            sql += " AND MenuItemId = @MenuItemId";
            parameters.Add("MenuItemId", filter.MenuItemId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.RoleMenuItems WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.RoleMenuItem.RoleMenuItem roleMenuItem)
    {
        ArgumentNullException.ThrowIfNull(roleMenuItem);

        var parameters = new DynamicParameters();
        parameters.Add("RoleId", roleMenuItem.RoleId);
        parameters.Add("MenuItemId", roleMenuItem.MenuItemId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", roleMenuItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.RoleMenuItems (Id, RoleId, MenuItemId, UpdatedAt, UpdatedBy, IsActive)
                             VALUES (gen_random_uuid(), @RoleId, @MenuItemId, @UpdatedAt, @UpdatedBy, true)
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
            SqlStatement = @"UPDATE PMS.RoleMenuItems SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             WHERE Id = @Id
                             RETURNING *",
            Parameters = parameters
        });
    }
}
