using PharmacyManagementSystem.Common.AppUser;

namespace PharmacyManagementSystem.Server.AppUser;

public interface IAppUserRepository
{
    Task<IReadOnlyCollection<Common.AppUser.AppUser>?> GetByFilterCriteriaAsync(AppUserFilter filter, CancellationToken cancellationToken);
    Task<Common.AppUser.AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.AppUser.AppUser?> AddAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken);
    Task<Common.AppUser.AppUser?> UpdateAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
