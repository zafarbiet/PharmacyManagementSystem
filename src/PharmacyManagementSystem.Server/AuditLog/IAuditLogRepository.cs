using PharmacyManagementSystem.Common.AuditLog;

namespace PharmacyManagementSystem.Server.AuditLog;

public interface IAuditLogRepository
{
    Task<IReadOnlyCollection<Common.AuditLog.AuditLog>?> GetByFilterCriteriaAsync(AuditLogFilter filter, CancellationToken cancellationToken);
    Task<Common.AuditLog.AuditLog?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.AuditLog.AuditLog?> AddAsync(Common.AuditLog.AuditLog? auditLog, CancellationToken cancellationToken);
}
