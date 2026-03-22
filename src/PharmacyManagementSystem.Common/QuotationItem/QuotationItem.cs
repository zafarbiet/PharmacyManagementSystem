namespace PharmacyManagementSystem.Common.QuotationItem;

public class QuotationItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid QuotationId { get; set; }
    public Guid DrugId { get; set; }
    public int QuantityOffered { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal GstRate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
}
