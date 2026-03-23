using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Notification;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public class SaveQuotationRequestAction(
    ILogger<SaveQuotationRequestAction> logger,
    IQuotationRequestRepository repository,
    ISaveNotificationAction notificationAction) : ISaveQuotationRequestAction
{
    private readonly ILogger<SaveQuotationRequestAction> _logger = logger;
    private readonly IQuotationRequestRepository _repository = repository;
    private readonly ISaveNotificationAction _notificationAction = notificationAction;

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

    public async Task<Common.QuotationRequest.QuotationRequest?> DispatchToVendorsAsync(
        Guid quotationRequestId,
        IReadOnlyList<Guid> vendorIds,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorIds);

        if (vendorIds.Count == 0)
            throw new BadRequestException("At least one vendor must be specified for RFQ dispatch.");

        _logger.LogDebug("Dispatching RFQ {QuotationRequestId} to {Count} vendors.", quotationRequestId, vendorIds.Count);

        var rfq = await _repository.GetByIdAsync(quotationRequestId.ToString(), cancellationToken)
            .ConfigureAwait(false);

        if (rfq is null)
            throw new BadRequestException($"Quotation request {quotationRequestId} not found.");

        foreach (var vendorId in vendorIds)
        {
            await _notificationAction.AddAsync(new Common.Notification.Notification
            {
                NotificationType = "RFQDispatch",
                Channel = "InApp",
                RecipientType = "Vendor",
                RecipientId = vendorId,
                Subject = "Request for Quotation",
                Body = $"A new RFQ (id: {quotationRequestId}) has been raised. Please submit your quotation by {rfq.RequiredByDate:yyyy-MM-dd}.",
                ReferenceId = quotationRequestId,
                ReferenceType = "QuotationRequest",
                ScheduledAt = DateTimeOffset.UtcNow,
                Status = "Pending"
            }, cancellationToken).ConfigureAwait(false);
        }

        // Mark RFQ as dispatched
        rfq.Status = "Dispatched";
        rfq.UpdatedBy = "system";
        var result = await _repository.UpdateAsync(rfq, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("RFQ {QuotationRequestId} dispatched to {Count} vendors.", quotationRequestId, vendorIds.Count);

        return result;
    }
}
