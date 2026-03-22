using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.StockTransaction;

public class SaveStockTransactionAction(ILogger<SaveStockTransactionAction> logger, IStockTransactionRepository repository) : ISaveStockTransactionAction
{
    private readonly ILogger<SaveStockTransactionAction> _logger = logger;
    private readonly IStockTransactionRepository _repository = repository;

    public async Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        if (stockTransaction.DrugId == Guid.Empty)
            throw new BadRequestException("StockTransaction DrugId is required.");

        if (string.IsNullOrWhiteSpace(stockTransaction.TransactionType))
            throw new BadRequestException("StockTransaction TransactionType is required.");

        stockTransaction.UpdatedBy = "system";

        _logger.LogDebug("Adding new stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        var result = await _repository.AddAsync(stockTransaction, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        return result;
    }

    public async Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        if (stockTransaction.DrugId == Guid.Empty)
            throw new BadRequestException("StockTransaction DrugId is required.");

        if (string.IsNullOrWhiteSpace(stockTransaction.TransactionType))
            throw new BadRequestException("StockTransaction TransactionType is required.");

        stockTransaction.UpdatedBy = "system";

        _logger.LogDebug("Updating stock transaction with id: {Id}.", stockTransaction.Id);

        var result = await _repository.UpdateAsync(stockTransaction, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated stock transaction with id: {Id}.", stockTransaction.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing stock transaction with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed stock transaction with id: {Id}.", id);
    }
}
