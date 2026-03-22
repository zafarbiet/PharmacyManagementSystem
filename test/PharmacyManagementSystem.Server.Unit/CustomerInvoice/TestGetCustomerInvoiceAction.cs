using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.CustomerInvoice;
using PharmacyManagementSystem.Server.CustomerInvoice;
using PharmacyManagementSystem.Server.Unit.CustomerInvoice.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoice;

[TestClass]
public class TestGetCustomerInvoiceAction
{
    private readonly ILogger<GetCustomerInvoiceAction> _logger;
    private readonly ICustomerInvoiceRepository _repository;
    private readonly GetCustomerInvoiceAction _action;

    public TestGetCustomerInvoiceAction()
    {
        _logger = Substitute.For<ILogger<GetCustomerInvoiceAction>>();
        _repository = Substitute.For<ICustomerInvoiceRepository>();
        _action = new GetCustomerInvoiceAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetCustomerInvoiceActionData.ValidFilterData), typeof(GetCustomerInvoiceActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(CustomerInvoiceFilter filter, List<Common.CustomerInvoice.CustomerInvoice> expected)
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
    [DynamicData(nameof(GetCustomerInvoiceActionData.ValidIdData), typeof(GetCustomerInvoiceActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsCustomerInvoice(string id, Common.CustomerInvoice.CustomerInvoice expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
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
