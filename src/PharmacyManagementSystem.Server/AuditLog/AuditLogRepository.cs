using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AuditLog;

namespace PharmacyManagementSystem.Server.AuditLog;

public class AuditLogRepository(ILogger<AuditLogRepository> logger, IAuditLogStorageClient storageClient) : IAuditLogRepository
{
    private readonly ILogger<AuditLogRepository> _logger = logger;
    private readonly IAuditLogStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.AuditLog.AuditLog>?> GetByFilterCriteriaAsync(AuditLogFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting audit logs by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} audit logs.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.AuditLog.AuditLog?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting audit log by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved audit log with id: {Id}.", id);

        return result;
    }

    public async Task<Common.AuditLog.AuditLog?> AddAsync(Common.AuditLog.AuditLog? auditLog, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        _logger.LogDebug("Repository: Adding audit log for drug {DrugId}.", auditLog.DrugId);

        var result = await _storageClient.AddAsync(auditLog, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added audit log for drug {DrugId}.", auditLog.DrugId);

        return result;
    }
}
