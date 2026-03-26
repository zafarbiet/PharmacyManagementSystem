namespace PharmacyManagementSystem.Common.Manufacturer;

public class Manufacturer : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? Gstin { get; set; }
    public string? DrugLicenseNumber { get; set; }
}
