namespace PharmacyManagementSystem.Common.ExpiryAlertConfiguration;

public class ExpiryAlertConfiguration : BaseObject
{
    public Guid Id { get; set; }
    public int ThresholdDays { get; set; }
    public string? AlertType { get; set; }
    public bool IsEnabled { get; set; }
}
