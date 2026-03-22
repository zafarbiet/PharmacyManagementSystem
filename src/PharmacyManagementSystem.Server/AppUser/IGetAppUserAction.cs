using PharmacyManagementSystem.Common.AppUser;

namespace PharmacyManagementSystem.Server.AppUser;

public interface IGetAppUserAction
{
    Task<IReadOnlyCollection<Common.AppUser.AppUser>?> GetByFilterCriteriaAsync(AppUserFilter filter, CancellationToken cancellationToken);
    Task<Common.AppUser.AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
