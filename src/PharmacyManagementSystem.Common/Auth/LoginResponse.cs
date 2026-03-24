namespace PharmacyManagementSystem.Common.Auth;

public class LoginResponse
{
    public string? Token { get; set; }
    public Common.AppUser.AppUser? User { get; set; }
}
