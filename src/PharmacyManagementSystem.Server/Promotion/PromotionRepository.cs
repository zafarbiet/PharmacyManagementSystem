using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Promotion;

namespace PharmacyManagementSystem.Server.Promotion;

public class PromotionRepository(ILogger<PromotionRepository> logger, IPromotionStorageClient storageClient) : IPromotionRepository
{
    private readonly ILogger<PromotionRepository> _logger = logger;
    private readonly IPromotionStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Promotion.Promotion>?> GetByFilterCriteriaAsync(PromotionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting promotions by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} promotions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Promotion.Promotion?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting promotion by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved promotion with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Promotion.Promotion?> AddAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        _logger.LogDebug("Repository: Adding promotion with name: {Name}.", promotion.Name);

        var result = await _storageClient.AddAsync(promotion, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added promotion with name: {Name}.", promotion.Name);

        return result;
    }

    public async Task<Common.Promotion.Promotion?> UpdateAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        _logger.LogDebug("Repository: Updating promotion with id: {Id}.", promotion.Id);

        var result = await _storageClient.UpdateAsync(promotion, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated promotion with id: {Id}.", promotion.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing promotion with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed promotion with id: {Id}.", id);
    }
}
