using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StockTransaction;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.StockTransaction;

namespace PharmacyManagementSystem.Server.Data.SqlServer.StockTransaction;

public class SqlServerStockTransactionStorageClient(ILogger<SqlServerStockTransactionStorageClient> logger, ISqlServerDbClient dbClient) : IStockTransactionStorageClient
{
    private readonly ILogger<SqlServerStockTransactionStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting stock transactions by filter criteria.");

        var sql = await StockTransactionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} stock transactions.", list.Count);

        return list;
    }

    public async Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting stock transaction by id: {Id}.", id);

        var sql = await StockTransactionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);

        var stockTransaction = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved stock transaction with id: {Id}.", id);

        return stockTransaction;
    }

    public async Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        _logger.LogDebug("StorageClient: Adding stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        var sql = await StockTransactionDatabaseCommandText.GetInsertSql(stockTransaction).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added stock transaction for DrugId: {DrugId}.", stockTransaction.DrugId);

        return inserted;
    }

    public async Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);

        _logger.LogDebug("StorageClient: Updating stock transaction with id: {Id}.", stockTransaction.Id);

        var sql = await StockTransactionDatabaseCommandText.GetUpdateSql(stockTransaction).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StockTransaction.StockTransaction>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated stock transaction with id: {Id}.", stockTransaction.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing stock transaction with id: {Id}.", id);

        var sql = await StockTransactionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed stock transaction with id: {Id}.", id);
    }
}
