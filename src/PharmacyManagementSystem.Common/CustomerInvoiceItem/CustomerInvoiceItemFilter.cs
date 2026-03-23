namespace PharmacyManagementSystem.Common.CustomerInvoiceItem;

public class CustomerInvoiceItemFilter : FilterBase
{
    public Guid? InvoiceId { get; set; }
    public Guid? DrugId { get; set; }
}
