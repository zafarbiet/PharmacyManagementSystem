namespace PharmacyManagementSystem.Server.CustomerSubscription;

public interface ISaveCustomerSubscriptionAction
{
    Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
    Task<Common.CustomerSubscription.CustomerSubscription?> ApproveAsync(Guid id, string approvedBy, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>> ApproveBatchAsync(string approvedBy, CancellationToken cancellationToken);
}
