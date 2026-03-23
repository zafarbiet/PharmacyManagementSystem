namespace PharmacyManagementSystem.Common.CustomerSubscription;

public class CustomerSubscriptionFilter : FilterBase
{
    public Guid? PatientId { get; set; }
    public string? Status { get; set; }
    public string? ApprovalStatus { get; set; }
}
