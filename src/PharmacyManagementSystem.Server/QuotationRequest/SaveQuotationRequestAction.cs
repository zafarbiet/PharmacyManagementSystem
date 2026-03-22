using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public class SaveQuotationRequestAction(ILogger<SaveQuotationRequestAction> logger, IQuotationRequestRepository repository) : ISaveQuotationRequestAction
{
    private readonly ILogger<SaveQuotationRequestAction> _logger = logger;
    private readonly IQuotationRequestRepository _repository = repository;

    public async Task<Common.QuotationRequest.QuotationRequest?> AddAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        if (string.IsNullOrWhiteSpace(quotationRequest.Status))
            throw new BadRequestException("QuotationRequest Status is required.");

        if (string.IsNullOrWhiteSpace(quotationRequest.RequestedBy))
            throw new BadRequestException("QuotationRequest RequestedBy is required.");

        quotationRequest.UpdatedBy = "system";

        _logger.LogDebug("Adding new quotation request.");

        var result = await _repository.AddAsync(quotationRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation request.");

        return result;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> UpdateAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        if (string.IsNullOrWhiteSpace(quotationRequest.Status))
            throw new BadRequestException("QuotationRequest Status is required.");

        if (string.IsNullOrWhiteSpace(quotationRequest.RequestedBy))
            throw new BadRequestException("QuotationRequest RequestedBy is required.");

        quotationRequest.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation request with id: {Id}.", quotationRequest.Id);

        var result = await _repository.UpdateAsync(quotationRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation request with id: {Id}.", quotationRequest.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation request with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation request with id: {Id}.", id);
    }
}
