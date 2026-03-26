using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DebtPayment;
using PharmacyManagementSystem.Server.Unit.DebtPayment.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtPayment;

[TestClass]
public class TestSaveDebtPaymentAction
{
    private readonly ILogger<SaveDebtPaymentAction> _logger;
    private readonly IDebtPaymentRepository _repository;
    private readonly SaveDebtPaymentAction _action;

    public TestSaveDebtPaymentAction()
    {
        _logger = Substitute.For<ILogger<SaveDebtPaymentAction>>();
        _repository = Substitute.For<IDebtPaymentRepository>();
        _action = new SaveDebtPaymentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtPaymentActionData.ValidAddData), typeof(SaveDebtPaymentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidPayment_ReturnsSavedPayment(Common.DebtPayment.DebtPayment input, Common.DebtPayment.DebtPayment expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DebtPayment.DebtPayment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.AmountPaid.Should().Be(expected.AmountPaid);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DebtPayment.DebtPayment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullPayment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtPaymentActionData.InvalidAddData), typeof(SaveDebtPaymentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DebtPayment.DebtPayment input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtPaymentActionData.ValidUpdateData), typeof(SaveDebtPaymentActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidPayment_ReturnsUpdatedPayment(Common.DebtPayment.DebtPayment input, Common.DebtPayment.DebtPayment expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DebtPayment.DebtPayment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.AmountPaid.Should().Be(expected.AmountPaid);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DebtPayment.DebtPayment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullPayment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtPaymentActionData.ValidRemoveData), typeof(SaveDebtPaymentActionData), DynamicDataSourceType.Method)]
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
