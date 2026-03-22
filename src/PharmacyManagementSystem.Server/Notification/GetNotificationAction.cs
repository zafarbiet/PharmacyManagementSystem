using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Notification;

namespace PharmacyManagementSystem.Server.Notification;

public class GetNotificationAction(ILogger<GetNotificationAction> logger, INotificationRepository repository) : IGetNotificationAction
{
    private readonly ILogger<GetNotificationAction> _logger = logger;
    private readonly INotificationRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Notification.Notification>?> GetByFilterCriteriaAsync(NotificationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting notifications by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} notifications.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Notification.Notification?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting notification by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved notification with id: {Id}.", id);

        return result;
    }
}
