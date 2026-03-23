namespace PharmacyManagementSystem.Common.Drug;

public class Drug : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? GenericName { get; set; }
    public string? ManufacturerName { get; set; }
    public Guid CategoryId { get; set; }
    public string? UnitOfMeasure { get; set; }
    public int ReorderLevel { get; set; }
    public string? BrandName { get; set; }
    public string? DosageForm { get; set; }
    public string? Strength { get; set; }
    public string? Description { get; set; }
    public string? DrugLicenseNumber { get; set; }
    public DateTimeOffset? ApprovalDate { get; set; }
    public string? ScheduleCategory { get; set; }
    public bool PrescriptionRequired { get; set; }
    public string? HsnCode { get; set; }
    public decimal GstSlab { get; set; }
    public string? Composition { get; set; }
    public decimal Mrp { get; set; }
}
