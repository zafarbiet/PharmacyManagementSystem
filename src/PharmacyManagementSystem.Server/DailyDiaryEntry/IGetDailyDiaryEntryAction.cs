using PharmacyManagementSystem.Common.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public interface IGetDailyDiaryEntryAction
{
    Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken);
    Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
