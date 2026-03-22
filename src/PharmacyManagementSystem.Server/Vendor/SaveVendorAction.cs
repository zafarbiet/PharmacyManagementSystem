using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Vendor;

public class SaveVendorAction(ILogger<SaveVendorAction> logger, IVendorRepository repository) : ISaveVendorAction
{
    private readonly ILogger<SaveVendorAction> _logger = logger;
    private readonly IVendorRepository _repository = repository;

    public async Task<Common.Vendor.Vendor?> AddAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        if (string.IsNullOrWhiteSpace(vendor.Name))
            throw new BadRequestException("Vendor Name is required.");

        vendor.UpdatedBy = "system";

        _logger.LogDebug("Adding new vendor with name: {Name}.", vendor.Name);

        var result = await _repository.AddAsync(vendor, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added vendor with name: {Name}.", vendor.Name);

        return result;
    }

    public async Task<Common.Vendor.Vendor?> UpdateAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        if (string.IsNullOrWhiteSpace(vendor.Name))
            throw new BadRequestException("Vendor Name is required.");

        vendor.UpdatedBy = "system";

        _logger.LogDebug("Updating vendor with id: {Id}.", vendor.Id);

        var result = await _repository.UpdateAsync(vendor, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated vendor with id: {Id}.", vendor.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing vendor with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed vendor with id: {Id}.", id);
    }
}
