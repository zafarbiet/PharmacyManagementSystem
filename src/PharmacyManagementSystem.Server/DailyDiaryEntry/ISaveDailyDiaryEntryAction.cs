namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public interface ISaveDailyDiaryEntryAction
{
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken);
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
