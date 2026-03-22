namespace PharmacyManagementSystem.Common.DrugUsage;

public class DrugUsage : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public string? DosageInstructions { get; set; }
    public string? Indications { get; set; }
    public string? Contraindications { get; set; }
    public string? SideEffects { get; set; }
}
