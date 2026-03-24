using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugInventoryRackAssignment;

public class NpgsqlDrugInventoryRackAssignmentStorageClient(ILogger<NpgsqlDrugInventoryRackAssignmentStorageClient> logger, INpgsqlDbClient dbClient) : IDrugInventoryRackAssignmentStorageClient
{
    private readonly ILogger<NpgsqlDrugInventoryRackAssignmentStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting drugInventoryRackAssignments by filter criteria.");
        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} drugInventoryRackAssignments.", list.Count);
        return list;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting drugInventoryRackAssignment by id: {Id}.", id);
        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);
        var drugInventoryRackAssignment = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved drugInventoryRackAssignment with id: {Id}.", id);
        return drugInventoryRackAssignment;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> AddAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);
        _logger.LogDebug("StorageClient: Adding drugInventoryRackAssignment.");
        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetInsertSql(drugInventoryRackAssignment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added drugInventoryRackAssignment.");
        return inserted;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> UpdateAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);
        _logger.LogDebug("StorageClient: Updating drugInventoryRackAssignment with id: {Id}.", drugInventoryRackAssignment.Id);
        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetUpdateSql(drugInventoryRackAssignment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated drugInventoryRackAssignment with id: {Id}.", drugInventoryRackAssignment.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing drugInventoryRackAssignment with id: {Id}.", id);
        var sql = await DrugInventoryRackAssignmentDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed drugInventoryRackAssignment with id: {Id}.", id);
    }
}
