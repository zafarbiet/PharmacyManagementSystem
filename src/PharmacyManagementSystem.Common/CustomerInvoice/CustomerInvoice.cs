namespace PharmacyManagementSystem.Common.CustomerInvoice;

public class CustomerInvoice : BaseObject
{
    public Guid Id { get; set; }
    public Guid? PatientId { get; set; }
    public DateTimeOffset InvoiceDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GstAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
