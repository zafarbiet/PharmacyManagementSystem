using Dapper;
using PharmacyManagementSystem.Common.MenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.MenuItem;

public static class MenuItemDatabaseCommandText
{
    private const string SelectColumns = "Id, Key, Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(MenuItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.MenuItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Key))
        {
            sql += " AND Key = @Key";
            parameters.Add("Key", filter.Key);
        }

        if (!string.IsNullOrWhiteSpace(filter.ParentKey))
        {
            sql += " AND ParentKey = @ParentKey";
            parameters.Add("ParentKey", filter.ParentKey);
        }

        sql += " AND IsActive = true ORDER BY OrderIndex";

        return Task.FromResult(new DatabaseSqlWithParameters { SqlStatement = sql, Parameters = parameters });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.MenuItems WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetForUserSql(string username)
    {
        ArgumentNullException.ThrowIfNull(username);

        var parameters = new DynamicParameters();
        parameters.Add("Username", username);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $@"SELECT {SelectColumns} FROM PMS.MenuItems mi
            WHERE mi.IsActive = true
              AND (
                NOT EXISTS (SELECT 1 FROM PMS.RoleMenuItems WHERE IsActive = true)
                OR EXISTS (
                  SELECT 1 FROM PMS.RoleMenuItems rmi
                  INNER JOIN PMS.UserRoles ur ON ur.RoleId = rmi.RoleId AND ur.IsActive = true
                  INNER JOIN PMS.Users au ON au.Id = ur.UserId AND au.IsActive = true
                  WHERE rmi.MenuItemId = mi.Id AND rmi.IsActive = true AND au.Username = @Username
                )
              )
            ORDER BY mi.OrderIndex",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.MenuItem.MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        var parameters = new DynamicParameters();
        parameters.Add("Key", menuItem.Key);
        parameters.Add("Label", menuItem.Label);
        parameters.Add("Icon", menuItem.Icon);
        parameters.Add("ParentKey", menuItem.ParentKey);
        parameters.Add("OrderIndex", menuItem.OrderIndex);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", menuItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.MenuItems (Id, Key, Label, Icon, ParentKey, OrderIndex, UpdatedAt, UpdatedBy, IsActive)
                             VALUES (gen_random_uuid(), @Key, @Label, @Icon, @ParentKey, @OrderIndex, @UpdatedAt, @UpdatedBy, true)
                             RETURNING *",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.MenuItem.MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", menuItem.Id);
        parameters.Add("Key", menuItem.Key);
        parameters.Add("Label", menuItem.Label);
        parameters.Add("Icon", menuItem.Icon);
        parameters.Add("ParentKey", menuItem.ParentKey);
        parameters.Add("OrderIndex", menuItem.OrderIndex);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", menuItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.MenuItems
                             SET Key = @Key, Label = @Label, Icon = @Icon, ParentKey = @ParentKey,
                                 OrderIndex = @OrderIndex, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
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
            SqlStatement = @"UPDATE PMS.MenuItems SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             WHERE Id = @Id
                             RETURNING *",
            Parameters = parameters
        });
    }
}
