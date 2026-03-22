using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public class SaveVendorExpiryReturnRequestAction(ILogger<SaveVendorExpiryReturnRequestAction> logger, IVendorExpiryReturnRequestRepository repository) : ISaveVendorExpiryReturnRequestAction
{
    private readonly ILogger<SaveVendorExpiryReturnRequestAction> _logger = logger;
    private readonly IVendorExpiryReturnRequestRepository _repository = repository;

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        if (vendorExpiryReturnRequest.ExpiryRecordId == Guid.Empty)
            throw new BadRequestException("VendorExpiryReturnRequest ExpiryRecordId is required.");

        if (vendorExpiryReturnRequest.VendorId == Guid.Empty)
            throw new BadRequestException("VendorExpiryReturnRequest VendorId is required.");

        if (string.IsNullOrWhiteSpace(vendorExpiryReturnRequest.Status))
            throw new BadRequestException("VendorExpiryReturnRequest Status is required.");

        if (vendorExpiryReturnRequest.QuantityToReturn <= 0)
            throw new BadRequestException("VendorExpiryReturnRequest QuantityToReturn must be greater than zero.");

        vendorExpiryReturnRequest.UpdatedBy = "system";

        _logger.LogDebug("Adding new vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        var result = await _repository.AddAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        return result;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        if (vendorExpiryReturnRequest.ExpiryRecordId == Guid.Empty)
            throw new BadRequestException("VendorExpiryReturnRequest ExpiryRecordId is required.");

        if (vendorExpiryReturnRequest.VendorId == Guid.Empty)
            throw new BadRequestException("VendorExpiryReturnRequest VendorId is required.");

        if (string.IsNullOrWhiteSpace(vendorExpiryReturnRequest.Status))
            throw new BadRequestException("VendorExpiryReturnRequest Status is required.");

        if (vendorExpiryReturnRequest.QuantityToReturn <= 0)
            throw new BadRequestException("VendorExpiryReturnRequest QuantityToReturn must be greater than zero.");

        vendorExpiryReturnRequest.UpdatedBy = "system";

        _logger.LogDebug("Updating vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        var result = await _repository.UpdateAsync(vendorExpiryReturnRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing vendor expiry return request with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed vendor expiry return request with id: {Id}.", id);
    }
}
