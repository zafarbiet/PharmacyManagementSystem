namespace PharmacyManagementSystem.Common.Prescription;

public class Prescription : BaseObject
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? PrescribingDoctor { get; set; }
    public string? DoctorRegistrationNumber { get; set; }
    public DateTimeOffset PrescriptionDate { get; set; }
    public int? PatientAge { get; set; }
    public string? Notes { get; set; }
}
