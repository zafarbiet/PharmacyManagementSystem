using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtReminder;

namespace PharmacyManagementSystem.Server.DebtReminder;

public class GetDebtReminderAction(ILogger<GetDebtReminderAction> logger, IDebtReminderRepository repository) : IGetDebtReminderAction
{
    private readonly ILogger<GetDebtReminderAction> _logger = logger;
    private readonly IDebtReminderRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DebtReminder.DebtReminder>?> GetByFilterCriteriaAsync(DebtReminderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting debt reminders by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} debt reminders.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtReminder.DebtReminder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting debt reminder by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved debt reminder with id: {Id}.", id);

        return result;
    }
}
