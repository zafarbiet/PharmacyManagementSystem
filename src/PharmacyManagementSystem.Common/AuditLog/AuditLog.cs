namespace PharmacyManagementSystem.Common.AuditLog;

public class AuditLog : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public string? DrugName { get; set; }
    public string? ScheduleCategory { get; set; }
    public Guid CustomerInvoiceId { get; set; }
    public Guid? PrescriptionId { get; set; }
    public Guid? PatientId { get; set; }
    public int QuantityDispensed { get; set; }
    public string? PerformedBy { get; set; }
    public DateTimeOffset PerformedAt { get; set; }
    public DateTimeOffset RetentionUntil { get; set; }
}
