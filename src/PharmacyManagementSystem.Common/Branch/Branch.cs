namespace PharmacyManagementSystem.Common.Branch;

public class Branch : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Gstin { get; set; }
    public string? PharmacyLicenseNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? ManagerUserId { get; set; }
}
