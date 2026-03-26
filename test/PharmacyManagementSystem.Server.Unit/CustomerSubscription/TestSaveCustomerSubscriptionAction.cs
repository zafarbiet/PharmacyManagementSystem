using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.CustomerSubscription;
using PharmacyManagementSystem.Server.Unit.CustomerSubscription.Data;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscription;

[TestClass]
public class TestSaveCustomerSubscriptionAction
{
    private readonly ILogger<SaveCustomerSubscriptionAction> _logger;
    private readonly ICustomerSubscriptionRepository _repository;
    private readonly SaveCustomerSubscriptionAction _action;

    public TestSaveCustomerSubscriptionAction()
    {
        _logger = Substitute.For<ILogger<SaveCustomerSubscriptionAction>>();
        _repository = Substitute.For<ICustomerSubscriptionRepository>();
        _action = new SaveCustomerSubscriptionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionActionData.ValidAddData), typeof(SaveCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidSubscription_ReturnsSavedSubscription(Common.CustomerSubscription.CustomerSubscription input, Common.CustomerSubscription.CustomerSubscription expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.CustomerSubscription.CustomerSubscription>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.CustomerSubscription.CustomerSubscription>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullSubscription_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionActionData.InvalidAddData), typeof(SaveCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.CustomerSubscription.CustomerSubscription input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionActionData.ValidUpdateData), typeof(SaveCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidSubscription_ReturnsUpdatedSubscription(Common.CustomerSubscription.CustomerSubscription input, Common.CustomerSubscription.CustomerSubscription expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.CustomerSubscription.CustomerSubscription>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.CustomerSubscription.CustomerSubscription>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullSubscription_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionActionData.ValidRemoveData), typeof(SaveCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
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

    [TestMethod]
    [DynamicData(nameof(SaveCustomerSubscriptionActionData.ValidApproveData), typeof(SaveCustomerSubscriptionActionData), DynamicDataSourceType.Method)]
    public async Task ApproveAsync_PendingSubscription_SetsApprovedStatus(Guid id, string approvedBy, Common.CustomerSubscription.CustomerSubscription subscription)
    {
        // Arrange
        _repository.GetByIdAsync(id.ToString(), Arg.Any<CancellationToken>())
            .Returns(subscription);
        _repository.UpdateAsync(Arg.Any<Common.CustomerSubscription.CustomerSubscription>(), Arg.Any<CancellationToken>())
            .Returns(subscription);

        // Act
        var result = await _action.ApproveAsync(id, approvedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).UpdateAsync(
            Arg.Is<Common.CustomerSubscription.CustomerSubscription>(s => s.ApprovalStatus == "Approved" && s.ApprovedBy == approvedBy),
            Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task ApproveAsync_AlreadyApproved_ThrowsConflictException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subscription = new Common.CustomerSubscription.CustomerSubscription
        {
            Id = id, PatientId = Guid.NewGuid(), CycleDayOfMonth = 15,
            Status = "Active", ApprovalStatus = "Approved", IsActive = true
        };
        _repository.GetByIdAsync(id.ToString(), Arg.Any<CancellationToken>())
            .Returns(subscription);

        // Act
        var act = async () => await _action.ApproveAsync(id, "manager", CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }

    [TestMethod]
    public async Task ApproveAsync_NullApprovedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.ApproveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task ApproveBatchAsync_WithPendingSubscriptions_ApprovesAll()
    {
        // Arrange
        var pending = new List<Common.CustomerSubscription.CustomerSubscription>
        {
            new() { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), CycleDayOfMonth = 15, Status = "Active", ApprovalStatus = "Pending", IsActive = true },
            new() { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), CycleDayOfMonth = 1, Status = "Active", ApprovalStatus = "Pending", IsActive = true }
        };

        _repository.GetByFilterCriteriaAsync(
            Arg.Is<CustomerSubscriptionFilter>(f => f.ApprovalStatus == "Pending"),
            Arg.Any<CancellationToken>())
            .Returns(pending.AsReadOnly());

        foreach (var sub in pending)
        {
            var captured = sub;
            _repository.UpdateAsync(Arg.Is<Common.CustomerSubscription.CustomerSubscription>(s => s.Id == captured.Id), Arg.Any<CancellationToken>())
                .Returns(captured);
        }

        // Act
        var result = await _action.ApproveBatchAsync("manager", CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        await _repository.Received(2).UpdateAsync(
            Arg.Is<Common.CustomerSubscription.CustomerSubscription>(s => s.ApprovalStatus == "Approved"),
            Arg.Any<CancellationToken>());
    }
}
