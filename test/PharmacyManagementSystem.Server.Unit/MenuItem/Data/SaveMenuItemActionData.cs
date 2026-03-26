namespace PharmacyManagementSystem.Server.Unit.MenuItem.Data;

public static class SaveMenuItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.MenuItem.MenuItem { Key = "/reports", Label = "Reports", Icon = "BarChartOutlined", OrderIndex = 5 },
            new Common.MenuItem.MenuItem { Id = Guid.NewGuid(), Key = "/reports", Label = "Reports", Icon = "BarChartOutlined", OrderIndex = 5, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.MenuItem.MenuItem { Key = string.Empty, Label = "Reports" }
        };

        yield return new object[]
        {
            new Common.MenuItem.MenuItem { Key = "/reports", Label = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.MenuItem.MenuItem { Id = id, Key = "/reports", Label = "Reports Updated", OrderIndex = 5 },
            new Common.MenuItem.MenuItem { Id = id, Key = "/reports", Label = "Reports Updated", OrderIndex = 5, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
