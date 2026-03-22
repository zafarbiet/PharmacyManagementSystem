namespace PharmacyManagementSystem.Server.Unit.Role.Data;

public static class SaveRoleActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.Role.Role { Name = "Admin", Description = "Administrator role" },
            new Common.Role.Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator role", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Role.Role { Name = string.Empty, Description = "Some description" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Role.Role { Id = id, Name = "Updated Admin", Description = "Updated admin role" },
            new Common.Role.Role { Id = id, Name = "Updated Admin", Description = "Updated admin role", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            Guid.NewGuid(),
            "system"
        };
    }
}
