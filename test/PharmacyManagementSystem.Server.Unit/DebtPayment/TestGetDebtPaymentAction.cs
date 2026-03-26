using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DebtPayment;
using PharmacyManagementSystem.Server.DebtPayment;
using PharmacyManagementSystem.Server.Unit.DebtPayment.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtPayment;

[TestClass]
public class TestGetDebtPaymentAction
{
    private readonly ILogger<GetDebtPaymentAction> _logger;
    private readonly IDebtPaymentRepository _repository;
    private readonly GetDebtPaymentAction _action;

    public TestGetDebtPaymentAction()
    {
        _logger = Substitute.For<ILogger<GetDebtPaymentAction>>();
        _repository = Substitute.For<IDebtPaymentRepository>();
        _action = new GetDebtPaymentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDebtPaymentActionData.ValidFilterData), typeof(GetDebtPaymentActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DebtPaymentFilter filter, List<Common.DebtPayment.DebtPayment> expected)
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
    [DynamicData(nameof(GetDebtPaymentActionData.ValidIdData), typeof(GetDebtPaymentActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDebtPayment(string id, Common.DebtPayment.DebtPayment expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.AmountPaid.Should().Be(expected.AmountPaid);
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
}
