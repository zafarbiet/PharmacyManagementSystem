namespace PharmacyManagementSystem.Common.CustomerInvoiceItem;

public class CustomerInvoiceItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid DrugId { get; set; }
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal GstRate { get; set; }
    public decimal Amount { get; set; }
}
