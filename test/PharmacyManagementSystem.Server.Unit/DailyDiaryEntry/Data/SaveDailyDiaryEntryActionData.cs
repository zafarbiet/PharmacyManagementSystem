namespace PharmacyManagementSystem.Server.Unit.DailyDiaryEntry.Data;

public static class SaveDailyDiaryEntryActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Category = "Stock", Title = "Morning check", Body = "All shelves checked.", CreatedBy = "admin" },
            new Common.DailyDiaryEntry.DailyDiaryEntry { Id = Guid.NewGuid(), Category = "Stock", Title = "Morning check", Body = "All shelves checked.", CreatedBy = "admin", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Category = string.Empty, Title = "Title", Body = "Body", CreatedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Category = "Stock", Title = string.Empty, Body = "Body", CreatedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Category = "Stock", Title = "Title", Body = string.Empty, CreatedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Category = "Stock", Title = "Title", Body = "Body", CreatedBy = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DailyDiaryEntry.DailyDiaryEntry { Id = id, Category = "Incident", Title = "Updated", Body = "Updated body.", CreatedBy = "admin" },
            new Common.DailyDiaryEntry.DailyDiaryEntry { Id = id, Category = "Incident", Title = "Updated", Body = "Updated body.", CreatedBy = "admin", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
