using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DrugInventoryRackAssignment;

public class SqlServerDrugInventoryRackAssignmentStorageClient(ILogger<SqlServerDrugInventoryRackAssignmentStorageClient> logger, ISqlServerDbClient dbClient) : IDrugInventoryRackAssignmentStorageClient
{
    private readonly ILogger<SqlServerDrugInventoryRackAssignmentStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drug inventory rack assignments by filter criteria.");

        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drug inventory rack assignments.", list.Count);

        return list;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug inventory rack assignment by id: {Id}.", id);

        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);

        var item = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug inventory rack assignment with id: {Id}.", id);

        return item;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> AddAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        _logger.LogDebug("StorageClient: Adding drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetInsertSql(drugInventoryRackAssignment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        return inserted;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> UpdateAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        _logger.LogDebug("StorageClient: Updating drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetUpdateSql(drugInventoryRackAssignment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug inventory rack assignment with id: {Id}.", id);

        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug inventory rack assignment with id: {Id}.", id);
    }
}
