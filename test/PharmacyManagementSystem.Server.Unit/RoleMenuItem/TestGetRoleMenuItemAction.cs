using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.RoleMenuItem;
using PharmacyManagementSystem.Server.RoleMenuItem;
using PharmacyManagementSystem.Server.Unit.RoleMenuItem.Data;

namespace PharmacyManagementSystem.Server.Unit.RoleMenuItem;

[TestClass]
public class TestGetRoleMenuItemAction
{
    private readonly ILogger<GetRoleMenuItemAction> _logger;
    private readonly IRoleMenuItemRepository _repository;
    private readonly GetRoleMenuItemAction _action;

    public TestGetRoleMenuItemAction()
    {
        _logger = Substitute.For<ILogger<GetRoleMenuItemAction>>();
        _repository = Substitute.For<IRoleMenuItemRepository>();
        _action = new GetRoleMenuItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetRoleMenuItemActionData.ValidFilterData), typeof(GetRoleMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(RoleMenuItemFilter filter, List<Common.RoleMenuItem.RoleMenuItem> expected)
    {
        // Arrange
        _repository.GetByFilterCriteriaAsync(filter, Arg.Any<CancellationToken>())
            .Returns(expected.AsReadOnly());

        // Act
        var result = await _action.GetByFilterCriteriaAsync(filter, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expected.Count);
        await _repository.Received(1).GetByFilterCriteriaAsync(filter, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetByFilterCriteriaAsync_NullFilter_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByFilterCriteriaAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(GetRoleMenuItemActionData.ValidIdData), typeof(GetRoleMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsRoleMenuItem(string id, Common.RoleMenuItem.RoleMenuItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.RoleId.Should().Be(expected.RoleId);
        await _repository.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetByIdAsync_NullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
