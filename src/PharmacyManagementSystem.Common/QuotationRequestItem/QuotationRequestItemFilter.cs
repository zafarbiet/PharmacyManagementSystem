namespace PharmacyManagementSystem.Common.QuotationRequestItem;

public class QuotationRequestItemFilter : FilterBase
{
    public Guid QuotationRequestId { get; set; }
    public Guid DrugId { get; set; }
}
