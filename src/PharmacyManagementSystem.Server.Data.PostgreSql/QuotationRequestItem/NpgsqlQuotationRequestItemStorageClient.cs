using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequestItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequestItem;

public class NpgsqlQuotationRequestItemStorageClient(ILogger<NpgsqlQuotationRequestItemStorageClient> logger, INpgsqlDbClient dbClient) : IQuotationRequestItemStorageClient
{
    private readonly ILogger<NpgsqlQuotationRequestItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequestItem.QuotationRequestItem?>?> GetByFilterCriteriaAsync(QuotationRequestItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting quotationRequestItems by filter criteria.");
        var sql = await QuotationRequestItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} quotationRequestItems.", list.Count);
        return list;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting quotationRequestItem by id: {Id}.", id);
        var sql = await QuotationRequestItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);
        var quotationRequestItem = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved quotationRequestItem with id: {Id}.", id);
        return quotationRequestItem;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> AddAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);
        _logger.LogDebug("StorageClient: Adding quotationRequestItem.");
        var sql = await QuotationRequestItemDatabaseCommandText.GetInsertSql(quotationRequestItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added quotationRequestItem.");
        return inserted;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> UpdateAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);
        _logger.LogDebug("StorageClient: Updating quotationRequestItem with id: {Id}.", quotationRequestItem.Id);
        var sql = await QuotationRequestItemDatabaseCommandText.GetUpdateSql(quotationRequestItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequestItem.QuotationRequestItem>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated quotationRequestItem with id: {Id}.", quotationRequestItem.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing quotationRequestItem with id: {Id}.", id);
        var sql = await QuotationRequestItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed quotationRequestItem with id: {Id}.", id);
    }
}
