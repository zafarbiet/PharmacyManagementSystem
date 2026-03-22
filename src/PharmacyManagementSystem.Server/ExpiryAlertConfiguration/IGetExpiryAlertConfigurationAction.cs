using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

public interface IGetExpiryAlertConfigurationAction
{
    Task<IReadOnlyCollection<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>?> GetByFilterCriteriaAsync(ExpiryAlertConfigurationFilter filter, CancellationToken cancellationToken);
    Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
