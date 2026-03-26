using PharmacyManagementSystem.Common.CustomerSubscription;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscription.Data;

public static class GetCustomerSubscriptionActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var patientId = Guid.NewGuid();

        yield return new object[]
        {
            new CustomerSubscriptionFilter { PatientId = patientId },
            new List<Common.CustomerSubscription.CustomerSubscription>
            {
                new() { Id = Guid.NewGuid(), PatientId = patientId, CycleDayOfMonth = 15, Status = "Active", ApprovalStatus = "Approved", IsActive = true }
            }
        };

        yield return new object[]
        {
            new CustomerSubscriptionFilter(),
            new List<Common.CustomerSubscription.CustomerSubscription>
            {
                new() { Id = Guid.NewGuid(), PatientId = patientId, CycleDayOfMonth = 15, Status = "Active", ApprovalStatus = "Approved", IsActive = true },
                new() { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), CycleDayOfMonth = 1, Status = "Pending", ApprovalStatus = "Pending", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.CustomerSubscription.CustomerSubscription { Id = id, PatientId = Guid.NewGuid(), CycleDayOfMonth = 15, Status = "Active", ApprovalStatus = "Approved", IsActive = true }
        };
    }
}
