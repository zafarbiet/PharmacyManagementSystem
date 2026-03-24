using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AuditLog;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.AuditLog;

public class NpgsqlAuditLogStorageClient(ILogger<NpgsqlAuditLogStorageClient> logger, INpgsqlDbClient dbClient) : IAuditLogStorageClient
{
    private readonly ILogger<NpgsqlAuditLogStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.AuditLog.AuditLog>?> GetByFilterCriteriaAsync(AuditLogFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting audit logs by filter criteria.");
        var sql = await AuditLogDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AuditLog.AuditLog>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} audit logs.", list.Count);
        return list;
    }

    public async Task<Common.AuditLog.AuditLog?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting audit log by id: {Id}.", id);
        var sql = await AuditLogDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AuditLog.AuditLog>(sql, cancellationToken).ConfigureAwait(false);
        var auditLog = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved audit log with id: {Id}.", id);
        return auditLog;
    }

    public async Task<Common.AuditLog.AuditLog?> AddAsync(Common.AuditLog.AuditLog? auditLog, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(auditLog);
        _logger.LogDebug("StorageClient: Adding audit log for drug {DrugId}.", auditLog.DrugId);
        var sql = await AuditLogDatabaseCommandText.GetInsertSql(auditLog).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AuditLog.AuditLog>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added audit log for drug {DrugId}.", auditLog.DrugId);
        return inserted;
    }
}
