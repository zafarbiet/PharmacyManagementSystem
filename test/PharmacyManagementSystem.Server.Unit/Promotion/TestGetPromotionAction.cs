using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Promotion;
using PharmacyManagementSystem.Server.Promotion;
using PharmacyManagementSystem.Server.Unit.Promotion.Data;

namespace PharmacyManagementSystem.Server.Unit.Promotion;

[TestClass]
public class TestGetPromotionAction
{
    private readonly ILogger<GetPromotionAction> _logger;
    private readonly IPromotionRepository _repository;
    private readonly GetPromotionAction _action;

    public TestGetPromotionAction()
    {
        _logger = Substitute.For<ILogger<GetPromotionAction>>();
        _repository = Substitute.For<IPromotionRepository>();
        _action = new GetPromotionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPromotionActionData.ValidFilterData), typeof(GetPromotionActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(PromotionFilter filter, List<Common.Promotion.Promotion> expected)
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
    [DynamicData(nameof(GetPromotionActionData.ValidIdData), typeof(GetPromotionActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsPromotion(string id, Common.Promotion.Promotion expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
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
