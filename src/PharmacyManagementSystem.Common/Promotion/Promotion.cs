namespace PharmacyManagementSystem.Common.Promotion;

public class Promotion : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset ValidTo { get; set; }
    public Guid? ApplicableDrugId { get; set; }
    public Guid? ApplicableCategoryId { get; set; }
}
