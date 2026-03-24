using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.QuotationItem;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationItem;

public class NpgsqlQuotationItemStorageClient(ILogger<NpgsqlQuotationItemStorageClient> logger, INpgsqlDbClient dbClient) : IQuotationItemStorageClient
{
    private readonly ILogger<NpgsqlQuotationItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem?>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting quotationItems by filter criteria.");
        var sql = await QuotationItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} quotationItems.", list.Count);
        return list;
    }

    public async Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting quotationItem by id: {Id}.", id);
        var sql = await QuotationItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);
        var quotationItem = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved quotationItem with id: {Id}.", id);
        return quotationItem;
    }

    public async Task<Common.QuotationItem.QuotationItem?> AddAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);
        _logger.LogDebug("StorageClient: Adding quotationItem.");
        var sql = await QuotationItemDatabaseCommandText.GetInsertSql(quotationItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added quotationItem.");
        return inserted;
    }

    public async Task<Common.QuotationItem.QuotationItem?> UpdateAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);
        _logger.LogDebug("StorageClient: Updating quotationItem with id: {Id}.", quotationItem.Id);
        var sql = await QuotationItemDatabaseCommandText.GetUpdateSql(quotationItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationItem.QuotationItem>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated quotationItem with id: {Id}.", quotationItem.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing quotationItem with id: {Id}.", id);
        var sql = await QuotationItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed quotationItem with id: {Id}.", id);
    }
}
