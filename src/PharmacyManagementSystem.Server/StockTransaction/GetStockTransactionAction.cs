using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StockTransaction;

namespace PharmacyManagementSystem.Server.StockTransaction;

public class GetStockTransactionAction(ILogger<GetStockTransactionAction> logger, IStockTransactionRepository repository) : IGetStockTransactionAction
{
    private readonly ILogger<GetStockTransactionAction> _logger = logger;
    private readonly IStockTransactionRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting stock transactions by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} stock transactions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting stock transaction by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved stock transaction with id: {Id}.", id);

        return result;
    }
}
