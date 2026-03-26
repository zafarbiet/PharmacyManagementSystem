using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.PaymentLedger;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.Unit.PaymentLedger.Data;

namespace PharmacyManagementSystem.Server.Unit.PaymentLedger;

[TestClass]
public class TestGetPaymentLedgerAction
{
    private readonly ILogger<GetPaymentLedgerAction> _logger;
    private readonly IPaymentLedgerRepository _repository;
    private readonly GetPaymentLedgerAction _action;

    public TestGetPaymentLedgerAction()
    {
        _logger = Substitute.For<ILogger<GetPaymentLedgerAction>>();
        _repository = Substitute.For<IPaymentLedgerRepository>();
        _action = new GetPaymentLedgerAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPaymentLedgerActionData.ValidFilterData), typeof(GetPaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(PaymentLedgerFilter filter, List<Common.PaymentLedger.PaymentLedger> expected)
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
    [DynamicData(nameof(GetPaymentLedgerActionData.ValidIdData), typeof(GetPaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsPaymentLedger(string id, Common.PaymentLedger.PaymentLedger expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.InvoicedAmount.Should().Be(expected.InvoicedAmount);
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
