namespace PharmacyManagementSystem.Server.AuditLog;

public interface ISaveAuditLogAction
{
    Task<Common.AuditLog.AuditLog?> AddAsync(Common.AuditLog.AuditLog? auditLog, CancellationToken cancellationToken);
}
