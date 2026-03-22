using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.CustomerInvoice;
using PharmacyManagementSystem.Server.Unit.CustomerInvoice.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoice;

[TestClass]
public class TestSaveCustomerInvoiceAction
{
    private readonly ILogger<SaveCustomerInvoiceAction> _logger;
    private readonly ICustomerInvoiceRepository _repository;
    private readonly SaveCustomerInvoiceAction _action;

    public TestSaveCustomerInvoiceAction()
    {
        _logger = Substitute.For<ILogger<SaveCustomerInvoiceAction>>();
        _repository = Substitute.For<ICustomerInvoiceRepository>();
        _action = new SaveCustomerInvoiceAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceActionData.ValidAddData), typeof(SaveCustomerInvoiceActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidCustomerInvoice_ReturnsSavedCustomerInvoice(Common.CustomerInvoice.CustomerInvoice input, Common.CustomerInvoice.CustomerInvoice expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.CustomerInvoice.CustomerInvoice>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.CustomerInvoice.CustomerInvoice>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullCustomerInvoice_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceActionData.InvalidAddData), typeof(SaveCustomerInvoiceActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.CustomerInvoice.CustomerInvoice input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceActionData.ValidUpdateData), typeof(SaveCustomerInvoiceActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidCustomerInvoice_ReturnsUpdatedCustomerInvoice(Common.CustomerInvoice.CustomerInvoice input, Common.CustomerInvoice.CustomerInvoice expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.CustomerInvoice.CustomerInvoice>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.CustomerInvoice.CustomerInvoice>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullCustomerInvoice_ThrowsArgumentNullException()
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
