using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StockTransaction;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.StockTransaction;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.StockTransaction;

public class NpgsqlStockTransactionStorageClient(ILogger<NpgsqlStockTransactionStorageClient> logger, INpgsqlDbClient dbClient) : IStockTransactionStorageClient
{
    private readonly ILogger<NpgsqlStockTransactionStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction?>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting stockTransactions by filter criteria.");
        var sql = await StockTransactionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} stockTransactions.", list.Count);
        return list;
    }

    public async Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting stockTransaction by id: {Id}.", id);
        var sql = await StockTransactionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);
        var stockTransaction = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved stockTransaction with id: {Id}.", id);
        return stockTransaction;
    }

    public async Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);
        _logger.LogDebug("StorageClient: Adding stockTransaction.");
        var sql = await StockTransactionDatabaseCommandText.GetInsertSql(stockTransaction).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added stockTransaction.");
        return inserted;
    }

    public async Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);
        _logger.LogDebug("StorageClient: Updating stockTransaction with id: {Id}.", stockTransaction.Id);
        var sql = await StockTransactionDatabaseCommandText.GetUpdateSql(stockTransaction).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated stockTransaction with id: {Id}.", stockTransaction.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing stockTransaction with id: {Id}.", id);
        var sql = await StockTransactionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed stockTransaction with id: {Id}.", id);
    }
}
