namespace PharmacyManagementSystem.Server.Unit.DebtReminder.Data;

public static class SaveDebtReminderActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var debtRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtReminder.DebtReminder { DebtRecordId = debtRecordId, Channel = "SMS", SentBy = "system" },
            new Common.DebtReminder.DebtReminder { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, Channel = "SMS", SentBy = "system", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DebtReminder.DebtReminder { DebtRecordId = Guid.Empty, Channel = "SMS", SentBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DebtReminder.DebtReminder { DebtRecordId = Guid.NewGuid(), Channel = string.Empty, SentBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DebtReminder.DebtReminder { DebtRecordId = Guid.NewGuid(), Channel = "SMS", SentBy = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var debtRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtReminder.DebtReminder { Id = id, DebtRecordId = debtRecordId, Channel = "Email", SentBy = "admin" },
            new Common.DebtReminder.DebtReminder { Id = id, DebtRecordId = debtRecordId, Channel = "Email", SentBy = "admin", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
