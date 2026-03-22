using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

public class ExpiryAlertConfigurationRepository(ILogger<ExpiryAlertConfigurationRepository> logger, IExpiryAlertConfigurationStorageClient storageClient) : IExpiryAlertConfigurationRepository
{
    private readonly ILogger<ExpiryAlertConfigurationRepository> _logger = logger;
    private readonly IExpiryAlertConfigurationStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>?> GetByFilterCriteriaAsync(ExpiryAlertConfigurationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting expiry alert configurations by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} expiry alert configurations.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting expiry alert configuration by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved expiry alert configuration with id: {Id}.", id);

        return result;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> AddAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        _logger.LogDebug("Repository: Adding expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        var result = await _storageClient.AddAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        return result;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> UpdateAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        _logger.LogDebug("Repository: Updating expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        var result = await _storageClient.UpdateAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing expiry alert configuration with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed expiry alert configuration with id: {Id}.", id);
    }
}
