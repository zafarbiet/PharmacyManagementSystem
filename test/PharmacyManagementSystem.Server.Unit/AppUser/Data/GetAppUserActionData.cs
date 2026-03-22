using PharmacyManagementSystem.Common.AppUser;

namespace PharmacyManagementSystem.Server.Unit.AppUser.Data;

public static class GetAppUserActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new AppUserFilter { Username = "johndoe" },
            new List<Common.AppUser.AppUser>
            {
                new() { Id = Guid.NewGuid(), Username = "johndoe", FullName = "John Doe", Email = "john@example.com", IsActive = true }
            }
        };

        yield return new object[]
        {
            new AppUserFilter(),
            new List<Common.AppUser.AppUser>
            {
                new() { Id = Guid.NewGuid(), Username = "johndoe", FullName = "John Doe", Email = "john@example.com", IsActive = true },
                new() { Id = Guid.NewGuid(), Username = "janedoe", FullName = "Jane Doe", Email = "jane@example.com", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.AppUser.AppUser { Id = id, Username = "johndoe", FullName = "John Doe", Email = "john@example.com", IsActive = true }
        };
    }
}
