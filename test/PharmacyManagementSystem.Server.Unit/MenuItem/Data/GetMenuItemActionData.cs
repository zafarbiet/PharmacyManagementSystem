using PharmacyManagementSystem.Common.MenuItem;

namespace PharmacyManagementSystem.Server.Unit.MenuItem.Data;

public static class GetMenuItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new MenuItemFilter { Key = "/dashboard" },
            new List<Common.MenuItem.MenuItem>
            {
                new() { Id = Guid.NewGuid(), Key = "/dashboard", Label = "Dashboard", Icon = "DashboardOutlined", OrderIndex = 1, IsActive = true }
            }
        };

        yield return new object[]
        {
            new MenuItemFilter(),
            new List<Common.MenuItem.MenuItem>
            {
                new() { Id = Guid.NewGuid(), Key = "/dashboard", Label = "Dashboard", Icon = "DashboardOutlined", OrderIndex = 1, IsActive = true },
                new() { Id = Guid.NewGuid(), Key = "/front-desk", Label = "Front Desk", Icon = "ShoppingCartOutlined", OrderIndex = 2, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.MenuItem.MenuItem { Id = id, Key = "/dashboard", Label = "Dashboard", Icon = "DashboardOutlined", OrderIndex = 1, IsActive = true }
        };
    }

    public static IEnumerable<object[]> ValidUsernameData()
    {
        yield return new object[]
        {
            "admin",
            new List<Common.MenuItem.MenuItem>
            {
                new() { Id = Guid.NewGuid(), Key = "/dashboard", Label = "Dashboard", Icon = "DashboardOutlined", OrderIndex = 1, IsActive = true },
                new() { Id = Guid.NewGuid(), Key = "/inventory", Label = "Inventory", Icon = "InboxOutlined", OrderIndex = 2, IsActive = true }
            }
        };
    }
}
