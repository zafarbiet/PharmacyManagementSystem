namespace PharmacyManagementSystem.Common.CustomerInvoice;

public class CustomerInvoice : BaseObject
{
    public Guid Id { get; set; }
    public Guid? PatientId { get; set; }
    public Guid? PrescriptionId { get; set; }
    public DateTimeOffset InvoiceDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GstAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal TotalCgst { get; set; }
    public decimal TotalSgst { get; set; }
    public decimal TotalIgst { get; set; }
    public Guid? BranchId { get; set; }
    public string? BilledBy { get; set; }
    public string? PharmacyGstin { get; set; }
    public string? PatientGstin { get; set; }

    /// <summary>
    /// Transient — used when submitting an invoice with items in one call.
    /// Not persisted via SELECT; items are stored in CustomerInvoiceItems.
    /// </summary>
    public List<CustomerInvoiceItem.CustomerInvoiceItem> Items { get; set; } = [];
}
