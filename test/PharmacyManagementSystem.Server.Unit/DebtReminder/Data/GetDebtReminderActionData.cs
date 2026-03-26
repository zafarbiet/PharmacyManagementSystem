using PharmacyManagementSystem.Common.DebtReminder;

namespace PharmacyManagementSystem.Server.Unit.DebtReminder.Data;

public static class GetDebtReminderActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var debtRecordId = Guid.NewGuid();

        yield return new object[]
        {
            new DebtReminderFilter { DebtRecordId = debtRecordId },
            new List<Common.DebtReminder.DebtReminder>
            {
                new() { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, Channel = "SMS", SentBy = "system", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DebtReminderFilter(),
            new List<Common.DebtReminder.DebtReminder>
            {
                new() { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, Channel = "SMS", SentBy = "system", IsActive = true },
                new() { Id = Guid.NewGuid(), DebtRecordId = Guid.NewGuid(), Channel = "Email", SentBy = "admin", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DebtReminder.DebtReminder { Id = id, DebtRecordId = Guid.NewGuid(), Channel = "SMS", SentBy = "system", IsActive = true }
        };
    }
}
