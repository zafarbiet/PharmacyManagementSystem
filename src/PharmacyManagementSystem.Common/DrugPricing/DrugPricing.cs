namespace PharmacyManagementSystem.Common.DrugPricing;

public class DrugPricing : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal GstRate { get; set; }
    public DateTimeOffset EffectiveFrom { get; set; }
    public decimal Mrp { get; set; }
    public string? HsnCode { get; set; }
}
