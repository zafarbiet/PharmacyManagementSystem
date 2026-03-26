using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DebtReminder;
using PharmacyManagementSystem.Server.DebtReminder;
using PharmacyManagementSystem.Server.Unit.DebtReminder.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtReminder;

[TestClass]
public class TestGetDebtReminderAction
{
    private readonly ILogger<GetDebtReminderAction> _logger;
    private readonly IDebtReminderRepository _repository;
    private readonly GetDebtReminderAction _action;

    public TestGetDebtReminderAction()
    {
        _logger = Substitute.For<ILogger<GetDebtReminderAction>>();
        _repository = Substitute.For<IDebtReminderRepository>();
        _action = new GetDebtReminderAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDebtReminderActionData.ValidFilterData), typeof(GetDebtReminderActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DebtReminderFilter filter, List<Common.DebtReminder.DebtReminder> expected)
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
    [DynamicData(nameof(GetDebtReminderActionData.ValidIdData), typeof(GetDebtReminderActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDebtReminder(string id, Common.DebtReminder.DebtReminder expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Channel.Should().Be(expected.Channel);
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
