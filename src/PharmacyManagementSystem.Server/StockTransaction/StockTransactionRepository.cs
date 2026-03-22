using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StockTransaction;

namespace PharmacyManagementSystem.Server.StockTransaction;

public class StockTransactionRepository(ILogger<StockTransactionRepository> logger, IStockTransactionStorageClient storageClient) : IStockTransactionRepository
{
    private readonly ILogger<StockTransactionRepository> _logger = logger;
    private readonly IStockTransactionStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting stock transactions by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} stock transactions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting stock transaction by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved stock transaction with id: {Id}.", id);

        return result;
    }

    public async Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        _logger.LogDebug("Repository: Adding stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        var result = await _storageClient.AddAsync(stockTransaction, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        return result;
    }

    public async Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        _logger.LogDebug("Repository: Updating stock transaction with id: {Id}.", stockTransaction.Id);

        var result = await _storageClient.UpdateAsync(stockTransaction, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated stock transaction with id: {Id}.", stockTransaction.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing stock transaction with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed stock transaction with id: {Id}.", id);
    }
}
