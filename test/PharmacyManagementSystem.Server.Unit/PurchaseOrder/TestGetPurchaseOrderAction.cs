using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.PurchaseOrder;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.Unit.PurchaseOrder.Data;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrder;

[TestClass]
public class TestGetPurchaseOrderAction
{
    private readonly ILogger<GetPurchaseOrderAction> _logger;
    private readonly IPurchaseOrderRepository _repository;
    private readonly GetPurchaseOrderAction _action;

    public TestGetPurchaseOrderAction()
    {
        _logger = Substitute.For<ILogger<GetPurchaseOrderAction>>();
        _repository = Substitute.For<IPurchaseOrderRepository>();
        _action = new GetPurchaseOrderAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPurchaseOrderActionData.ValidFilterData), typeof(GetPurchaseOrderActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(PurchaseOrderFilter filter, List<Common.PurchaseOrder.PurchaseOrder> expected)
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
    [DynamicData(nameof(GetPurchaseOrderActionData.ValidIdData), typeof(GetPurchaseOrderActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsPurchaseOrder(string id, Common.PurchaseOrder.PurchaseOrder expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.VendorId.Should().Be(expected.VendorId);
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
