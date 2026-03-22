using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.QuotationItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.QuotationItem;

public class SqlServerQuotationItemStorageClient(ILogger<SqlServerQuotationItemStorageClient> logger, ISqlServerDbClient dbClient) : IQuotationItemStorageClient
{
    private readonly ILogger<SqlServerQuotationItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting quotation items by filter criteria.");

        var sql = await QuotationItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} quotation items.", list.Count);

        return list;
    }

    public async Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting quotation item by id: {Id}.", id);

        var sql = await QuotationItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);

        var quotationItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved quotation item with id: {Id}.", id);

        return quotationItem;
    }

    public async Task<Common.QuotationItem.QuotationItem?> AddAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        _logger.LogDebug("StorageClient: Adding quotation item.");

        var sql = await QuotationItemDatabaseCommandText.GetInsertSql(quotationItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added quotation item.");

        return inserted;
    }

    public async Task<Common.QuotationItem.QuotationItem?> UpdateAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        _logger.LogDebug("StorageClient: Updating quotation item with id: {Id}.", quotationItem.Id);

        var sql = await QuotationItemDatabaseCommandText.GetUpdateSql(quotationItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated quotation item with id: {Id}.", quotationItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing quotation item with id: {Id}.", id);

        var sql = await QuotationItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed quotation item with id: {Id}.", id);
    }
}
