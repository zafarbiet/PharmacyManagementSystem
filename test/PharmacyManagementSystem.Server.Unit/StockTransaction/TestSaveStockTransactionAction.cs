using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.StockTransaction;
using PharmacyManagementSystem.Server.Unit.StockTransaction.Data;

namespace PharmacyManagementSystem.Server.Unit.StockTransaction;

[TestClass]
public class TestSaveStockTransactionAction
{
    private readonly ILogger<SaveStockTransactionAction> _logger;
    private readonly IStockTransactionRepository _repository;
    private readonly SaveStockTransactionAction _action;

    public TestSaveStockTransactionAction()
    {
        _logger = Substitute.For<ILogger<SaveStockTransactionAction>>();
        _repository = Substitute.For<IStockTransactionRepository>();
        _action = new SaveStockTransactionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveStockTransactionActionData.ValidAddData), typeof(SaveStockTransactionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidStockTransaction_ReturnsSavedStockTransaction(Common.StockTransaction.StockTransaction input, Common.StockTransaction.StockTransaction expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.StockTransaction.StockTransaction>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.TransactionType.Should().Be(expected.TransactionType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.StockTransaction.StockTransaction>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullStockTransaction_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveStockTransactionActionData.InvalidAddData), typeof(SaveStockTransactionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.StockTransaction.StockTransaction input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveStockTransactionActionData.ValidUpdateData), typeof(SaveStockTransactionActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidStockTransaction_ReturnsUpdatedStockTransaction(Common.StockTransaction.StockTransaction input, Common.StockTransaction.StockTransaction expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.StockTransaction.StockTransaction>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.TransactionType.Should().Be(expected.TransactionType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.StockTransaction.StockTransaction>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullStockTransaction_ThrowsArgumentNullException()
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
