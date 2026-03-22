namespace PharmacyManagementSystem.Server.ExpiryAlertConfiguration;

public interface ISaveExpiryAlertConfigurationAction
{
    Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> AddAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken);
    Task<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration?> UpdateAsync(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration? expiryAlertConfiguration, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
