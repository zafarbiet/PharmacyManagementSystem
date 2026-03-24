namespace PharmacyManagementSystem.Common.Report;

public class StockValuationItem
{
    public Guid DrugId { get; set; }
    public string? DrugName { get; set; }
    public string? HsnCode { get; set; }
    public int TotalQuantity { get; set; }
    public decimal Mrp { get; set; }
    public decimal TotalMrpValue { get; set; }
    public decimal? AverageCostPrice { get; set; }
    public decimal? TotalCostValue { get; set; }
}
