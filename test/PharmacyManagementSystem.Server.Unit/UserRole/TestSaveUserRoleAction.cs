using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.UserRole;
using PharmacyManagementSystem.Server.Unit.UserRole.Data;

namespace PharmacyManagementSystem.Server.Unit.UserRole;

[TestClass]
public class TestSaveUserRoleAction
{
    private readonly ILogger<SaveUserRoleAction> _logger;
    private readonly IUserRoleRepository _repository;
    private readonly SaveUserRoleAction _action;

    public TestSaveUserRoleAction()
    {
        _logger = Substitute.For<ILogger<SaveUserRoleAction>>();
        _repository = Substitute.For<IUserRoleRepository>();
        _action = new SaveUserRoleAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveUserRoleActionData.ValidAddData), typeof(SaveUserRoleActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidUserRole_ReturnsSavedUserRole(Common.UserRole.UserRole input, Common.UserRole.UserRole expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.UserRole.UserRole>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(expected.UserId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.UserRole.UserRole>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullUserRole_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveUserRoleActionData.InvalidAddData), typeof(SaveUserRoleActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.UserRole.UserRole input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveUserRoleActionData.ValidUpdateData), typeof(SaveUserRoleActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidUserRole_ReturnsUpdatedUserRole(Common.UserRole.UserRole input, Common.UserRole.UserRole expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.UserRole.UserRole>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UserId.Should().Be(expected.UserId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.UserRole.UserRole>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullUserRole_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveUserRoleActionData.ValidRemoveData), typeof(SaveUserRoleActionData), DynamicDataSourceType.Method)]
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
