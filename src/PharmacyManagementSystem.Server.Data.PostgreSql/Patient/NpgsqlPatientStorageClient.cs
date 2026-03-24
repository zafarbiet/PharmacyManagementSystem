using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Patient;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.Patient;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Patient;

public class NpgsqlPatientStorageClient(ILogger<NpgsqlPatientStorageClient> logger, INpgsqlDbClient dbClient) : IPatientStorageClient
{
    private readonly ILogger<NpgsqlPatientStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Patient.Patient?>?> GetByFilterCriteriaAsync(PatientFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting patients by filter criteria.");
        var sql = await PatientDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Patient.Patient>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} patients.", list.Count);
        return list;
    }

    public async Task<Common.Patient.Patient?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting patient by id: {Id}.", id);
        var sql = await PatientDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Patient.Patient>(sql, cancellationToken).ConfigureAwait(false);
        var patient = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved patient with id: {Id}.", id);
        return patient;
    }

    public async Task<Common.Patient.Patient?> AddAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);
        _logger.LogDebug("StorageClient: Adding patient.");
        var sql = await PatientDatabaseCommandText.GetInsertSql(patient).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Patient.Patient>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added patient.");
        return inserted;
    }

    public async Task<Common.Patient.Patient?> UpdateAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);
        _logger.LogDebug("StorageClient: Updating patient with id: {Id}.", patient.Id);
        var sql = await PatientDatabaseCommandText.GetUpdateSql(patient).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Patient.Patient>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated patient with id: {Id}.", patient.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing patient with id: {Id}.", id);
        var sql = await PatientDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed patient with id: {Id}.", id);
    }
}
