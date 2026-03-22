using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DisposalRecord;

public class SaveDisposalRecordAction(ILogger<SaveDisposalRecordAction> logger, IDisposalRecordRepository repository) : ISaveDisposalRecordAction
{
    private readonly ILogger<SaveDisposalRecordAction> _logger = logger;
    private readonly IDisposalRecordRepository _repository = repository;

    public async Task<Common.DisposalRecord.DisposalRecord?> AddAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        if (disposalRecord.ExpiryRecordId == Guid.Empty)
            throw new BadRequestException("DisposalRecord ExpiryRecordId is required.");

        if (string.IsNullOrWhiteSpace(disposalRecord.DisposalMethod))
            throw new BadRequestException("DisposalRecord DisposalMethod is required.");

        if (string.IsNullOrWhiteSpace(disposalRecord.DisposedBy))
            throw new BadRequestException("DisposalRecord DisposedBy is required.");

        if (disposalRecord.QuantityDisposed <= 0)
            throw new BadRequestException("DisposalRecord QuantityDisposed must be greater than zero.");

        disposalRecord.UpdatedBy = "system";

        _logger.LogDebug("Adding new disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        var result = await _repository.AddAsync(disposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        return result;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> UpdateAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        if (disposalRecord.ExpiryRecordId == Guid.Empty)
            throw new BadRequestException("DisposalRecord ExpiryRecordId is required.");

        if (string.IsNullOrWhiteSpace(disposalRecord.DisposalMethod))
            throw new BadRequestException("DisposalRecord DisposalMethod is required.");

        if (string.IsNullOrWhiteSpace(disposalRecord.DisposedBy))
            throw new BadRequestException("DisposalRecord DisposedBy is required.");

        if (disposalRecord.QuantityDisposed <= 0)
            throw new BadRequestException("DisposalRecord QuantityDisposed must be greater than zero.");

        disposalRecord.UpdatedBy = "system";

        _logger.LogDebug("Updating disposal record with id: {Id}.", disposalRecord.Id);

        var result = await _repository.UpdateAsync(disposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated disposal record with id: {Id}.", disposalRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing disposal record with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed disposal record with id: {Id}.", id);
    }
}
