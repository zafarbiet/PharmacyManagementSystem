using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Promotion;
using PharmacyManagementSystem.Server.Promotion;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Promotion;

public class NpgsqlPromotionStorageClient(ILogger<NpgsqlPromotionStorageClient> logger, INpgsqlDbClient dbClient) : IPromotionStorageClient
{
    private readonly ILogger<NpgsqlPromotionStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Promotion.Promotion>?> GetByFilterCriteriaAsync(PromotionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting promotions by filter criteria.");

        var sql = await PromotionDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Promotion.Promotion>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} promotions.", list.Count);

        return list;
    }

    public async Task<Common.Promotion.Promotion?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting promotion by id: {Id}.", id);

        var sql = await PromotionDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Promotion.Promotion>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.Promotion.Promotion?> AddAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        _logger.LogDebug("StorageClient: Adding promotion with name: {Name}.", promotion.Name);

        var sql = await PromotionDatabaseCommandText.GetInsertSql(promotion).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Promotion.Promotion>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.Promotion.Promotion?> UpdateAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        _logger.LogDebug("StorageClient: Updating promotion with id: {Id}.", promotion.Id);

        var sql = await PromotionDatabaseCommandText.GetUpdateSql(promotion).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Promotion.Promotion>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing promotion with id: {Id}.", id);

        var sql = await PromotionDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
