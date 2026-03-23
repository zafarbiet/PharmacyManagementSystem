using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PaymentLedger;

namespace PharmacyManagementSystem.Server.PaymentLedger;

public class GetPaymentLedgerAction(ILogger<GetPaymentLedgerAction> logger, IPaymentLedgerRepository repository) : IGetPaymentLedgerAction
{
    private readonly ILogger<GetPaymentLedgerAction> _logger = logger;
    private readonly IPaymentLedgerRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting payment ledgers by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} payment ledgers.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting payment ledger by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved payment ledger with id: {Id}.", id);

        return result;
    }
}
