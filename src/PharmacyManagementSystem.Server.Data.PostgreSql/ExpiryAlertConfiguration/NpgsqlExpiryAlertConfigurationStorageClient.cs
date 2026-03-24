using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.ExpiryAlertConfiguration;

public class NpgsqlExpiryAlertConfigurationStorageClient(ILogger<NpgsqlExpiryAlertConfigurationStorageClient> logger, INpgsqlDbClient dbClient) : IExpiryAlertConfigurationStorageClient
{
    private readonly ILogger<NpgsqlExpiryAlertConfigurationStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?>?> GetByFilterCriteriaAsync(ExpiryAlertConfigurationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting expiryAlertConfigurations by filter criteria.");
        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} expiryAlertConfigurations.", list.Count);
        return list;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting expiryAlertConfiguration by id: {Id}.", id);
        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);
        var expiryAlertConfiguration = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved expiryAlertConfiguration with id: {Id}.", id);
        return expiryAlertConfiguration;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> AddAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);
        _logger.LogDebug("StorageClient: Adding expiryAlertConfiguration.");
        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetInsertSql(expiryAlertConfiguration).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added expiryAlertConfiguration.");
        return inserted;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> UpdateAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);
        _logger.LogDebug("StorageClient: Updating expiryAlertConfiguration with id: {Id}.", expiryAlertConfiguration.Id);
        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetUpdateSql(expiryAlertConfiguration).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated expiryAlertConfiguration with id: {Id}.", expiryAlertConfiguration.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing expiryAlertConfiguration with id: {Id}.", id);
        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed expiryAlertConfiguration with id: {Id}.", id);
    }
}
