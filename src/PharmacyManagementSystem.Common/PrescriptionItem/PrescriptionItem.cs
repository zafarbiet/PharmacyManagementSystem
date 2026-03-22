namespace PharmacyManagementSystem.Common.PrescriptionItem;

public class PrescriptionItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid PrescriptionId { get; set; }
    public Guid DrugId { get; set; }
    public string? Dosage { get; set; }
    public int Quantity { get; set; }
    public string? Instructions { get; set; }
}
