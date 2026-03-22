using PharmacyManagementSystem.Common.Role;

namespace PharmacyManagementSystem.Server.Unit.Role.Data;

public static class GetRoleActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new RoleFilter { Name = "Admin" },
            new List<Common.Role.Role>
            {
                new() { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator role", IsActive = true }
            }
        };

        yield return new object[]
        {
            new RoleFilter(),
            new List<Common.Role.Role>
            {
                new() { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator role", IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "User", Description = "Regular user role", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Role.Role { Id = id, Name = "Admin", Description = "Administrator role", IsActive = true }
        };
    }
}
