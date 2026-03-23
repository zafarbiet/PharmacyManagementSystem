namespace PharmacyManagementSystem.Common.AuditLog;

public class AuditLogFilter : FilterBase
{
    public Guid DrugId { get; set; }
    public Guid CustomerInvoiceId { get; set; }
    public Guid PatientId { get; set; }
    public string? ScheduleCategory { get; set; }
}
