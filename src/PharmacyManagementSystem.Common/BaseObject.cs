namespace PharmacyManagementSystem.Common;

public class BaseObject
{
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; }
}
