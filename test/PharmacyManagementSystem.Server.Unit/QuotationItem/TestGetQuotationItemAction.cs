using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.QuotationItem;
using PharmacyManagementSystem.Server.QuotationItem;
using PharmacyManagementSystem.Server.Unit.QuotationItem.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationItem;

[TestClass]
public class TestGetQuotationItemAction
{
    private readonly ILogger<GetQuotationItemAction> _logger;
    private readonly IQuotationItemRepository _repository;
    private readonly GetQuotationItemAction _action;

    public TestGetQuotationItemAction()
    {
        _logger = Substitute.For<ILogger<GetQuotationItemAction>>();
        _repository = Substitute.For<IQuotationItemRepository>();
        _action = new GetQuotationItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetQuotationItemActionData.ValidFilterData), typeof(GetQuotationItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(QuotationItemFilter filter, List<Common.QuotationItem.QuotationItem> expected)
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
    [DynamicData(nameof(GetQuotationItemActionData.ValidIdData), typeof(GetQuotationItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsQuotationItem(string id, Common.QuotationItem.QuotationItem expected)
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
