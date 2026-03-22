using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public class VendorExpiryReturnRequestRepository(ILogger<VendorExpiryReturnRequestRepository> logger, IVendorExpiryReturnRequestStorageClient storageClient) : IVendorExpiryReturnRequestRepository
{
    private readonly ILogger<VendorExpiryReturnRequestRepository> _logger = logger;
    private readonly IVendorExpiryReturnRequestStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting vendor expiry return requests by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} vendor expiry return requests.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting vendor expiry return request by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved vendor expiry return request with id: {Id}.", id);

        return result;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        _logger.LogDebug("Repository: Adding vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        var result = await _storageClient.AddAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        return result;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        _logger.LogDebug("Repository: Updating vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        var result = await _storageClient.UpdateAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing vendor expiry return request with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed vendor expiry return request with id: {Id}.", id);
    }
}
