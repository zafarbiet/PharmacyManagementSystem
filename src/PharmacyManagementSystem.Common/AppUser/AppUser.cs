namespace PharmacyManagementSystem.Common.AppUser;

public class AppUser : BaseObject
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsLocked { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
}
