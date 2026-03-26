using System.Security.Cryptography;
using System.Text;
using PharmacyManagementSystem.Common.Auth;

namespace PharmacyManagementSystem.Server.Unit.Auth.Data;

public static class LoginActionData
{
    private static string ComputeHash(string username, string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{username}:{password}"));
        return Convert.ToHexString(bytes);
    }

    public static IEnumerable<object[]> ValidCredentialsData()
    {
        const string username = "admin";
        const string password = "Admin@123";
        var hash = ComputeHash(username, password);
        var userId = Guid.NewGuid();

        yield return new object[]
        {
            new LoginRequest { Username = username, Password = password },
            new Common.AppUser.AppUser
            {
                Id = userId,
                Username = username,
                FullName = "Admin User",
                PasswordHash = hash,
                IsActive = true,
                IsLocked = false
            }
        };
    }

    public static IEnumerable<object[]> InvalidCredentialsData()
    {
        const string username = "admin";
        const string correctPassword = "Admin@123";
        var hash = ComputeHash(username, correctPassword);
        var userId = Guid.NewGuid();

        yield return new object[]
        {
            new LoginRequest { Username = username, Password = "WrongPassword" },
            new Common.AppUser.AppUser
            {
                Id = userId,
                Username = username,
                PasswordHash = hash,
                IsActive = true,
                IsLocked = false
            }
        };
    }
}
