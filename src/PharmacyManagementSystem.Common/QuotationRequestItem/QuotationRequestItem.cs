namespace PharmacyManagementSystem.Common.QuotationRequestItem;

public class QuotationRequestItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid QuotationRequestId { get; set; }
    public Guid DrugId { get; set; }
    public int QuantityRequired { get; set; }
    public string? Notes { get; set; }
}
