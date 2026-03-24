using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.SubscriptionFulfillment;

public class NpgsqlSubscriptionFulfillmentStorageClient(ILogger<NpgsqlSubscriptionFulfillmentStorageClient> logger, INpgsqlDbClient dbClient) : ISubscriptionFulfillmentStorageClient
{
    private readonly ILogger<NpgsqlSubscriptionFulfillmentStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment?>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting subscriptionFulfillments by filter criteria.");
        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} subscriptionFulfillments.", list.Count);
        return list;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting subscriptionFulfillment by id: {Id}.", id);
        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);
        var subscriptionFulfillment = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved subscriptionFulfillment with id: {Id}.", id);
        return subscriptionFulfillment;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);
        _logger.LogDebug("StorageClient: Adding subscriptionFulfillment.");
        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetInsertSql(subscriptionFulfillment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added subscriptionFulfillment.");
        return inserted;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);
        _logger.LogDebug("StorageClient: Updating subscriptionFulfillment with id: {Id}.", subscriptionFulfillment.Id);
        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetUpdateSql(subscriptionFulfillment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.SubscriptionFulfillment.SubscriptionFulfillment>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated subscriptionFulfillment with id: {Id}.", subscriptionFulfillment.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing subscriptionFulfillment with id: {Id}.", id);
        var sql = await SubscriptionFulfillmentDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed subscriptionFulfillment with id: {Id}.", id);
    }
}
