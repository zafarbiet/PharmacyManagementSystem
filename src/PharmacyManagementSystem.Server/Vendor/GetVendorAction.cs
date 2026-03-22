using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Vendor;

namespace PharmacyManagementSystem.Server.Vendor;

public class GetVendorAction(ILogger<GetVendorAction> logger, IVendorRepository repository) : IGetVendorAction
{
    private readonly ILogger<GetVendorAction> _logger = logger;
    private readonly IVendorRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Vendor.Vendor>?> GetByFilterCriteriaAsync(VendorFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting vendors by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} vendors.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Vendor.Vendor?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting vendor by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved vendor with id: {Id}.", id);

        return result;
    }
}
