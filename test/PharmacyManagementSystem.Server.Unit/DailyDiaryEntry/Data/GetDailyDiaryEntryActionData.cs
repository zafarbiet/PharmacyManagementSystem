using PharmacyManagementSystem.Common.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.Unit.DailyDiaryEntry.Data;

public static class GetDailyDiaryEntryActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new DailyDiaryEntryFilter { Category = "Stock" },
            new List<Common.DailyDiaryEntry.DailyDiaryEntry>
            {
                new() { Id = Guid.NewGuid(), Category = "Stock", Title = "Stock check", Body = "Checked all shelves", CreatedBy = "admin", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DailyDiaryEntryFilter(),
            new List<Common.DailyDiaryEntry.DailyDiaryEntry>
            {
                new() { Id = Guid.NewGuid(), Category = "Stock", Title = "Stock check", Body = "All good", CreatedBy = "admin", IsActive = true },
                new() { Id = Guid.NewGuid(), Category = "Incident", Title = "Expiry found", Body = "Removed expired batch", CreatedBy = "admin", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DailyDiaryEntry.DailyDiaryEntry { Id = id, Category = "Stock", Title = "Stock check", Body = "All good", CreatedBy = "admin", IsActive = true }
        };
    }
}
