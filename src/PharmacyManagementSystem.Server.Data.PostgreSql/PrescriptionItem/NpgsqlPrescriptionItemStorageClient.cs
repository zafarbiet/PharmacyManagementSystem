using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.PrescriptionItem;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.PrescriptionItem;

public class NpgsqlPrescriptionItemStorageClient(ILogger<NpgsqlPrescriptionItemStorageClient> logger, INpgsqlDbClient dbClient) : IPrescriptionItemStorageClient
{
    private readonly ILogger<NpgsqlPrescriptionItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem?>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting prescriptionItems by filter criteria.");
        var sql = await PrescriptionItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} prescriptionItems.", list.Count);
        return list;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting prescriptionItem by id: {Id}.", id);
        var sql = await PrescriptionItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var prescriptionItem = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved prescriptionItem with id: {Id}.", id);
        return prescriptionItem;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);
        _logger.LogDebug("StorageClient: Adding prescriptionItem.");
        var sql = await PrescriptionItemDatabaseCommandText.GetInsertSql(prescriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added prescriptionItem.");
        return inserted;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);
        _logger.LogDebug("StorageClient: Updating prescriptionItem with id: {Id}.", prescriptionItem.Id);
        var sql = await PrescriptionItemDatabaseCommandText.GetUpdateSql(prescriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated prescriptionItem with id: {Id}.", prescriptionItem.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing prescriptionItem with id: {Id}.", id);
        var sql = await PrescriptionItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed prescriptionItem with id: {Id}.", id);
    }
}
