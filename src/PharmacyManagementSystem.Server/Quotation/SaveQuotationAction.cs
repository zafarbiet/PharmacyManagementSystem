using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Quotation;

public class SaveQuotationAction(ILogger<SaveQuotationAction> logger, IQuotationRepository repository) : ISaveQuotationAction
{
    private readonly ILogger<SaveQuotationAction> _logger = logger;
    private readonly IQuotationRepository _repository = repository;

    public async Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        if (quotation.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("Quotation QuotationRequestId is required.");

        if (quotation.VendorId == Guid.Empty)
            throw new BadRequestException("Quotation VendorId is required.");

        if (string.IsNullOrWhiteSpace(quotation.Status))
            throw new BadRequestException("Quotation Status is required.");

        quotation.UpdatedBy = "system";

        _logger.LogDebug("Adding new quotation.");

        var result = await _repository.AddAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation.");

        return result;
    }

    public async Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        if (quotation.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("Quotation QuotationRequestId is required.");

        if (quotation.VendorId == Guid.Empty)
            throw new BadRequestException("Quotation VendorId is required.");

        if (string.IsNullOrWhiteSpace(quotation.Status))
            throw new BadRequestException("Quotation Status is required.");

        quotation.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation with id: {Id}.", quotation.Id);

        var result = await _repository.UpdateAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation with id: {Id}.", quotation.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation with id: {Id}.", id);
    }
}
