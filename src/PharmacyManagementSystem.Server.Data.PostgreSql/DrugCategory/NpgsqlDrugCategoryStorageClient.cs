using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugCategory;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DrugCategory;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugCategory;

public class NpgsqlDrugCategoryStorageClient(ILogger<NpgsqlDrugCategoryStorageClient> logger, INpgsqlDbClient dbClient) : IDrugCategoryStorageClient
{
    private readonly ILogger<NpgsqlDrugCategoryStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugCategory.DrugCategory>?> GetByFilterCriteriaAsync(DrugCategoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drug categories by filter criteria.");

        var sql = await DrugCategoryDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugCategory.DrugCategory>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drug categories.", list.Count);

        return list;
    }

    public async Task<Common.DrugCategory.DrugCategory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug category by id: {Id}.", id);

        var sql = await DrugCategoryDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugCategory.DrugCategory>(sql, cancellationToken).ConfigureAwait(false);

        var category = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug category with id: {Id}.", id);

        return category;
    }

    public async Task<Common.DrugCategory.DrugCategory?> AddAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        _logger.LogDebug("StorageClient: Adding drug category with name: {Name}.", category.Name);

        var sql = await DrugCategoryDatabaseCommandText.GetInsertSql(category).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugCategory.DrugCategory>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug category with name: {Name}.", category.Name);

        return inserted;
    }

    public async Task<Common.DrugCategory.DrugCategory?> UpdateAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        _logger.LogDebug("StorageClient: Updating drug category with id: {Id}.", category.Id);

        var sql = await DrugCategoryDatabaseCommandText.GetUpdateSql(category).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugCategory.DrugCategory>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug category with id: {Id}.", category.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug category with id: {Id}.", id);

        var sql = await DrugCategoryDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug category with id: {Id}.", id);
    }
}
