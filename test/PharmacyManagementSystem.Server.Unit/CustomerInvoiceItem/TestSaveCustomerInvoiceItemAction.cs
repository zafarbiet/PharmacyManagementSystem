using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem;

[TestClass]
public class TestSaveCustomerInvoiceItemAction
{
    private readonly ILogger<SaveCustomerInvoiceItemAction> _logger;
    private readonly ICustomerInvoiceItemRepository _repository;
    private readonly SaveCustomerInvoiceItemAction _action;

    public TestSaveCustomerInvoiceItemAction()
    {
        _logger = Substitute.For<ILogger<SaveCustomerInvoiceItemAction>>();
        _repository = Substitute.For<ICustomerInvoiceItemRepository>();
        _action = new SaveCustomerInvoiceItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceItemActionData.ValidAddData), typeof(SaveCustomerInvoiceItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidCustomerInvoiceItem_ReturnsSavedCustomerInvoiceItem(Common.CustomerInvoiceItem.CustomerInvoiceItem input, Common.CustomerInvoiceItem.CustomerInvoiceItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.CustomerInvoiceItem.CustomerInvoiceItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.InvoiceId.Should().Be(expected.InvoiceId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.CustomerInvoiceItem.CustomerInvoiceItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullCustomerInvoiceItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceItemActionData.InvalidAddData), typeof(SaveCustomerInvoiceItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.CustomerInvoiceItem.CustomerInvoiceItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerInvoiceItemActionData.ValidUpdateData), typeof(SaveCustomerInvoiceItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidCustomerInvoiceItem_ReturnsUpdatedCustomerInvoiceItem(Common.CustomerInvoiceItem.CustomerInvoiceItem input, Common.CustomerInvoiceItem.CustomerInvoiceItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.CustomerInvoiceItem.CustomerInvoiceItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.InvoiceId.Should().Be(expected.InvoiceId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.CustomerInvoiceItem.CustomerInvoiceItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullCustomerInvoiceItem_ThrowsArgumentNullException()
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
