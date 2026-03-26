using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Server.CustomerSubscription;
using PharmacyManagementSystem.Server.Unit.CustomerSubscription.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscription;

[TestClass]
public class TestGetCustomerSubscriptionAction
{
    private readonly ILogger<GetCustomerSubscriptionAction> _logger;
    private readonly ICustomerSubscriptionRepository _repository;
    private readonly GetCustomerSubscriptionAction _action;

    public TestGetCustomerSubscriptionAction()
    {
        _logger = Substitute.For<ILogger<GetCustomerSubscriptionAction>>();
        _repository = Substitute.For<ICustomerSubscriptionRepository>();
        _action = new GetCustomerSubscriptionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetCustomerSubscriptionActionData.ValidFilterData), typeof(GetCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(CustomerSubscriptionFilter filter, List<Common.CustomerSubscription.CustomerSubscription> expected)
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
    [DynamicData(nameof(GetCustomerSubscriptionActionData.ValidIdData), typeof(GetCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsCustomerSubscription(string id, Common.CustomerSubscription.CustomerSubscription expected)
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
