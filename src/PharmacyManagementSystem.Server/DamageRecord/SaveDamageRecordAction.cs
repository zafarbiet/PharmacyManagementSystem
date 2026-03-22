using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DamageRecord;

public class SaveDamageRecordAction(ILogger<SaveDamageRecordAction> logger, IDamageRecordRepository repository) : ISaveDamageRecordAction
{
    private readonly ILogger<SaveDamageRecordAction> _logger = logger;
    private readonly IDamageRecordRepository _repository = repository;

    public async Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        if (damageRecord.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("DamageRecord DrugInventoryId is required.");

        if (damageRecord.QuantityDamaged <= 0)
            throw new BadRequestException("DamageRecord QuantityDamaged must be greater than zero.");

        if (string.IsNullOrWhiteSpace(damageRecord.DamageType))
            throw new BadRequestException("DamageRecord DamageType is required.");

        if (string.IsNullOrWhiteSpace(damageRecord.DiscoveredBy))
            throw new BadRequestException("DamageRecord DiscoveredBy is required.");

        if (string.IsNullOrWhiteSpace(damageRecord.Status))
            throw new BadRequestException("DamageRecord Status is required.");

        damageRecord.UpdatedBy = "system";

        _logger.LogDebug("Adding new damage record.");

        var result = await _repository.AddAsync(damageRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added damage record.");

        return result;
    }

    public async Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        if (damageRecord.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("DamageRecord DrugInventoryId is required.");

        if (damageRecord.QuantityDamaged <= 0)
            throw new BadRequestException("DamageRecord QuantityDamaged must be greater than zero.");

        if (string.IsNullOrWhiteSpace(damageRecord.DamageType))
            throw new BadRequestException("DamageRecord DamageType is required.");

        if (string.IsNullOrWhiteSpace(damageRecord.DiscoveredBy))
            throw new BadRequestException("DamageRecord DiscoveredBy is required.");

        if (string.IsNullOrWhiteSpace(damageRecord.Status))
            throw new BadRequestException("DamageRecord Status is required.");

        damageRecord.UpdatedBy = "system";

        _logger.LogDebug("Updating damage record with id: {Id}.", damageRecord.Id);

        var result = await _repository.UpdateAsync(damageRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated damage record with id: {Id}.", damageRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing damage record with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed damage record with id: {Id}.", id);
    }
}
