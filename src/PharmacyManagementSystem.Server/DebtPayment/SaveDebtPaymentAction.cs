using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DebtPayment;

public class SaveDebtPaymentAction(ILogger<SaveDebtPaymentAction> logger, IDebtPaymentRepository repository) : ISaveDebtPaymentAction
{
    private readonly ILogger<SaveDebtPaymentAction> _logger = logger;
    private readonly IDebtPaymentRepository _repository = repository;

    public async Task<Common.DebtPayment.DebtPayment?> AddAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        if (debtPayment.DebtRecordId == Guid.Empty)
            throw new BadRequestException("DebtPayment DebtRecordId is required.");

        if (debtPayment.AmountPaid <= 0)
            throw new BadRequestException("DebtPayment AmountPaid must be greater than zero.");

        if (string.IsNullOrWhiteSpace(debtPayment.ReceivedBy))
            throw new BadRequestException("DebtPayment ReceivedBy is required.");

        debtPayment.UpdatedBy = "system";

        _logger.LogDebug("Adding new debt payment for debt record id: {DebtRecordId}.", debtPayment.DebtRecordId);

        var result = await _repository.AddAsync(debtPayment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added debt payment for debt record id: {DebtRecordId}.", debtPayment.DebtRecordId);

        return result;
    }

    public async Task<Common.DebtPayment.DebtPayment?> UpdateAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        if (debtPayment.DebtRecordId == Guid.Empty)
            throw new BadRequestException("DebtPayment DebtRecordId is required.");

        if (debtPayment.AmountPaid <= 0)
            throw new BadRequestException("DebtPayment AmountPaid must be greater than zero.");

        if (string.IsNullOrWhiteSpace(debtPayment.ReceivedBy))
            throw new BadRequestException("DebtPayment ReceivedBy is required.");

        debtPayment.UpdatedBy = "system";

        _logger.LogDebug("Updating debt payment with id: {Id}.", debtPayment.Id);

        var result = await _repository.UpdateAsync(debtPayment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated debt payment with id: {Id}.", debtPayment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing debt payment with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed debt payment with id: {Id}.", id);
    }
}
