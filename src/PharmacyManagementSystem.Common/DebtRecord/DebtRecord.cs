namespace PharmacyManagementSystem.Common.DebtRecord;

public class DebtRecord : BaseObject
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
