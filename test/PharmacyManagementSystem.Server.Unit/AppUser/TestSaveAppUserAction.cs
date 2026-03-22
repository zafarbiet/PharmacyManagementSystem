using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.Unit.AppUser.Data;

namespace PharmacyManagementSystem.Server.Unit.AppUser;

[TestClass]
public class TestSaveAppUserAction
{
    private readonly ILogger<SaveAppUserAction> _logger;
    private readonly IAppUserRepository _repository;
    private readonly SaveAppUserAction _action;

    public TestSaveAppUserAction()
    {
        _logger = Substitute.For<ILogger<SaveAppUserAction>>();
        _repository = Substitute.For<IAppUserRepository>();
        _action = new SaveAppUserAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveAppUserActionData.ValidAddData), typeof(SaveAppUserActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidAppUser_ReturnsSavedAppUser(Common.AppUser.AppUser input, Common.AppUser.AppUser expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.AppUser.AppUser>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be(expected.Username);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.AppUser.AppUser>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullAppUser_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveAppUserActionData.InvalidAddData), typeof(SaveAppUserActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.AppUser.AppUser input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveAppUserActionData.ValidUpdateData), typeof(SaveAppUserActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidAppUser_ReturnsUpdatedAppUser(Common.AppUser.AppUser input, Common.AppUser.AppUser expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.AppUser.AppUser>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Username.Should().Be(expected.Username);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.AppUser.AppUser>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullAppUser_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyUsername_ThrowsBadRequestException()
    {
        // Arrange
        var appUser = new Common.AppUser.AppUser { Id = Guid.NewGuid(), Username = string.Empty, FullName = "John Doe", PasswordHash = "hash" };

        // Act
        var act = async () => await _action.UpdateAsync(appUser, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Username*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveAppUserActionData.ValidRemoveData), typeof(SaveAppUserActionData), DynamicDataSourceType.Method)]
    public async Task RemoveAsync_ValidId_CallsRepository(Guid id, string updatedBy)
    {
        // Arrange
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_NullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
