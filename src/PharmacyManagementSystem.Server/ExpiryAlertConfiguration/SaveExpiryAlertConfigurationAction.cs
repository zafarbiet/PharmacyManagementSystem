using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

public class SaveExpiryAlertConfigurationAction(ILogger<SaveExpiryAlertConfigurationAction> logger, IExpiryAlertConfigurationRepository repository) : ISaveExpiryAlertConfigurationAction
{
    private readonly ILogger<SaveExpiryAlertConfigurationAction> _logger = logger;
    private readonly IExpiryAlertConfigurationRepository _repository = repository;

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> AddAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        if (expiryAlertConfiguration.ThresholdDays <= 0)
            throw new BadRequestException("ExpiryAlertConfiguration ThresholdDays must be greater than zero.");

        if (string.IsNullOrWhiteSpace(expiryAlertConfiguration.AlertType))
            throw new BadRequestException("ExpiryAlertConfiguration AlertType is required.");

        expiryAlertConfiguration.UpdatedBy = "system";

        _logger.LogDebug("Adding new expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        var result = await _repository.AddAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added expiry alert configuration with alert type: {AlertType}.", expiryAlertConfiguration.AlertType);

        return result;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> UpdateAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        if (expiryAlertConfiguration.ThresholdDays <= 0)
            throw new BadRequestException("ExpiryAlertConfiguration ThresholdDays must be greater than zero.");

        if (string.IsNullOrWhiteSpace(expiryAlertConfiguration.AlertType))
            throw new BadRequestException("ExpiryAlertConfiguration AlertType is required.");

        expiryAlertConfiguration.UpdatedBy = "system";

        _logger.LogDebug("Updating expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        var result = await _repository.UpdateAsync(expiryAlertConfiguration, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated expiry alert configuration with id: {Id}.", expiryAlertConfiguration.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing expiry alert configuration with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed expiry alert configuration with id: {Id}.", id);
    }
}
