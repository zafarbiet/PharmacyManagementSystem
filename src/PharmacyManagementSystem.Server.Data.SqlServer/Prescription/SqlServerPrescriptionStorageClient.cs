using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Prescription;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Prescription;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Prescription;

public class SqlServerPrescriptionStorageClient(ILogger<SqlServerPrescriptionStorageClient> logger, ISqlServerDbClient dbClient) : IPrescriptionStorageClient
{
    private readonly ILogger<SqlServerPrescriptionStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Prescription.Prescription>?> GetByFilterCriteriaAsync(PrescriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting prescriptions by filter criteria.");

        var sql = await PrescriptionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Prescription.Prescription>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} prescriptions.", list.Count);

        return list;
    }

    public async Task<Common.Prescription.Prescription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting prescription by id: {Id}.", id);

        var sql = await PrescriptionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Prescription.Prescription>(sql, cancellationToken).ConfigureAwait(false);

        var prescription = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved prescription with id: {Id}.", id);

        return prescription;
    }

    public async Task<Common.Prescription.Prescription?> AddAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _logger.LogDebug("StorageClient: Adding prescription for PatientId: {PatientId}.", prescription.PatientId);

        var sql = await PrescriptionDatabaseCommandText.GetInsertSql(prescription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Prescription.Prescription>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added prescription for PatientId: {PatientId}.", prescription.PatientId);

        return inserted;
    }

    public async Task<Common.Prescription.Prescription?> UpdateAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _logger.LogDebug("StorageClient: Updating prescription with id: {Id}.", prescription.Id);

        var sql = await PrescriptionDatabaseCommandText.GetUpdateSql(prescription).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Prescription.Prescription>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated prescription with id: {Id}.", prescription.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing prescription with id: {Id}.", id);

        var sql = await PrescriptionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed prescription with id: {Id}.", id);
    }
}
