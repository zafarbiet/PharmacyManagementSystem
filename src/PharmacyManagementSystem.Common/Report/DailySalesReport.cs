namespace PharmacyManagementSystem.Common.Report;

public class DailySalesReport
{
    public DateOnly Date { get; set; }
    public int InvoiceCount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalCgst { get; set; }
    public decimal TotalSgst { get; set; }
    public decimal TotalIgst { get; set; }
    public decimal GstAmount { get; set; }
    public decimal NetAmount { get; set; }
}
