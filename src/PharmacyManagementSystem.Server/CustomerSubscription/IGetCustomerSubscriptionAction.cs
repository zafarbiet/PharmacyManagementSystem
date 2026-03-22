using PharmacyManagementSystem.Common.CustomerSubscription;
namespace PharmacyManagementSystem.Server.CustomerSubscription;
public interface IGetCustomerSubscriptionAction
{
    Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
