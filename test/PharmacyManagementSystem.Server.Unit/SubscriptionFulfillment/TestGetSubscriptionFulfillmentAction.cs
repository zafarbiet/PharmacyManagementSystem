using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment.Data;

namespace PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment;

[TestClass]
public class TestGetSubscriptionFulfillmentAction
{
    private readonly ILogger<GetSubscriptionFulfillmentAction> _logger;
    private readonly ISubscriptionFulfillmentRepository _repository;
    private readonly GetSubscriptionFulfillmentAction _action;

    public TestGetSubscriptionFulfillmentAction()
    {
        _logger = Substitute.For<ILogger<GetSubscriptionFulfillmentAction>>();
        _repository = Substitute.For<ISubscriptionFulfillmentRepository>();
        _action = new GetSubscriptionFulfillmentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetSubscriptionFulfillmentActionData.ValidFilterData), typeof(GetSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(SubscriptionFulfillmentFilter filter, List<Common.SubscriptionFulfillment.SubscriptionFulfillment> expected)
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
    [DynamicData(nameof(GetSubscriptionFulfillmentActionData.ValidIdData), typeof(GetSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsFulfillment(string id, Common.SubscriptionFulfillment.SubscriptionFulfillment expected)
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
    public async Task GetByIdAsync_NullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
