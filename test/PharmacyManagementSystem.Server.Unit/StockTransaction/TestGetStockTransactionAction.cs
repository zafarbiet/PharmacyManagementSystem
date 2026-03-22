using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.StockTransaction;
using PharmacyManagementSystem.Server.StockTransaction;
using PharmacyManagementSystem.Server.Unit.StockTransaction.Data;

namespace PharmacyManagementSystem.Server.Unit.StockTransaction;

[TestClass]
public class TestGetStockTransactionAction
{
    private readonly ILogger<GetStockTransactionAction> _logger;
    private readonly IStockTransactionRepository _repository;
    private readonly GetStockTransactionAction _action;

    public TestGetStockTransactionAction()
    {
        _logger = Substitute.For<ILogger<GetStockTransactionAction>>();
        _repository = Substitute.For<IStockTransactionRepository>();
        _action = new GetStockTransactionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetStockTransactionActionData.ValidFilterData), typeof(GetStockTransactionActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(StockTransactionFilter filter, List<Common.StockTransaction.StockTransaction> expected)
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
    [DynamicData(nameof(GetStockTransactionActionData.ValidIdData), typeof(GetStockTransactionActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsStockTransaction(string id, Common.StockTransaction.StockTransaction expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.TransactionType.Should().Be(expected.TransactionType);
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
