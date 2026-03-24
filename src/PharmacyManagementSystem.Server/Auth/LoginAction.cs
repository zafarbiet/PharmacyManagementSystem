using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Auth;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.AppUser;

namespace PharmacyManagementSystem.Server.Auth;

public class LoginAction(ILogger<LoginAction> logger, IAppUserStorageClient storageClient) : ILoginAction
{
    private readonly ILogger<LoginAction> _logger = logger;
    private readonly IAppUserStorageClient _storageClient = storageClient;

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Username))
            throw new BadRequestException("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new BadRequestException("Password is required.");

        _logger.LogDebug("Login attempt for username: {Username}.", request.Username);

        var user = await _storageClient.GetByUsernameAsync(request.Username, cancellationToken).ConfigureAwait(false);

        if (user is null || !user.IsActive)
        {
            _logger.LogDebug("Login failed: user not found or inactive for username: {Username}.", request.Username);
            return null;
        }

        if (user.IsLocked)
        {
            _logger.LogDebug("Login failed: account locked for username: {Username}.", request.Username);
            throw new BadRequestException("Account is locked. Please contact your administrator.");
        }

        var hash = ComputeHash(request.Username, request.Password);

        if (!string.Equals(hash, user.PasswordHash, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("Login failed: incorrect password for username: {Username}.", request.Username);
            return null;
        }

        // Token is the user's ID — backend is currently unauthenticated; frontend uses this
        // as an opaque credential to pass the RequireAuth guard.
        var token = user.Id.ToString();

        _logger.LogDebug("Login succeeded for username: {Username}.", request.Username);

        return new LoginResponse { Token = token, User = user };
    }

    /// <summary>
    /// Computes SHA-256 of "username:password" and returns uppercase hex.
    /// Must match the format stored in PMS.Users.PasswordHash by the seed migration.
    /// </summary>
    internal static string ComputeHash(string username, string password)
    {
        var input = Encoding.UTF8.GetBytes($"{username}:{password}");
        var bytes = SHA256.HashData(input);
        return Convert.ToHexString(bytes); // uppercase hex, e.g. "A1B2C3..."
    }
}
