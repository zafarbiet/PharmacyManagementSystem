namespace PharmacyManagementSystem.Common.Report;

public class ProfitMarginItem
{
    public Guid DrugId { get; set; }
    public string? DrugName { get; set; }
    public string? HsnCode { get; set; }
    public int TotalQtySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal MarginPct { get; set; }
    public decimal Mrp { get; set; }
    public decimal? AverageCostPrice { get; set; }
    /// <summary>MRP-based margin: (MRP - AverageCostPrice) / MRP * 100</summary>
    public decimal? MrpMarginPct { get; set; }
}
