using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Unit.DailyDiaryEntry.Data;

namespace PharmacyManagementSystem.Server.Unit.DailyDiaryEntry;

[TestClass]
public class TestSaveDailyDiaryEntryAction
{
    private readonly ILogger<SaveDailyDiaryEntryAction> _logger;
    private readonly IDailyDiaryEntryRepository _repository;
    private readonly SaveDailyDiaryEntryAction _action;

    public TestSaveDailyDiaryEntryAction()
    {
        _logger = Substitute.For<ILogger<SaveDailyDiaryEntryAction>>();
        _repository = Substitute.For<IDailyDiaryEntryRepository>();
        _action = new SaveDailyDiaryEntryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDailyDiaryEntryActionData.ValidAddData), typeof(SaveDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidEntry_ReturnsSavedEntry(Common.DailyDiaryEntry.DailyDiaryEntry input, Common.DailyDiaryEntry.DailyDiaryEntry expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DailyDiaryEntry.DailyDiaryEntry>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be(expected.Title);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DailyDiaryEntry.DailyDiaryEntry>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullEntry_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDailyDiaryEntryActionData.InvalidAddData), typeof(SaveDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DailyDiaryEntry.DailyDiaryEntry input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDailyDiaryEntryActionData.ValidUpdateData), typeof(SaveDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidEntry_ReturnsUpdatedEntry(Common.DailyDiaryEntry.DailyDiaryEntry input, Common.DailyDiaryEntry.DailyDiaryEntry expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DailyDiaryEntry.DailyDiaryEntry>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Title.Should().Be(expected.Title);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DailyDiaryEntry.DailyDiaryEntry>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullEntry_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyCategory_ThrowsBadRequestException()
    {
        // Arrange
        var entry = new Common.DailyDiaryEntry.DailyDiaryEntry { Id = Guid.NewGuid(), Category = string.Empty, Title = "T", Body = "B", CreatedBy = "admin" };

        // Act
        var act = async () => await _action.UpdateAsync(entry, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Category*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveDailyDiaryEntryActionData.ValidRemoveData), typeof(SaveDailyDiaryEntryActionData), DynamicDataSourceType.Method)]
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
