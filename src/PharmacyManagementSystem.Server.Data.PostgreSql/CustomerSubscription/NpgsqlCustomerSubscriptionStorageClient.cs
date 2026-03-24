using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.CustomerSubscription;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.CustomerSubscription;

public class NpgsqlCustomerSubscriptionStorageClient(ILogger<NpgsqlCustomerSubscriptionStorageClient> logger, INpgsqlDbClient dbClient) : ICustomerSubscriptionStorageClient
{
    private readonly ILogger<NpgsqlCustomerSubscriptionStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription?>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting customerSubscriptions by filter criteria.");
        var sql = await CustomerSubscriptionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} customerSubscriptions.", list.Count);
        return list;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting customerSubscription by id: {Id}.", id);
        var sql = await CustomerSubscriptionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);
        var customerSubscription = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved customerSubscription with id: {Id}.", id);
        return customerSubscription;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);
        _logger.LogDebug("StorageClient: Adding customerSubscription.");
        var sql = await CustomerSubscriptionDatabaseCommandText.GetInsertSql(customerSubscription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added customerSubscription.");
        return inserted;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);
        _logger.LogDebug("StorageClient: Updating customerSubscription with id: {Id}.", customerSubscription.Id);
        var sql = await CustomerSubscriptionDatabaseCommandText.GetUpdateSql(customerSubscription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerSubscription.CustomerSubscription>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated customerSubscription with id: {Id}.", customerSubscription.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing customerSubscription with id: {Id}.", id);
        var sql = await CustomerSubscriptionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed customerSubscription with id: {Id}.", id);
    }
}
