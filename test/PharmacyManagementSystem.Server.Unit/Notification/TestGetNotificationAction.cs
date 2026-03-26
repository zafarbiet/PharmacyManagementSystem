using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Notification;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.Unit.Notification.Data;

namespace PharmacyManagementSystem.Server.Unit.Notification;

[TestClass]
public class TestGetNotificationAction
{
    private readonly ILogger<GetNotificationAction> _logger;
    private readonly INotificationRepository _repository;
    private readonly GetNotificationAction _action;

    public TestGetNotificationAction()
    {
        _logger = Substitute.For<ILogger<GetNotificationAction>>();
        _repository = Substitute.For<INotificationRepository>();
        _action = new GetNotificationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetNotificationActionData.ValidFilterData), typeof(GetNotificationActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(NotificationFilter filter, List<Common.Notification.Notification> expected)
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
    [DynamicData(nameof(GetNotificationActionData.ValidIdData), typeof(GetNotificationActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsNotification(string id, Common.Notification.Notification expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.NotificationType.Should().Be(expected.NotificationType);
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
