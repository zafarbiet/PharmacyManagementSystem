using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Promotion;

public class SavePromotionAction(ILogger<SavePromotionAction> logger, IPromotionRepository repository) : ISavePromotionAction
{
    private readonly ILogger<SavePromotionAction> _logger = logger;
    private readonly IPromotionRepository _repository = repository;

    public async Task<Common.Promotion.Promotion?> AddAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        if (string.IsNullOrWhiteSpace(promotion.Name))
            throw new BadRequestException("Promotion Name is required.");

        if (promotion.DiscountPercentage <= 0 || promotion.DiscountPercentage > 100)
            throw new BadRequestException("DiscountPercentage must be between 0 and 100.");

        if (promotion.ValidTo <= promotion.ValidFrom)
            throw new BadRequestException("ValidTo must be after ValidFrom.");

        promotion.UpdatedBy = "system";

        _logger.LogDebug("Adding new promotion with name: {Name}.", promotion.Name);

        var result = await _repository.AddAsync(promotion, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added promotion with name: {Name}.", promotion.Name);

        return result;
    }

    public async Task<Common.Promotion.Promotion?> UpdateAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        if (string.IsNullOrWhiteSpace(promotion.Name))
            throw new BadRequestException("Promotion Name is required.");

        if (promotion.DiscountPercentage <= 0 || promotion.DiscountPercentage > 100)
            throw new BadRequestException("DiscountPercentage must be between 0 and 100.");

        if (promotion.ValidTo <= promotion.ValidFrom)
            throw new BadRequestException("ValidTo must be after ValidFrom.");

        promotion.UpdatedBy = "system";

        _logger.LogDebug("Updating promotion with id: {Id}.", promotion.Id);

        var result = await _repository.UpdateAsync(promotion, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated promotion with id: {Id}.", promotion.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing promotion with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed promotion with id: {Id}.", id);
    }
}
