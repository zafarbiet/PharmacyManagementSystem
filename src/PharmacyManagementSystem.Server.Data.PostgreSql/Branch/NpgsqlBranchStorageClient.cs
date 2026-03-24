using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Branch;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.Branch;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Branch;

public class NpgsqlBranchStorageClient(ILogger<NpgsqlBranchStorageClient> logger, INpgsqlDbClient dbClient) : IBranchStorageClient
{
    private readonly ILogger<NpgsqlBranchStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Branch.Branch?>?> GetByFilterCriteriaAsync(BranchFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting branchs by filter criteria.");
        var sql = await BranchDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Branch.Branch>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} branchs.", list.Count);
        return list;
    }

    public async Task<Common.Branch.Branch?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting branch by id: {Id}.", id);
        var sql = await BranchDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Branch.Branch>(sql, cancellationToken).ConfigureAwait(false);
        var branch = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved branch with id: {Id}.", id);
        return branch;
    }

    public async Task<Common.Branch.Branch?> AddAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);
        _logger.LogDebug("StorageClient: Adding branch.");
        var sql = await BranchDatabaseCommandText.GetInsertSql(branch).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Branch.Branch>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added branch.");
        return inserted;
    }

    public async Task<Common.Branch.Branch?> UpdateAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);
        _logger.LogDebug("StorageClient: Updating branch with id: {Id}.", branch.Id);
        var sql = await BranchDatabaseCommandText.GetUpdateSql(branch).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Branch.Branch>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated branch with id: {Id}.", branch.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing branch with id: {Id}.", id);
        var sql = await BranchDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed branch with id: {Id}.", id);
    }
}
