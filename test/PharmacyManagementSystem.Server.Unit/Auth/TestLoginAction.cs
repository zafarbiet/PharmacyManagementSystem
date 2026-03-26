using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Auth;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.Auth;
using PharmacyManagementSystem.Server.Unit.Auth.Data;

namespace PharmacyManagementSystem.Server.Unit.Auth;

[TestClass]
public class TestLoginAction
{
    private readonly ILogger<LoginAction> _logger;
    private readonly IAppUserStorageClient _storageClient;
    private readonly LoginAction _action;

    public TestLoginAction()
    {
        _logger = Substitute.For<ILogger<LoginAction>>();
        _storageClient = Substitute.For<IAppUserStorageClient>();
        _action = new LoginAction(_logger, _storageClient);
    }

    [TestMethod]
    [DynamicData(nameof(LoginActionData.ValidCredentialsData), typeof(LoginActionData), DynamicDataSourceType.Method)]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse(LoginRequest request, Common.AppUser.AppUser user)
    {
        // Arrange
        _storageClient.GetByUsernameAsync(request.Username!, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().Be(user.Id.ToString());
        result.User.Should().NotBeNull();
        result.User!.Username.Should().Be(user.Username);
    }

    [TestMethod]
    public async Task LoginAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.LoginAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task LoginAsync_EmptyUsername_ThrowsBadRequestException()
    {
        // Arrange
        var request = new LoginRequest { Username = string.Empty, Password = "Admin@123" };

        // Act
        var act = async () => await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Username*");
    }

    [TestMethod]
    public async Task LoginAsync_EmptyPassword_ThrowsBadRequestException()
    {
        // Arrange
        var request = new LoginRequest { Username = "admin", Password = string.Empty };

        // Act
        var act = async () => await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Password*");
    }

    [TestMethod]
    public async Task LoginAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        var request = new LoginRequest { Username = "nonexistent", Password = "password" };
        _storageClient.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns((Common.AppUser.AppUser?)null);

        // Act
        var result = await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task LoginAsync_InactiveUser_ReturnsNull()
    {
        // Arrange
        var request = new LoginRequest { Username = "inactive", Password = "password" };
        _storageClient.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(new Common.AppUser.AppUser { Username = "inactive", IsActive = false });

        // Act
        var result = await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task LoginAsync_LockedAccount_ThrowsBadRequestException()
    {
        // Arrange
        var request = new LoginRequest { Username = "locked", Password = "password" };
        _storageClient.GetByUsernameAsync(request.Username, Arg.Any<CancellationToken>())
            .Returns(new Common.AppUser.AppUser { Username = "locked", IsActive = true, IsLocked = true });

        // Act
        var act = async () => await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*locked*");
    }

    [TestMethod]
    [DynamicData(nameof(LoginActionData.InvalidCredentialsData), typeof(LoginActionData), DynamicDataSourceType.Method)]
    public async Task LoginAsync_WrongPassword_ReturnsNull(LoginRequest request, Common.AppUser.AppUser user)
    {
        // Arrange
        _storageClient.GetByUsernameAsync(request.Username!, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _action.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void ComputeHash_SameInputs_ReturnsSameHash()
    {
        // Act
        var hash1 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin:Admin@123")));
        var hash2 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin:Admin@123")));

        // Assert
        hash1.Should().Be(hash2);
    }

    [TestMethod]
    public void ComputeHash_DifferentPasswords_ReturnsDifferentHashes()
    {
        // Act
        var hash1 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin:Admin@123")));
        var hash2 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin:OtherPassword")));

        // Assert
        hash1.Should().NotBe(hash2);
    }
}
