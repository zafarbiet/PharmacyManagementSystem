using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public class SaveDamageDisposalRecordAction(ILogger<SaveDamageDisposalRecordAction> logger, IDamageDisposalRecordRepository repository) : ISaveDamageDisposalRecordAction
{
    private readonly ILogger<SaveDamageDisposalRecordAction> _logger = logger;
    private readonly IDamageDisposalRecordRepository _repository = repository;

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        if (damageDisposalRecord.DamageRecordId == Guid.Empty)
            throw new BadRequestException("DamageDisposalRecord DamageRecordId is required.");

        if (string.IsNullOrWhiteSpace(damageDisposalRecord.DisposalMethod))
            throw new BadRequestException("DamageDisposalRecord DisposalMethod is required.");

        if (string.IsNullOrWhiteSpace(damageDisposalRecord.DisposedBy))
            throw new BadRequestException("DamageDisposalRecord DisposedBy is required.");

        damageDisposalRecord.UpdatedBy = "system";

        _logger.LogDebug("Adding new damage disposal record.");

        var result = await _repository.AddAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added damage disposal record.");

        return result;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        if (damageDisposalRecord.DamageRecordId == Guid.Empty)
            throw new BadRequestException("DamageDisposalRecord DamageRecordId is required.");

        if (string.IsNullOrWhiteSpace(damageDisposalRecord.DisposalMethod))
            throw new BadRequestException("DamageDisposalRecord DisposalMethod is required.");

        if (string.IsNullOrWhiteSpace(damageDisposalRecord.DisposedBy))
            throw new BadRequestException("DamageDisposalRecord DisposedBy is required.");

        damageDisposalRecord.UpdatedBy = "system";

        _logger.LogDebug("Updating damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        var result = await _repository.UpdateAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing damage disposal record with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed damage disposal record with id: {Id}.", id);
    }
}
