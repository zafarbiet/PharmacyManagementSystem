namespace PharmacyManagementSystem.Common.DebtPayment;

public class DebtPayment : BaseObject
{
    public Guid Id { get; set; }
    public Guid DebtRecordId { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ReceivedBy { get; set; }
    public string? Notes { get; set; }
}
