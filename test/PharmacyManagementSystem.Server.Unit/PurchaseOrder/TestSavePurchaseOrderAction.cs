using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.Unit.PurchaseOrder.Data;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrder;

[TestClass]
public class TestSavePurchaseOrderAction
{
    private readonly ILogger<SavePurchaseOrderAction> _logger;
    private readonly IPurchaseOrderRepository _repository;
    private readonly SavePurchaseOrderAction _action;

    public TestSavePurchaseOrderAction()
    {
        _logger = Substitute.For<ILogger<SavePurchaseOrderAction>>();
        _repository = Substitute.For<IPurchaseOrderRepository>();
        _action = new SavePurchaseOrderAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderActionData.ValidAddData), typeof(SavePurchaseOrderActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidPurchaseOrder_ReturnsSavedPurchaseOrder(Common.PurchaseOrder.PurchaseOrder input, Common.PurchaseOrder.PurchaseOrder expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.PurchaseOrder.PurchaseOrder>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.VendorId.Should().Be(expected.VendorId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.PurchaseOrder.PurchaseOrder>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullPurchaseOrder_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderActionData.InvalidAddData), typeof(SavePurchaseOrderActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.PurchaseOrder.PurchaseOrder input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePurchaseOrderActionData.ValidUpdateData), typeof(SavePurchaseOrderActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidPurchaseOrder_ReturnsUpdatedPurchaseOrder(Common.PurchaseOrder.PurchaseOrder input, Common.PurchaseOrder.PurchaseOrder expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.PurchaseOrder.PurchaseOrder>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.VendorId.Should().Be(expected.VendorId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.PurchaseOrder.PurchaseOrder>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullPurchaseOrder_ThrowsArgumentNullException()
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
