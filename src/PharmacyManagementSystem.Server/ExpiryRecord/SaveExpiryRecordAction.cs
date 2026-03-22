using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.ExpiryRecord;

public class SaveExpiryRecordAction(ILogger<SaveExpiryRecordAction> logger, IExpiryRecordRepository repository) : ISaveExpiryRecordAction
{
    private readonly ILogger<SaveExpiryRecordAction> _logger = logger;
    private readonly IExpiryRecordRepository _repository = repository;

    public async Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        if (expiryRecord.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("ExpiryRecord DrugInventoryId is required.");

        if (string.IsNullOrWhiteSpace(expiryRecord.Status))
            throw new BadRequestException("ExpiryRecord Status is required.");

        if (string.IsNullOrWhiteSpace(expiryRecord.InitiatedBy))
            throw new BadRequestException("ExpiryRecord InitiatedBy is required.");

        if (expiryRecord.QuantityAffected <= 0)
            throw new BadRequestException("ExpiryRecord QuantityAffected must be greater than zero.");

        expiryRecord.UpdatedBy = "system";

        _logger.LogDebug("Adding new expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        var result = await _repository.AddAsync(expiryRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        return result;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        if (expiryRecord.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("ExpiryRecord DrugInventoryId is required.");

        if (string.IsNullOrWhiteSpace(expiryRecord.Status))
            throw new BadRequestException("ExpiryRecord Status is required.");

        if (string.IsNullOrWhiteSpace(expiryRecord.InitiatedBy))
            throw new BadRequestException("ExpiryRecord InitiatedBy is required.");

        if (expiryRecord.QuantityAffected <= 0)
            throw new BadRequestException("ExpiryRecord QuantityAffected must be greater than zero.");

        expiryRecord.UpdatedBy = "system";

        _logger.LogDebug("Updating expiry record with id: {Id}.", expiryRecord.Id);

        var result = await _repository.UpdateAsync(expiryRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated expiry record with id: {Id}.", expiryRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing expiry record with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed expiry record with id: {Id}.", id);
    }
}
