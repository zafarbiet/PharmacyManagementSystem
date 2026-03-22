using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.PurchaseOrderItem;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Unit.PurchaseOrderItem.Data;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrderItem;

[TestClass]
public class TestGetPurchaseOrderItemAction
{
    private readonly ILogger<GetPurchaseOrderItemAction> _logger;
    private readonly IPurchaseOrderItemRepository _repository;
    private readonly GetPurchaseOrderItemAction _action;

    public TestGetPurchaseOrderItemAction()
    {
        _logger = Substitute.For<ILogger<GetPurchaseOrderItemAction>>();
        _repository = Substitute.For<IPurchaseOrderItemRepository>();
        _action = new GetPurchaseOrderItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPurchaseOrderItemActionData.ValidFilterData), typeof(GetPurchaseOrderItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(PurchaseOrderItemFilter filter, List<Common.PurchaseOrderItem.PurchaseOrderItem> expected)
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
    public async Task GetByFilterCriteriaAsync_WhenNullFilter_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByFilterCriteriaAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(GetPurchaseOrderItemActionData.ValidIdData), typeof(GetPurchaseOrderItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsPurchaseOrderItem(string id, Common.PurchaseOrderItem.PurchaseOrderItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PurchaseOrderId.Should().Be(expected.PurchaseOrderId);
        await _repository.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenNullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
