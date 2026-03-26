using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.Unit.PaymentLedger.Data;
using PharmacyManagementSystem.Server.Vendor;

namespace PharmacyManagementSystem.Server.Unit.PaymentLedger;

[TestClass]
public class TestSavePaymentLedgerAction
{
    private readonly ILogger<SavePaymentLedgerAction> _logger;
    private readonly IPaymentLedgerRepository _repository;
    private readonly IVendorRepository _vendorRepository;
    private readonly SavePaymentLedgerAction _action;

    public TestSavePaymentLedgerAction()
    {
        _logger = Substitute.For<ILogger<SavePaymentLedgerAction>>();
        _repository = Substitute.For<IPaymentLedgerRepository>();
        _vendorRepository = Substitute.For<IVendorRepository>();
        _action = new SavePaymentLedgerAction(_logger, _repository, _vendorRepository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePaymentLedgerActionData.ValidAddData), typeof(SavePaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidLedger_ReturnsSavedLedger(Common.PaymentLedger.PaymentLedger input, Common.PaymentLedger.PaymentLedger expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.InvoicedAmount.Should().Be(expected.InvoicedAmount);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullLedger_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePaymentLedgerActionData.InvalidAddData), typeof(SavePaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.PaymentLedger.PaymentLedger input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePaymentLedgerActionData.ValidUpdateData), typeof(SavePaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidLedger_ReturnsUpdatedLedger(Common.PaymentLedger.PaymentLedger input, Common.PaymentLedger.PaymentLedger expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullLedger_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePaymentLedgerActionData.ValidRemoveData), typeof(SavePaymentLedgerActionData), DynamicDataSourceType.Method)]
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

    [TestMethod]
    [DynamicData(nameof(SavePaymentLedgerActionData.RecordPaymentData), typeof(SavePaymentLedgerActionData), DynamicDataSourceType.Method)]
    public async Task RecordPaymentAsync_PartialPayment_SetsPartiallyPaidStatus(Guid ledgerId, decimal paymentAmount, Common.PaymentLedger.PaymentLedger ledger, Common.Vendor.Vendor vendor)
    {
        // Arrange
        _repository.GetByIdAsync(ledgerId.ToString(), Arg.Any<CancellationToken>())
            .Returns(ledger);
        _repository.UpdateAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>())
            .Returns(ledger);
        _vendorRepository.GetByIdAsync(ledger.VendorId.ToString(), Arg.Any<CancellationToken>())
            .Returns(vendor);
        _vendorRepository.UpdateAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>())
            .Returns(vendor);

        // Act
        await _action.RecordPaymentAsync(ledgerId, paymentAmount, CancellationToken.None);

        // Assert
        await _repository.Received(1).UpdateAsync(
            Arg.Is<Common.PaymentLedger.PaymentLedger>(l => l.Status == "PartiallyPaid" && l.PaidAmount == paymentAmount),
            Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RecordPaymentAsync_AlreadyPaid_ThrowsConflictException()
    {
        // Arrange
        var ledgerId = Guid.NewGuid();
        var paidLedger = new Common.PaymentLedger.PaymentLedger { Id = ledgerId, VendorId = Guid.NewGuid(), InvoicedAmount = 50000m, PaidAmount = 50000m, Status = "Paid" };
        _repository.GetByIdAsync(ledgerId.ToString(), Arg.Any<CancellationToken>())
            .Returns(paidLedger);

        // Act
        var act = async () => await _action.RecordPaymentAsync(ledgerId, 1000m, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }

    [TestMethod]
    public async Task RecordPaymentAsync_ZeroAmount_ThrowsBadRequestException()
    {
        // Act
        var act = async () => await _action.RecordPaymentAsync(Guid.NewGuid(), 0m, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    public async Task RecordPaymentAsync_FullPayment_SetsPaidStatus()
    {
        // Arrange
        var ledgerId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        var ledger = new Common.PaymentLedger.PaymentLedger { Id = ledgerId, VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid" };
        var vendor = new Common.Vendor.Vendor { Id = vendorId, Name = "MedSupply", OutstandingBalance = 50000m };

        _repository.GetByIdAsync(ledgerId.ToString(), Arg.Any<CancellationToken>()).Returns(ledger);
        _repository.UpdateAsync(Arg.Any<Common.PaymentLedger.PaymentLedger>(), Arg.Any<CancellationToken>()).Returns(ledger);
        _vendorRepository.GetByIdAsync(vendorId.ToString(), Arg.Any<CancellationToken>()).Returns(vendor);
        _vendorRepository.UpdateAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>()).Returns(vendor);

        // Act
        await _action.RecordPaymentAsync(ledgerId, 50000m, CancellationToken.None);

        // Assert
        await _repository.Received(1).UpdateAsync(
            Arg.Is<Common.PaymentLedger.PaymentLedger>(l => l.Status == "Paid"),
            Arg.Any<CancellationToken>());
    }
}
