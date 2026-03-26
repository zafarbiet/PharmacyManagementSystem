namespace PharmacyManagementSystem.Common.Patient;

public class Patient : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? Age { get; set; }
    public string? Gstin { get; set; }
    public decimal CreditBalance { get; set; }
    public bool IsSubscriber { get; set; }
    public decimal CreditLimit { get; set; }
}
