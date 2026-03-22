using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.PrescriptionItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PrescriptionItem;

public class SqlServerPrescriptionItemStorageClient(ILogger<SqlServerPrescriptionItemStorageClient> logger, ISqlServerDbClient dbClient) : IPrescriptionItemStorageClient
{
    private readonly ILogger<SqlServerPrescriptionItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting prescription items by filter criteria.");

        var sql = await PrescriptionItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} prescription items.", list.Count);

        return list;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting prescription item by id: {Id}.", id);

        var sql = await PrescriptionItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var prescriptionItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved prescription item with id: {Id}.", id);

        return prescriptionItem;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        _logger.LogDebug("StorageClient: Adding prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        var sql = await PrescriptionItemDatabaseCommandText.GetInsertSql(prescriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        return inserted;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        _logger.LogDebug("StorageClient: Updating prescription item with id: {Id}.", prescriptionItem.Id);

        var sql = await PrescriptionItemDatabaseCommandText.GetUpdateSql(prescriptionItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PrescriptionItem.PrescriptionItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated prescription item with id: {Id}.", prescriptionItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing prescription item with id: {Id}.", id);

        var sql = await PrescriptionItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed prescription item with id: {Id}.", id);
    }
}
