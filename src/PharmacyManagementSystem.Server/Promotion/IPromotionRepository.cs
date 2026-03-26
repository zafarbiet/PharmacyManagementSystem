using PharmacyManagementSystem.Common.Promotion;

namespace PharmacyManagementSystem.Server.Promotion;

public interface IPromotionRepository
{
    Task<IReadOnlyCollection<Common.Promotion.Promotion>?> GetByFilterCriteriaAsync(PromotionFilter filter, CancellationToken cancellationToken);
    Task<Common.Promotion.Promotion?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Promotion.Promotion?> AddAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken);
    Task<Common.Promotion.Promotion?> UpdateAsync(Common.Promotion.Promotion promotion, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
