using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Notification;

public class SaveNotificationAction(ILogger<SaveNotificationAction> logger, INotificationRepository repository) : ISaveNotificationAction
{
    private readonly ILogger<SaveNotificationAction> _logger = logger;
    private readonly INotificationRepository _repository = repository;

    public async Task<Common.Notification.Notification?> AddAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (string.IsNullOrWhiteSpace(notification.NotificationType))
            throw new BadRequestException("Notification NotificationType is required.");

        if (string.IsNullOrWhiteSpace(notification.Channel))
            throw new BadRequestException("Notification Channel is required.");

        if (string.IsNullOrWhiteSpace(notification.Status))
            throw new BadRequestException("Notification Status is required.");

        if (string.IsNullOrWhiteSpace(notification.RecipientType))
            throw new BadRequestException("Notification RecipientType is required.");

        notification.UpdatedBy = "system";

        _logger.LogDebug("Adding new notification.");

        var result = await _repository.AddAsync(notification, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added notification.");

        return result;
    }

    public async Task<Common.Notification.Notification?> UpdateAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (string.IsNullOrWhiteSpace(notification.NotificationType))
            throw new BadRequestException("Notification NotificationType is required.");

        if (string.IsNullOrWhiteSpace(notification.Channel))
            throw new BadRequestException("Notification Channel is required.");

        if (string.IsNullOrWhiteSpace(notification.Status))
            throw new BadRequestException("Notification Status is required.");

        if (string.IsNullOrWhiteSpace(notification.RecipientType))
            throw new BadRequestException("Notification RecipientType is required.");

        notification.UpdatedBy = "system";

        _logger.LogDebug("Updating notification with id: {Id}.", notification.Id);

        var result = await _repository.UpdateAsync(notification, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated notification with id: {Id}.", notification.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing notification with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed notification with id: {Id}.", id);
    }
}
