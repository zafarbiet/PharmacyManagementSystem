namespace PharmacyManagementSystem.Common;

public class FilterBase
{
    public Guid Id { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
}
