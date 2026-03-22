using PharmacyManagementSystem.Common.UserRole;

namespace PharmacyManagementSystem.Server.Unit.UserRole.Data;

public static class GetUserRoleActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        yield return new object[]
        {
            new UserRoleFilter { UserId = userId },
            new List<Common.UserRole.UserRole>
            {
                new() { Id = Guid.NewGuid(), UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };

        yield return new object[]
        {
            new UserRoleFilter(),
            new List<Common.UserRole.UserRole>
            {
                new() { Id = Guid.NewGuid(), UserId = userId, RoleId = roleId, AssignedAt = DateTimeOffset.UtcNow, IsActive = true },
                new() { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), RoleId = Guid.NewGuid(), AssignedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.UserRole.UserRole { Id = id, UserId = Guid.NewGuid(), RoleId = Guid.NewGuid(), AssignedAt = DateTimeOffset.UtcNow, IsActive = true }
        };
    }
}
