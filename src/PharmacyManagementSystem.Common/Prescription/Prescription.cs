namespace PharmacyManagementSystem.Common.Prescription;

public class Prescription : BaseObject
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? PrescribingDoctor { get; set; }
    public DateTimeOffset PrescriptionDate { get; set; }
    public string? Notes { get; set; }
}
