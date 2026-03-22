using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscriptionItem;

public class SqlServerCustomerSubscriptionItemStorageClient(ILogger<SqlServerCustomerSubscriptionItemStorageClient> logger, ISqlServerDbClient dbClient) : ICustomerSubscriptionItemStorageClient
{
    private readonly ILogger<SqlServerCustomerSubscriptionItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting customer subscription items by filter criteria.");

        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} customer subscription items.", list.Count);

        return list;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting customer subscription item by id: {Id}.", id);

        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var customerSubscriptionItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved customer subscription item with id: {Id}.", id);

        return customerSubscriptionItem;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        _logger.LogDebug("StorageClient: Adding customer subscription item.");

        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetInsertSql(customerSubscriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added customer subscription item.");

        return inserted;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        _logger.LogDebug("StorageClient: Updating customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetUpdateSql(customerSubscriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing customer subscription item with id: {Id}.", id);

        var sql = await CustomerSubscriptionItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed customer subscription item with id: {Id}.", id);
    }
}
