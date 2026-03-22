namespace PharmacyManagementSystem.Common.CustomerInvoice;

public class CustomerInvoiceFilter : FilterBase
{
    public Guid PatientId { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? InvoiceDateFrom { get; set; }
    public DateTimeOffset? InvoiceDateTo { get; set; }
}
