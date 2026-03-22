namespace PharmacyManagementSystem.Server.Unit.AppUser.Data;

public static class SaveAppUserActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.AppUser.AppUser { Username = "johndoe", FullName = "John Doe", PasswordHash = "hashedpassword" },
            new Common.AppUser.AppUser { Id = Guid.NewGuid(), Username = "johndoe", FullName = "John Doe", PasswordHash = "hashedpassword", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.AppUser.AppUser { Username = string.Empty, FullName = "John Doe", PasswordHash = "hashedpassword" }
        };

        yield return new object[]
        {
            new Common.AppUser.AppUser { Username = "johndoe", FullName = string.Empty, PasswordHash = "hashedpassword" }
        };

        yield return new object[]
        {
            new Common.AppUser.AppUser { Username = "johndoe", FullName = "John Doe", PasswordHash = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.AppUser.AppUser { Id = id, Username = "johndoe_updated", FullName = "John Doe Updated", PasswordHash = "newhashedpassword" },
            new Common.AppUser.AppUser { Id = id, Username = "johndoe_updated", FullName = "John Doe Updated", PasswordHash = "newhashedpassword", IsActive = true, UpdatedBy = "system" }
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
