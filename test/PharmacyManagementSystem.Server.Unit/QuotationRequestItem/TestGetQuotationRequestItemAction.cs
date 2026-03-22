using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.QuotationRequestItem;
using PharmacyManagementSystem.Server.QuotationRequestItem;
using PharmacyManagementSystem.Server.Unit.QuotationRequestItem.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequestItem;

[TestClass]
public class TestGetQuotationRequestItemAction
{
    private readonly ILogger<GetQuotationRequestItemAction> _logger;
    private readonly IQuotationRequestItemRepository _repository;
    private readonly GetQuotationRequestItemAction _action;

    public TestGetQuotationRequestItemAction()
    {
        _logger = Substitute.For<ILogger<GetQuotationRequestItemAction>>();
        _repository = Substitute.For<IQuotationRequestItemRepository>();
        _action = new GetQuotationRequestItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetQuotationRequestItemActionData.ValidFilterData), typeof(GetQuotationRequestItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(QuotationRequestItemFilter filter, List<Common.QuotationRequestItem.QuotationRequestItem> expected)
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
    [DynamicData(nameof(GetQuotationRequestItemActionData.ValidIdData), typeof(GetQuotationRequestItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsQuotationRequestItem(string id, Common.QuotationRequestItem.QuotationRequestItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
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
