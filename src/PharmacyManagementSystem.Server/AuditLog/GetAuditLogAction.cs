using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AuditLog;

namespace PharmacyManagementSystem.Server.AuditLog;

public class GetAuditLogAction(ILogger<GetAuditLogAction> logger, IAuditLogRepository repository) : IGetAuditLogAction
{
    private readonly ILogger<GetAuditLogAction> _logger = logger;
    private readonly IAuditLogRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.AuditLog.AuditLog>?> GetByFilterCriteriaAsync(AuditLogFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting audit logs by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} audit logs.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.AuditLog.AuditLog?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting audit log by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved audit log with id: {Id}.", id);

        return result;
    }
}
