using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public class GetVendorExpiryReturnRequestAction(ILogger<GetVendorExpiryReturnRequestAction> logger, IVendorExpiryReturnRequestRepository repository) : IGetVendorExpiryReturnRequestAction
{
    private readonly ILogger<GetVendorExpiryReturnRequestAction> _logger = logger;
    private readonly IVendorExpiryReturnRequestRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting vendor expiry return requests by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} vendor expiry return requests.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting vendor expiry return request by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved vendor expiry return request with id: {Id}.", id);

        return result;
    }
}
