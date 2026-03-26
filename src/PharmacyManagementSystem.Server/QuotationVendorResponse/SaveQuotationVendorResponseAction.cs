using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public class SaveQuotationVendorResponseAction(ILogger<SaveQuotationVendorResponseAction> logger, IQuotationVendorResponseRepository repository) : ISaveQuotationVendorResponseAction
{
    private readonly ILogger<SaveQuotationVendorResponseAction> _logger = logger;
    private readonly IQuotationVendorResponseRepository _repository = repository;

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> AddAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("QuotationRequestId is required.");

        if (response.VendorId == Guid.Empty)
            throw new BadRequestException("VendorId is required.");

        response.Status ??= "pending";
        response.UpdatedBy = "system";

        _logger.LogDebug("Adding quotation vendor response for request: {RequestId}, vendor: {VendorId}.", response.QuotationRequestId, response.VendorId);

        var result = await _repository.AddAsync(response, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation vendor response with id: {Id}.", result?.Id);

        return result;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> UpdateAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("QuotationRequestId is required.");

        if (response.VendorId == Guid.Empty)
            throw new BadRequestException("VendorId is required.");

        response.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation vendor response with id: {Id}.", response.Id);

        var result = await _repository.UpdateAsync(response, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation vendor response with id: {Id}.", response.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation vendor response with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation vendor response with id: {Id}.", id);
    }
}
