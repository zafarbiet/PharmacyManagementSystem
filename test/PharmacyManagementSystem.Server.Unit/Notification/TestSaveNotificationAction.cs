using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.Unit.Notification.Data;

namespace PharmacyManagementSystem.Server.Unit.Notification;

[TestClass]
public class TestSaveNotificationAction
{
    private readonly ILogger<SaveNotificationAction> _logger;
    private readonly INotificationRepository _repository;
    private readonly SaveNotificationAction _action;

    public TestSaveNotificationAction()
    {
        _logger = Substitute.For<ILogger<SaveNotificationAction>>();
        _repository = Substitute.For<INotificationRepository>();
        _action = new SaveNotificationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveNotificationActionData.ValidAddData), typeof(SaveNotificationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidNotification_ReturnsSavedNotification(Common.Notification.Notification input, Common.Notification.Notification expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Notification.Notification>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.NotificationType.Should().Be(expected.NotificationType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Notification.Notification>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullNotification_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveNotificationActionData.InvalidAddData), typeof(SaveNotificationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Notification.Notification input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveNotificationActionData.ValidUpdateData), typeof(SaveNotificationActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidNotification_ReturnsUpdatedNotification(Common.Notification.Notification input, Common.Notification.Notification expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Notification.Notification>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Notification.Notification>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullNotification_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveNotificationActionData.ValidRemoveData), typeof(SaveNotificationActionData), DynamicDataSourceType.Method)]
    public async Task RemoveAsync_ValidId_CallsRepository(Guid id, string updatedBy)
    {
        // Arrange
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_NullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
