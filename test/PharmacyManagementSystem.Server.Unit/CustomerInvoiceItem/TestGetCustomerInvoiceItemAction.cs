using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem;

[TestClass]
public class TestGetCustomerInvoiceItemAction
{
    private readonly ILogger<GetCustomerInvoiceItemAction> _logger;
    private readonly ICustomerInvoiceItemRepository _repository;
    private readonly GetCustomerInvoiceItemAction _action;

    public TestGetCustomerInvoiceItemAction()
    {
        _logger = Substitute.For<ILogger<GetCustomerInvoiceItemAction>>();
        _repository = Substitute.For<ICustomerInvoiceItemRepository>();
        _action = new GetCustomerInvoiceItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetCustomerInvoiceItemActionData.ValidFilterData), typeof(GetCustomerInvoiceItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(CustomerInvoiceItemFilter filter, List<Common.CustomerInvoiceItem.CustomerInvoiceItem> expected)
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
    [DynamicData(nameof(GetCustomerInvoiceItemActionData.ValidIdData), typeof(GetCustomerInvoiceItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsCustomerInvoiceItem(string id, Common.CustomerInvoiceItem.CustomerInvoiceItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.InvoiceId.Should().Be(expected.InvoiceId);
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
