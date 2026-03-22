using PharmacyManagementSystem.Common.CustomerSubscription;
namespace PharmacyManagementSystem.Server.CustomerSubscription;
public interface ICustomerSubscriptionStorageClient
{
    Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
