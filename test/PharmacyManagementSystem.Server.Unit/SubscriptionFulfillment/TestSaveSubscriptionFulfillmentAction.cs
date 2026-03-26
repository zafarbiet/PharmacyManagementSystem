using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.SubscriptionFulfillment;
using PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment.Data;

namespace PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment;

[TestClass]
public class TestSaveSubscriptionFulfillmentAction
{
    private readonly ILogger<SaveSubscriptionFulfillmentAction> _logger;
    private readonly ISubscriptionFulfillmentRepository _repository;
    private readonly SaveSubscriptionFulfillmentAction _action;

    public TestSaveSubscriptionFulfillmentAction()
    {
        _logger = Substitute.For<ILogger<SaveSubscriptionFulfillmentAction>>();
        _repository = Substitute.For<ISubscriptionFulfillmentRepository>();
        _action = new SaveSubscriptionFulfillmentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveSubscriptionFulfillmentActionData.ValidAddData), typeof(SaveSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidFulfillment_ReturnsSavedFulfillment(Common.SubscriptionFulfillment.SubscriptionFulfillment input, Common.SubscriptionFulfillment.SubscriptionFulfillment expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.SubscriptionFulfillment.SubscriptionFulfillment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.SubscriptionFulfillment.SubscriptionFulfillment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullFulfillment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveSubscriptionFulfillmentActionData.InvalidAddData), typeof(SaveSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.SubscriptionFulfillment.SubscriptionFulfillment input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveSubscriptionFulfillmentActionData.ValidUpdateData), typeof(SaveSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidFulfillment_ReturnsUpdatedFulfillment(Common.SubscriptionFulfillment.SubscriptionFulfillment input, Common.SubscriptionFulfillment.SubscriptionFulfillment expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.SubscriptionFulfillment.SubscriptionFulfillment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.SubscriptionFulfillment.SubscriptionFulfillment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullFulfillment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveSubscriptionFulfillmentActionData.ValidRemoveData), typeof(SaveSubscriptionFulfillmentActionData), DynamicDataSourceType.Method)]
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
