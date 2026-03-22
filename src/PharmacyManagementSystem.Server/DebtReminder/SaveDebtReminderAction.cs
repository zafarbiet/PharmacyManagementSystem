using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DebtReminder;

public class SaveDebtReminderAction(ILogger<SaveDebtReminderAction> logger, IDebtReminderRepository repository) : ISaveDebtReminderAction
{
    private readonly ILogger<SaveDebtReminderAction> _logger = logger;
    private readonly IDebtReminderRepository _repository = repository;

    public async Task<Common.DebtReminder.DebtReminder?> AddAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        if (debtReminder.DebtRecordId == Guid.Empty)
            throw new BadRequestException("DebtReminder DebtRecordId is required.");

        if (string.IsNullOrWhiteSpace(debtReminder.Channel))
            throw new BadRequestException("DebtReminder Channel is required.");

        if (string.IsNullOrWhiteSpace(debtReminder.SentBy))
            throw new BadRequestException("DebtReminder SentBy is required.");

        debtReminder.UpdatedBy = "system";

        _logger.LogDebug("Adding new debt reminder for debt record id: {DebtRecordId}.", debtReminder.DebtRecordId);

        var result = await _repository.AddAsync(debtReminder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added debt reminder for debt record id: {DebtRecordId}.", debtReminder.DebtRecordId);

        return result;
    }

    public async Task<Common.DebtReminder.DebtReminder?> UpdateAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        if (debtReminder.DebtRecordId == Guid.Empty)
            throw new BadRequestException("DebtReminder DebtRecordId is required.");

        if (string.IsNullOrWhiteSpace(debtReminder.Channel))
            throw new BadRequestException("DebtReminder Channel is required.");

        if (string.IsNullOrWhiteSpace(debtReminder.SentBy))
            throw new BadRequestException("DebtReminder SentBy is required.");

        debtReminder.UpdatedBy = "system";

        _logger.LogDebug("Updating debt reminder with id: {Id}.", debtReminder.Id);

        var result = await _repository.UpdateAsync(debtReminder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated debt reminder with id: {Id}.", debtReminder.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing debt reminder with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed debt reminder with id: {Id}.", id);
    }
}
