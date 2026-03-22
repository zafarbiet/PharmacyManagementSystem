using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtReminder;

namespace PharmacyManagementSystem.Server.DebtReminder;

public class DebtReminderRepository(ILogger<DebtReminderRepository> logger, IDebtReminderStorageClient storageClient) : IDebtReminderRepository
{
    private readonly ILogger<DebtReminderRepository> _logger = logger;
    private readonly IDebtReminderStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DebtReminder.DebtReminder>?> GetByFilterCriteriaAsync(DebtReminderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting debt reminders by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} debt reminders.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtReminder.DebtReminder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting debt reminder by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved debt reminder with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DebtReminder.DebtReminder?> AddAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        _logger.LogDebug("Repository: Adding debt reminder for debt record id: {DebtRecordId}.", debtReminder.DebtRecordId);

        var result = await _storageClient.AddAsync(debtReminder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added debt reminder for debt record id: {DebtRecordId}.", debtReminder.DebtRecordId);

        return result;
    }

    public async Task<Common.DebtReminder.DebtReminder?> UpdateAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        _logger.LogDebug("Repository: Updating debt reminder with id: {Id}.", debtReminder.Id);

        var result = await _storageClient.UpdateAsync(debtReminder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated debt reminder with id: {Id}.", debtReminder.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing debt reminder with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed debt reminder with id: {Id}.", id);
    }
}
