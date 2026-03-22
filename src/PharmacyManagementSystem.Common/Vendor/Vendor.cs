namespace PharmacyManagementSystem.Common.Vendor;

public class Vendor : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? GstNumber { get; set; }
}
