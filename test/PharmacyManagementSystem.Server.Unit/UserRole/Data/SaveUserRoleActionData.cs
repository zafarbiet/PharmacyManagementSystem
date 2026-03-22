namespace PharmacyManagementSystem.Server.Unit.UserRole.Data;

public static class SaveUserRoleActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.UserRole.UserRole { UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow },
            new Common.UserRole.UserRole { Id = Guid.NewGuid(), UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.UserRole.UserRole { UserId = Guid.Empty, RoleId = Guid.NewGuid(), AssignedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.UserRole.UserRole { UserId = Guid.NewGuid(), RoleId = Guid.Empty, AssignedAt = DateTimeOffset.UtcNow }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.UserRole.UserRole { Id = id, UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow },
            new Common.UserRole.UserRole { Id = id, UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
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
