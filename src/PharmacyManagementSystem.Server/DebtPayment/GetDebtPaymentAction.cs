using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtPayment;

namespace PharmacyManagementSystem.Server.DebtPayment;

public class GetDebtPaymentAction(ILogger<GetDebtPaymentAction> logger, IDebtPaymentRepository repository) : IGetDebtPaymentAction
{
    private readonly ILogger<GetDebtPaymentAction> _logger = logger;
    private readonly IDebtPaymentRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DebtPayment.DebtPayment>?> GetByFilterCriteriaAsync(DebtPaymentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting debt payments by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} debt payments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtPayment.DebtPayment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting debt payment by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved debt payment with id: {Id}.", id);

        return result;
    }
}
