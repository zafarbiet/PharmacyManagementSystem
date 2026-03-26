using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DailyDiaryEntry;
using PharmacyManagementSystem.Server.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Unit.DailyDiaryEntry.Data;

namespace PharmacyManagementSystem.Server.Unit.DailyDiaryEntry;

[TestClass]
public class TestGetDailyDiaryEntryAction
{
    private readonly ILogger<GetDailyDiaryEntryAction> _logger;
    private readonly IDailyDiaryEntryRepository _repository;
    private readonly GetDailyDiaryEntryAction _action;

    public TestGetDailyDiaryEntryAction()
    {
        _logger = Substitute.For<ILogger<GetDailyDiaryEntryAction>>();
        _repository = Substitute.For<IDailyDiaryEntryRepository>();
        _action = new GetDailyDiaryEntryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDailyDiaryEntryActionData.ValidFilterData), typeof(GetDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DailyDiaryEntryFilter filter, List<Common.DailyDiaryEntry.DailyDiaryEntry> expected)
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
    [DynamicData(nameof(GetDailyDiaryEntryActionData.ValidIdData), typeof(GetDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDailyDiaryEntry(string id, Common.DailyDiaryEntry.DailyDiaryEntry expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Title.Should().Be(expected.Title);
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
