namespace PharmacyManagementSystem.Common.CustomerSubscription;

public class CustomerSubscription : BaseObject
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public int CycleDayOfMonth { get; set; }
    public string? Status { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? Notes { get; set; }
}
