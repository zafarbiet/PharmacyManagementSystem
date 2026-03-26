using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem;

[TestClass]
public class TestSaveCustomerSubscriptionItemAction
{
    private readonly ILogger<SaveCustomerSubscriptionItemAction> _logger;
    private readonly ICustomerSubscriptionItemRepository _repository;
    private readonly SaveCustomerSubscriptionItemAction _action;

    public TestSaveCustomerSubscriptionItemAction()
    {
        _logger = Substitute.For<ILogger<SaveCustomerSubscriptionItemAction>>();
        _repository = Substitute.For<ICustomerSubscriptionItemRepository>();
        _action = new SaveCustomerSubscriptionItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionItemActionData.ValidAddData), typeof(SaveCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidItem_ReturnsSavedItem(Common.CustomerSubscriptionItem.CustomerSubscriptionItem input, Common.CustomerSubscriptionItem.CustomerSubscriptionItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.QuantityPerCycle.Should().Be(expected.QuantityPerCycle);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionItemActionData.InvalidAddData), typeof(SaveCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.CustomerSubscriptionItem.CustomerSubscriptionItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionItemActionData.ValidUpdateData), typeof(SaveCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidItem_ReturnsUpdatedItem(Common.CustomerSubscriptionItem.CustomerSubscriptionItem input, Common.CustomerSubscriptionItem.CustomerSubscriptionItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.QuantityPerCycle.Should().Be(expected.QuantityPerCycle);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionItemActionData.ValidRemoveData), typeof(SaveCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
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
