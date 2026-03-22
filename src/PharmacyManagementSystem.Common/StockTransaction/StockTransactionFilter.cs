namespace PharmacyManagementSystem.Common.StockTransaction;

public class StockTransactionFilter : FilterBase
{
    public Guid DrugId { get; set; }
    public string? TransactionType { get; set; }
    public DateTimeOffset? TransactionDateFrom { get; set; }
    public DateTimeOffset? TransactionDateTo { get; set; }
}
