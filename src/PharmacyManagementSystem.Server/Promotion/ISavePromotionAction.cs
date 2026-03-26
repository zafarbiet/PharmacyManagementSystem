namespace PharmacyManagementSystem.Server.Promotion;

public interface ISavePromotionAction
{
    Task<Common.Promotion.Promotion?> AddAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken);
    Task<Common.Promotion.Promotion?> UpdateAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
