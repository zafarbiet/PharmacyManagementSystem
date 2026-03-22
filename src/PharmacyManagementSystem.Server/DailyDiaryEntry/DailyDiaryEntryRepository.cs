using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public class DailyDiaryEntryRepository(ILogger<DailyDiaryEntryRepository> logger, IDailyDiaryEntryStorageClient storageClient) : IDailyDiaryEntryRepository
{
    private readonly ILogger<DailyDiaryEntryRepository> _logger = logger;
    private readonly IDailyDiaryEntryStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting daily diary entries by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} daily diary entries.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting daily diary entry by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved daily diary entry with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        _logger.LogDebug("Repository: Adding daily diary entry.");

        var result = await _storageClient.AddAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added daily diary entry.");

        return result;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        _logger.LogDebug("Repository: Updating daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        var result = await _storageClient.UpdateAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing daily diary entry with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed daily diary entry with id: {Id}.", id);
    }
}
