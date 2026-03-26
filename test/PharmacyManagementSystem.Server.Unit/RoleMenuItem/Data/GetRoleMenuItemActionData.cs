using PharmacyManagementSystem.Common.RoleMenuItem;

namespace PharmacyManagementSystem.Server.Unit.RoleMenuItem.Data;

public static class GetRoleMenuItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var roleId = Guid.NewGuid();

        yield return new object[]
        {
            new RoleMenuItemFilter { RoleId = roleId },
            new List<Common.RoleMenuItem.RoleMenuItem>
            {
                new() { Id = Guid.NewGuid(), RoleId = roleId, MenuItemId = Guid.NewGuid(), IsActive = true },
                new() { Id = Guid.NewGuid(), RoleId = roleId, MenuItemId = Guid.NewGuid(), IsActive = true }
            }
        };

        yield return new object[]
        {
            new RoleMenuItemFilter(),
            new List<Common.RoleMenuItem.RoleMenuItem>
            {
                new() { Id = Guid.NewGuid(), RoleId = roleId, MenuItemId = Guid.NewGuid(), IsActive = true },
                new() { Id = Guid.NewGuid(), RoleId = Guid.NewGuid(), MenuItemId = Guid.NewGuid(), IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.RoleMenuItem.RoleMenuItem { Id = id, RoleId = Guid.NewGuid(), MenuItemId = Guid.NewGuid(), IsActive = true }
        };
    }
}
