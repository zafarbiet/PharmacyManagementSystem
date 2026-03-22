using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

public class GetExpiryAlertConfigurationAction(ILogger<GetExpiryAlertConfigurationAction> logger, IExpiryAlertConfigurationRepository repository) : IGetExpiryAlertConfigurationAction
{
    private readonly ILogger<GetExpiryAlertConfigurationAction> _logger = logger;
    private readonly IExpiryAlertConfigurationRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>?> GetByFilterCriteriaAsync(ExpiryAlertConfigurationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting expiry alert configurations by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} expiry alert configurations.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting expiry alert configuration by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved expiry alert configuration with id: {Id}.", id);

        return result;
    }
}
