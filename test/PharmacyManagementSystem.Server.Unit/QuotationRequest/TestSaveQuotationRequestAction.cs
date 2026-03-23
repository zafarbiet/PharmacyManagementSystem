using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.QuotationRequest;
using PharmacyManagementSystem.Server.Unit.QuotationRequest.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequest;

[TestClass]
public class TestSaveQuotationRequestAction
{
    private readonly ILogger<SaveQuotationRequestAction> _logger;
    private readonly IQuotationRequestRepository _repository;
    private readonly ISaveNotificationAction _notificationAction;
    private readonly SaveQuotationRequestAction _action;

    public TestSaveQuotationRequestAction()
    {
        _logger = Substitute.For<ILogger<SaveQuotationRequestAction>>();
        _repository = Substitute.For<IQuotationRequestRepository>();
        _notificationAction = Substitute.For<ISaveNotificationAction>();
        _action = new SaveQuotationRequestAction(_logger, _repository, _notificationAction);
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestActionData.ValidAddData), typeof(SaveQuotationRequestActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidQuotationRequest_ReturnsSaved(Common.QuotationRequest.QuotationRequest input, Common.QuotationRequest.QuotationRequest expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.QuotationRequest.QuotationRequest>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.QuotationRequest.QuotationRequest>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestActionData.InvalidAddData), typeof(SaveQuotationRequestActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.QuotationRequest.QuotationRequest input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestActionData.ValidUpdateData), typeof(SaveQuotationRequestActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidQuotationRequest_ReturnsUpdated(Common.QuotationRequest.QuotationRequest input, Common.QuotationRequest.QuotationRequest expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.QuotationRequest.QuotationRequest>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.QuotationRequest.QuotationRequest>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestActionData.ValidRemoveData), typeof(SaveQuotationRequestActionData), DynamicDataSourceType.Method)]
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
