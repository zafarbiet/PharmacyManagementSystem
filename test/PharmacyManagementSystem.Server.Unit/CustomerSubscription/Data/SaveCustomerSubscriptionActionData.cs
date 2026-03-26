namespace PharmacyManagementSystem.Server.Unit.CustomerSubscription.Data;

public static class SaveCustomerSubscriptionActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var patientId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { PatientId = patientId, CycleDayOfMonth = 15, Status = "Active" },
            new Common.CustomerSubscription.CustomerSubscription { Id = Guid.NewGuid(), PatientId = patientId, CycleDayOfMonth = 15, Status = "Active", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { PatientId = Guid.Empty, CycleDayOfMonth = 15, Status = "Active" }
        };

        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { PatientId = Guid.NewGuid(), CycleDayOfMonth = 0, Status = "Active" }
        };

        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { PatientId = Guid.NewGuid(), CycleDayOfMonth = 29, Status = "Active" }
        };

        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { PatientId = Guid.NewGuid(), CycleDayOfMonth = 15, Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.CustomerSubscription.CustomerSubscription { Id = id, PatientId = patientId, CycleDayOfMonth = 1, Status = "Paused" },
            new Common.CustomerSubscription.CustomerSubscription { Id = id, PatientId = patientId, CycleDayOfMonth = 1, Status = "Paused", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }

    public static IEnumerable<object[]> ValidApproveData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id,
            "manager",
            new Common.CustomerSubscription.CustomerSubscription { Id = id, PatientId = Guid.NewGuid(), CycleDayOfMonth = 15, Status = "Active", ApprovalStatus = "Pending", IsActive = true }
        };
    }
}
