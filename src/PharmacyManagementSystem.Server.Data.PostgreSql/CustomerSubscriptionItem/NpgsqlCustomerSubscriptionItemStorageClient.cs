using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.CustomerSubscriptionItem;

public class NpgsqlCustomerSubscriptionItemStorageClient(ILogger<NpgsqlCustomerSubscriptionItemStorageClient> logger, INpgsqlDbClient dbClient) : ICustomerSubscriptionItemStorageClient
{
    private readonly ILogger<NpgsqlCustomerSubscriptionItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting customerSubscriptionItems by filter criteria.");
        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} customerSubscriptionItems.", list.Count);
        return list;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting customerSubscriptionItem by id: {Id}.", id);
        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var customerSubscriptionItem = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved customerSubscriptionItem with id: {Id}.", id);
        return customerSubscriptionItem;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);
        _logger.LogDebug("StorageClient: Adding customerSubscriptionItem.");
        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetInsertSql(customerSubscriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added customerSubscriptionItem.");
        return inserted;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);
        _logger.LogDebug("StorageClient: Updating customerSubscriptionItem with id: {Id}.", customerSubscriptionItem.Id);
        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetUpdateSql(customerSubscriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated customerSubscriptionItem with id: {Id}.", customerSubscriptionItem.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing customerSubscriptionItem with id: {Id}.", id);
        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed customerSubscriptionItem with id: {Id}.", id);
    }
}
