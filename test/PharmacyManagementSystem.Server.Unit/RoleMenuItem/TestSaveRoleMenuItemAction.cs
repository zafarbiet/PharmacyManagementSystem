using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.RoleMenuItem;
using PharmacyManagementSystem.Server.Unit.RoleMenuItem.Data;

namespace PharmacyManagementSystem.Server.Unit.RoleMenuItem;

[TestClass]
public class TestSaveRoleMenuItemAction
{
    private readonly ILogger<SaveRoleMenuItemAction> _logger;
    private readonly IRoleMenuItemRepository _repository;
    private readonly SaveRoleMenuItemAction _action;

    public TestSaveRoleMenuItemAction()
    {
        _logger = Substitute.For<ILogger<SaveRoleMenuItemAction>>();
        _repository = Substitute.For<IRoleMenuItemRepository>();
        _action = new SaveRoleMenuItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleMenuItemActionData.ValidAddData), typeof(SaveRoleMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRoleMenuItem_ReturnsSavedRoleMenuItem(Common.RoleMenuItem.RoleMenuItem input, Common.RoleMenuItem.RoleMenuItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.RoleMenuItem.RoleMenuItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.RoleId.Should().Be(expected.RoleId);
        result.MenuItemId.Should().Be(expected.MenuItemId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.RoleMenuItem.RoleMenuItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullRoleMenuItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleMenuItemActionData.InvalidAddData), typeof(SaveRoleMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.RoleMenuItem.RoleMenuItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleMenuItemActionData.ValidRemoveData), typeof(SaveRoleMenuItemActionData), DynamicDataSourceType.Method)]
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
