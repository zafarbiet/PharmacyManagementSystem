using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public class GetDailyDiaryEntryAction(ILogger<GetDailyDiaryEntryAction> logger, IDailyDiaryEntryRepository repository) : IGetDailyDiaryEntryAction
{
    private readonly ILogger<GetDailyDiaryEntryAction> _logger = logger;
    private readonly IDailyDiaryEntryRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting daily diary entries by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} daily diary entries.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting daily diary entry by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved daily diary entry with id: {Id}.", id);

        return result;
    }
}
