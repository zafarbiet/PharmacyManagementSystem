using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;

namespace PharmacyManagementSystem.Server.Data.SqlServer.SubscriptionFulfillment;

public class SqlServerSubscriptionFulfillmentStorageClient(ILogger<SqlServerSubscriptionFulfillmentStorageClient> logger, ISqlServerDbClient dbClient) : ISubscriptionFulfillmentStorageClient
{
    private readonly ILogger<SqlServerSubscriptionFulfillmentStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting subscription fulfillments by filter criteria.");

        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} subscription fulfillments.", list.Count);

        return list;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting subscription fulfillment by id: {Id}.", id);

        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);

        var subscriptionFulfillment = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved subscription fulfillment with id: {Id}.", id);

        return subscriptionFulfillment;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        _logger.LogDebug("StorageClient: Adding subscription fulfillment.");

        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetInsertSql(subscriptionFulfillment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added subscription fulfillment.");

        return inserted;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        _logger.LogDebug("StorageClient: Updating subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetUpdateSql(subscriptionFulfillment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing subscription fulfillment with id: {Id}.", id);

        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed subscription fulfillment with id: {Id}.", id);
    }
}
