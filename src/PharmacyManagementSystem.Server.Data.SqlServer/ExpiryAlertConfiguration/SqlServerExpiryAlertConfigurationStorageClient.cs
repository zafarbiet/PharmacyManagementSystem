using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.Data.SqlServer.ExpiryAlertConfiguration;

public class SqlServerExpiryAlertConfigurationStorageClient(ILogger<SqlServerExpiryAlertConfigurationStorageClient> logger, ISqlServerDbClient dbClient) : IExpiryAlertConfigurationStorageClient
{
    private readonly ILogger<SqlServerExpiryAlertConfigurationStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>?> GetByFilterCriteriaAsync(ExpiryAlertConfigurationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting expiry alert configurations by filter criteria.");

        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} expiry alert configurations.", list.Count);

        return list;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting expiry alert configuration by id: {Id}.", id);

        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);

        var item = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved expiry alert configuration with id: {Id}.", id);

        return item;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> AddAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        _logger.LogDebug("StorageClient: Adding expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetInsertSql(expiryAlertConfiguration).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        return inserted;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> UpdateAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        _logger.LogDebug("StorageClient: Updating expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetUpdateSql(expiryAlertConfiguration).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing expiry alert configuration with id: {Id}.", id);

        var sql = await ExpiryAlertConfigurationDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed expiry alert configuration with id: {Id}.", id);
    }
}
