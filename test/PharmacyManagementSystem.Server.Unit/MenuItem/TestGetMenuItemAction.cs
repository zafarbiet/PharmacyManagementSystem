using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.MenuItem;
using PharmacyManagementSystem.Server.MenuItem;
using PharmacyManagementSystem.Server.Unit.MenuItem.Data;

namespace PharmacyManagementSystem.Server.Unit.MenuItem;

[TestClass]
public class TestGetMenuItemAction
{
    private readonly ILogger<GetMenuItemAction> _logger;
    private readonly IMenuItemRepository _repository;
    private readonly GetMenuItemAction _action;

    public TestGetMenuItemAction()
    {
        _logger = Substitute.For<ILogger<GetMenuItemAction>>();
        _repository = Substitute.For<IMenuItemRepository>();
        _action = new GetMenuItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetMenuItemActionData.ValidFilterData), typeof(GetMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(MenuItemFilter filter, List<Common.MenuItem.MenuItem> expected)
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
    [DynamicData(nameof(GetMenuItemActionData.ValidIdData), typeof(GetMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsMenuItem(string id, Common.MenuItem.MenuItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Label.Should().Be(expected.Label);
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

    [TestMethod]
    [DynamicData(nameof(GetMenuItemActionData.ValidUsernameData), typeof(GetMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task GetForUserAsync_ValidUsername_ReturnsMenuItems(string username, List<Common.MenuItem.MenuItem> expected)
    {
        // Arrange
        _repository.GetForUserAsync(username, Arg.Any<CancellationToken>())
            .Returns(expected.AsReadOnly());

        // Act
        var result = await _action.GetForUserAsync(username, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expected.Count);
        await _repository.Received(1).GetForUserAsync(username, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetForUserAsync_NullUsername_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetForUserAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
