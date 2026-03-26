using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DebtReminder;
using PharmacyManagementSystem.Server.Unit.DebtReminder.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtReminder;

[TestClass]
public class TestSaveDebtReminderAction
{
    private readonly ILogger<SaveDebtReminderAction> _logger;
    private readonly IDebtReminderRepository _repository;
    private readonly SaveDebtReminderAction _action;

    public TestSaveDebtReminderAction()
    {
        _logger = Substitute.For<ILogger<SaveDebtReminderAction>>();
        _repository = Substitute.For<IDebtReminderRepository>();
        _action = new SaveDebtReminderAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtReminderActionData.ValidAddData), typeof(SaveDebtReminderActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidReminder_ReturnsSavedReminder(Common.DebtReminder.DebtReminder input, Common.DebtReminder.DebtReminder expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DebtReminder.DebtReminder>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Channel.Should().Be(expected.Channel);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DebtReminder.DebtReminder>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullReminder_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtReminderActionData.InvalidAddData), typeof(SaveDebtReminderActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DebtReminder.DebtReminder input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtReminderActionData.ValidUpdateData), typeof(SaveDebtReminderActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidReminder_ReturnsUpdatedReminder(Common.DebtReminder.DebtReminder input, Common.DebtReminder.DebtReminder expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DebtReminder.DebtReminder>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Channel.Should().Be(expected.Channel);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DebtReminder.DebtReminder>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullReminder_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtReminderActionData.ValidRemoveData), typeof(SaveDebtReminderActionData), DynamicDataSourceType.Method)]
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
