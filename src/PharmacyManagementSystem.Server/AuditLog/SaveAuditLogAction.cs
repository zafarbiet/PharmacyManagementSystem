using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.AuditLog;

public class SaveAuditLogAction(ILogger<SaveAuditLogAction> logger, IAuditLogRepository repository) : ISaveAuditLogAction
{
    private readonly ILogger<SaveAuditLogAction> _logger = logger;
    private readonly IAuditLogRepository _repository = repository;

    public async Task<Common.AuditLog.AuditLog?> AddAsync(Common.AuditLog.AuditLog? auditLog, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        if (auditLog.DrugId == Guid.Empty)
            throw new BadRequestException("AuditLog DrugId is required.");

        if (auditLog.CustomerInvoiceId == Guid.Empty)
            throw new BadRequestException("AuditLog CustomerInvoiceId is required.");

        auditLog.PerformedAt = DateTimeOffset.UtcNow;
        auditLog.RetentionUntil = auditLog.PerformedAt.AddYears(3);
        auditLog.UpdatedBy = auditLog.PerformedBy ?? "system";

        _logger.LogDebug("Adding audit log for drug {DrugId} on invoice {InvoiceId}.", auditLog.DrugId, auditLog.CustomerInvoiceId);

        var result = await _repository.AddAsync(auditLog, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added audit log for drug {DrugId}.", auditLog.DrugId);

        return result;
    }
}
