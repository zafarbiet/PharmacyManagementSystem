namespace PharmacyManagementSystem.Common.PrescriptionItem;

public class PrescriptionItemFilter : FilterBase
{
    public Guid? PrescriptionId { get; set; }
    public Guid? DrugId { get; set; }
}
