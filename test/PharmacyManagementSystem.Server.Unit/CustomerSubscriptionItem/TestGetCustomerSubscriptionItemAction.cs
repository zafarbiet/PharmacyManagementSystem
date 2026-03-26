using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.CustomerSubscriptionItem;
using PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem;

[TestClass]
public class TestGetCustomerSubscriptionItemAction
{
    private readonly ILogger<GetCustomerSubscriptionItemAction> _logger;
    private readonly ICustomerSubscriptionItemRepository _repository;
    private readonly GetCustomerSubscriptionItemAction _action;

    public TestGetCustomerSubscriptionItemAction()
    {
        _logger = Substitute.For<ILogger<GetCustomerSubscriptionItemAction>>();
        _repository = Substitute.For<ICustomerSubscriptionItemRepository>();
        _action = new GetCustomerSubscriptionItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetCustomerSubscriptionItemActionData.ValidFilterData), typeof(GetCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(CustomerSubscriptionItemFilter filter, List<Common.CustomerSubscriptionItem.CustomerSubscriptionItem> expected)
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
    [DynamicData(nameof(GetCustomerSubscriptionItemActionData.ValidIdData), typeof(GetCustomerSubscriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsItem(string id, Common.CustomerSubscriptionItem.CustomerSubscriptionItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.QuantityPerCycle.Should().Be(expected.QuantityPerCycle);
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
