using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Unit.PurchaseOrderItem.Data;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrderItem;

[TestClass]
public class TestSavePurchaseOrderItemAction
{
    private readonly ILogger<SavePurchaseOrderItemAction> _logger;
    private readonly IPurchaseOrderItemRepository _repository;
    private readonly SavePurchaseOrderItemAction _action;

    public TestSavePurchaseOrderItemAction()
    {
        _logger = Substitute.For<ILogger<SavePurchaseOrderItemAction>>();
        _repository = Substitute.For<IPurchaseOrderItemRepository>();
        _action = new SavePurchaseOrderItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderItemActionData.ValidAddData), typeof(SavePurchaseOrderItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidPurchaseOrderItem_ReturnsSavedPurchaseOrderItem(Common.PurchaseOrderItem.PurchaseOrderItem input, Common.PurchaseOrderItem.PurchaseOrderItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.PurchaseOrderItem.PurchaseOrderItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PurchaseOrderId.Should().Be(expected.PurchaseOrderId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.PurchaseOrderItem.PurchaseOrderItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullPurchaseOrderItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderItemActionData.InvalidAddData), typeof(SavePurchaseOrderItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.PurchaseOrderItem.PurchaseOrderItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderItemActionData.ValidUpdateData), typeof(SavePurchaseOrderItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidPurchaseOrderItem_ReturnsUpdatedPurchaseOrderItem(Common.PurchaseOrderItem.PurchaseOrderItem input, Common.PurchaseOrderItem.PurchaseOrderItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.PurchaseOrderItem.PurchaseOrderItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PurchaseOrderId.Should().Be(expected.PurchaseOrderId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.PurchaseOrderItem.PurchaseOrderItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullPurchaseOrderItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task RemoveAsync_WhenValidId_CallsRepository()
    {
        // Arrange
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var updatedBy = "system";
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_WhenNullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
