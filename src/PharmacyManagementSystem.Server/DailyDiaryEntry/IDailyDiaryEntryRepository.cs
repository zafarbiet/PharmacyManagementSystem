using PharmacyManagementSystem.Common.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public interface IDailyDiaryEntryRepository
{
    Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken);
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken);
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
