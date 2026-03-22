namespace PharmacyManagementSystem.Common.QuotationItem;

public class QuotationItemFilter : FilterBase
{
    public Guid QuotationId { get; set; }
    public Guid DrugId { get; set; }
}
