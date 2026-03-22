using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Vendor;

namespace PharmacyManagementSystem.Server.Vendor;

public class VendorRepository(ILogger<VendorRepository> logger, IVendorStorageClient storageClient) : IVendorRepository
{
    private readonly ILogger<VendorRepository> _logger = logger;
    private readonly IVendorStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Vendor.Vendor>?> GetByFilterCriteriaAsync(VendorFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting vendors by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} vendors.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Vendor.Vendor?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting vendor by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved vendor with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Vendor.Vendor?> AddAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        _logger.LogDebug("Repository: Adding vendor with name: {Name}.", vendor.Name);

        var result = await _storageClient.AddAsync(vendor, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added vendor with name: {Name}.", vendor.Name);

        return result;
    }

    public async Task<Common.Vendor.Vendor?> UpdateAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        _logger.LogDebug("Repository: Updating vendor with id: {Id}.", vendor.Id);

        var result = await _storageClient.UpdateAsync(vendor, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated vendor with id: {Id}.", vendor.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing vendor with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed vendor with id: {Id}.", id);
    }
}
