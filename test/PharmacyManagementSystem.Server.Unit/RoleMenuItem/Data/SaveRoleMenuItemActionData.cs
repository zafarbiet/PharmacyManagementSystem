namespace PharmacyManagementSystem.Server.Unit.RoleMenuItem.Data;

public static class SaveRoleMenuItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var roleId = Guid.NewGuid();
        var menuItemId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.RoleMenuItem.RoleMenuItem { RoleId = roleId, MenuItemId = menuItemId },
            new Common.RoleMenuItem.RoleMenuItem { Id = Guid.NewGuid(), RoleId = roleId, MenuItemId = menuItemId, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.RoleMenuItem.RoleMenuItem { RoleId = Guid.Empty, MenuItemId = Guid.NewGuid() }
        };

        yield return new object[]
        {
            new Common.RoleMenuItem.RoleMenuItem { RoleId = Guid.NewGuid(), MenuItemId = Guid.Empty }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
