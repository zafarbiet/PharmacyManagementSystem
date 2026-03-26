using PharmacyManagementSystem.Common.Promotion;

namespace PharmacyManagementSystem.Server.Promotion;

public interface IGetPromotionAction
{
    Task<IReadOnlyCollection<Common.Promotion.Promotion>?> GetByFilterCriteriaAsync(PromotionFilter filter, CancellationToken cancellationToken);
    Task<Common.Promotion.Promotion?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
