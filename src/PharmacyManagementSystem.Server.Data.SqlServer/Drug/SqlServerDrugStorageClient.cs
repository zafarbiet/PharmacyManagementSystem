using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Drug;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Drug;

public class SqlServerDrugStorageClient(ILogger<SqlServerDrugStorageClient> logger, ISqlServerDbClient dbClient) : IDrugStorageClient
{
    private readonly ILogger<SqlServerDrugStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Drug.Drug>?> GetByFilterCriteriaAsync(DrugFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drugs by filter criteria.");

        var sql = await DrugDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Drug.Drug>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drugs.", list.Count);

        return list;
    }

    public async Task<Common.Drug.Drug?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug by id: {Id}.", id);

        var sql = await DrugDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Drug.Drug>(sql, cancellationToken).ConfigureAwait(false);

        var drug = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug with id: {Id}.", id);

        return drug;
    }

    public async Task<Common.Drug.Drug?> AddAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        _logger.LogDebug("StorageClient: Adding drug with name: {Name}.", drug.Name);

        var sql = await DrugDatabaseCommandText.GetInsertSql(drug).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Drug.Drug>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug with name: {Name}.", drug.Name);

        return inserted;
    }

    public async Task<Common.Drug.Drug?> UpdateAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        _logger.LogDebug("StorageClient: Updating drug with id: {Id}.", drug.Id);

        var sql = await DrugDatabaseCommandText.GetUpdateSql(drug).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Drug.Drug>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug with id: {Id}.", drug.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug with id: {Id}.", id);

        var sql = await DrugDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug with id: {Id}.", id);
    }
}
