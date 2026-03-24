using PharmacyManagementSystem.Common.Auth;

namespace PharmacyManagementSystem.Server.Auth;

public interface ILoginAction
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
