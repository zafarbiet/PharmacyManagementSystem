namespace PharmacyManagementSystem.Common.StockTransaction;

public class StockTransaction : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public string? BatchNumber { get; set; }
    public string? TransactionType { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public Guid? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public string? Notes { get; set; }
}
