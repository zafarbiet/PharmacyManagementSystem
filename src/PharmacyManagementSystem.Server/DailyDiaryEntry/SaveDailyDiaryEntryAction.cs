using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DailyDiaryEntry;

public class SaveDailyDiaryEntryAction(ILogger<SaveDailyDiaryEntryAction> logger, IDailyDiaryEntryRepository repository) : ISaveDailyDiaryEntryAction
{
    private readonly ILogger<SaveDailyDiaryEntryAction> _logger = logger;
    private readonly IDailyDiaryEntryRepository _repository = repository;

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Category))
            throw new BadRequestException("DailyDiaryEntry Category is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Title))
            throw new BadRequestException("DailyDiaryEntry Title is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Body))
            throw new BadRequestException("DailyDiaryEntry Body is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.CreatedBy))
            throw new BadRequestException("DailyDiaryEntry CreatedBy is required.");

        dailyDiaryEntry.UpdatedBy = "system";

        _logger.LogDebug("Adding new daily diary entry.");

        var result = await _repository.AddAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added daily diary entry.");

        return result;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Category))
            throw new BadRequestException("DailyDiaryEntry Category is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Title))
            throw new BadRequestException("DailyDiaryEntry Title is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.Body))
            throw new BadRequestException("DailyDiaryEntry Body is required.");

        if (string.IsNullOrWhiteSpace(dailyDiaryEntry.CreatedBy))
            throw new BadRequestException("DailyDiaryEntry CreatedBy is required.");

        dailyDiaryEntry.UpdatedBy = "system";

        _logger.LogDebug("Updating daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        var result = await _repository.UpdateAsync(dailyDiaryEntry, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing daily diary entry with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed daily diary entry with id: {Id}.", id);
    }
}
