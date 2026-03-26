using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.MenuItem;
using PharmacyManagementSystem.Server.Unit.MenuItem.Data;

namespace PharmacyManagementSystem.Server.Unit.MenuItem;

[TestClass]
public class TestSaveMenuItemAction
{
    private readonly ILogger<SaveMenuItemAction> _logger;
    private readonly IMenuItemRepository _repository;
    private readonly SaveMenuItemAction _action;

    public TestSaveMenuItemAction()
    {
        _logger = Substitute.For<ILogger<SaveMenuItemAction>>();
        _repository = Substitute.For<IMenuItemRepository>();
        _action = new SaveMenuItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveMenuItemActionData.ValidAddData), typeof(SaveMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidMenuItem_ReturnsSavedMenuItem(Common.MenuItem.MenuItem input, Common.MenuItem.MenuItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.MenuItem.MenuItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Key.Should().Be(expected.Key);
        result.Label.Should().Be(expected.Label);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.MenuItem.MenuItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullMenuItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveMenuItemActionData.InvalidAddData), typeof(SaveMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.MenuItem.MenuItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveMenuItemActionData.ValidUpdateData), typeof(SaveMenuItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidMenuItem_ReturnsUpdatedMenuItem(Common.MenuItem.MenuItem input, Common.MenuItem.MenuItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.MenuItem.MenuItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Label.Should().Be(expected.Label);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.MenuItem.MenuItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullMenuItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveMenuItemActionData.ValidRemoveData), typeof(SaveMenuItemActionData), DynamicDataSourceType.Method)]
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
