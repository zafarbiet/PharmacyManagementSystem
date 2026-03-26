namespace PharmacyManagementSystem.Common.Promotion;

public class PromotionFilter : FilterBase
{
    public string? Name { get; set; }
    public Guid? ApplicableDrugId { get; set; }
    public Guid? ApplicableCategoryId { get; set; }
    public bool? ActiveOnly { get; set; }
}
