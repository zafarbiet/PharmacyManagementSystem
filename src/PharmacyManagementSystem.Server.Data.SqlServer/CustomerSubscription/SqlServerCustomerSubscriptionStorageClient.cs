using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.CustomerSubscription;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerSubscription;

public class SqlServerCustomerSubscriptionStorageClient(ILogger<SqlServerCustomerSubscriptionStorageClient> logger, ISqlServerDbClient dbClient) : ICustomerSubscriptionStorageClient
{
    private readonly ILogger<SqlServerCustomerSubscriptionStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting customer subscriptions by filter criteria.");

        var sql = await CustomerSubscriptionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} customer subscriptions.", list.Count);

        return list;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting customer subscription by id: {Id}.", id);

        var sql = await CustomerSubscriptionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);

        var customerSubscription = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved customer subscription with id: {Id}.", id);

        return customerSubscription;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        _logger.LogDebug("StorageClient: Adding customer subscription.");

        var sql = await CustomerSubscriptionDatabaseCommandText.GetInsertSql(customerSubscription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added customer subscription.");

        return inserted;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        _logger.LogDebug("StorageClient: Updating customer subscription with id: {Id}.", customerSubscription.Id);

        var sql = await CustomerSubscriptionDatabaseCommandText.GetUpdateSql(customerSubscription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated customer subscription with id: {Id}.", customerSubscription.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing customer subscription with id: {Id}.", id);

        var sql = await CustomerSubscriptionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed customer subscription with id: {Id}.", id);
    }
}
