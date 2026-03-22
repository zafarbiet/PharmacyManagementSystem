using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequestItem;

public class SqlServerQuotationRequestItemStorageClient(ILogger<SqlServerQuotationRequestItemStorageClient> logger, ISqlServerDbClient dbClient) : IQuotationRequestItemStorageClient
{
    private readonly ILogger<SqlServerQuotationRequestItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequestItem.QuotationRequestItem>?> GetByFilterCriteriaAsync(QuotationRequestItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting quotation request items by filter criteria.");

        var sql = await QuotationRequestItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} quotation request items.", list.Count);

        return list;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting quotation request item by id: {Id}.", id);

        var sql = await QuotationRequestItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);

        var quotationRequestItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved quotation request item with id: {Id}.", id);

        return quotationRequestItem;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> AddAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        _logger.LogDebug("StorageClient: Adding quotation request item.");

        var sql = await QuotationRequestItemDatabaseCommandText.GetInsertSql(quotationRequestItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added quotation request item.");

        return inserted;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> UpdateAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        _logger.LogDebug("StorageClient: Updating quotation request item with id: {Id}.", quotationRequestItem.Id);

        var sql = await QuotationRequestItemDatabaseCommandText.GetUpdateSql(quotationRequestItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated quotation request item with id: {Id}.", quotationRequestItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing quotation request item with id: {Id}.", id);

        var sql = await QuotationRequestItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed quotation request item with id: {Id}.", id);
    }
}
