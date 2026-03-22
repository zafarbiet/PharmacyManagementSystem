using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DebtRecord;

public class SaveDebtRecordAction(ILogger<SaveDebtRecordAction> logger, IDebtRecordRepository repository) : ISaveDebtRecordAction
{
    private readonly ILogger<SaveDebtRecordAction> _logger = logger;
    private readonly IDebtRecordRepository _repository = repository;

    public async Task<Common.DebtRecord.DebtRecord?> AddAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        if (debtRecord.PatientId == Guid.Empty)
            throw new BadRequestException("DebtRecord PatientId is required.");

        if (debtRecord.InvoiceId == Guid.Empty)
            throw new BadRequestException("DebtRecord InvoiceId is required.");

        if (debtRecord.OriginalAmount <= 0)
            throw new BadRequestException("DebtRecord OriginalAmount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(debtRecord.Status))
            throw new BadRequestException("DebtRecord Status is required.");

        debtRecord.UpdatedBy = "system";

        _logger.LogDebug("Adding new debt record for patient id: {PatientId}.", debtRecord.PatientId);

        var result = await _repository.AddAsync(debtRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added debt record for patient id: {PatientId}.", debtRecord.PatientId);

        return result;
    }

    public async Task<Common.DebtRecord.DebtRecord?> UpdateAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        if (debtRecord.PatientId == Guid.Empty)
            throw new BadRequestException("DebtRecord PatientId is required.");

        if (debtRecord.InvoiceId == Guid.Empty)
            throw new BadRequestException("DebtRecord InvoiceId is required.");

        if (debtRecord.OriginalAmount <= 0)
            throw new BadRequestException("DebtRecord OriginalAmount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(debtRecord.Status))
            throw new BadRequestException("DebtRecord Status is required.");

        debtRecord.UpdatedBy = "system";

        _logger.LogDebug("Updating debt record with id: {Id}.", debtRecord.Id);

        var result = await _repository.UpdateAsync(debtRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated debt record with id: {Id}.", debtRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing debt record with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed debt record with id: {Id}.", id);
    }
}
