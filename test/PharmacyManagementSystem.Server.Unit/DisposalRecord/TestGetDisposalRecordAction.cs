using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DisposalRecord;
using PharmacyManagementSystem.Server.DisposalRecord;
using PharmacyManagementSystem.Server.Unit.DisposalRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DisposalRecord;

[TestClass]
public class TestGetDisposalRecordAction
{
    private readonly ILogger<GetDisposalRecordAction> _logger;
    private readonly IDisposalRecordRepository _repository;
    private readonly GetDisposalRecordAction _action;

    public TestGetDisposalRecordAction()
    {
        _logger = Substitute.For<ILogger<GetDisposalRecordAction>>();
        _repository = Substitute.For<IDisposalRecordRepository>();
        _action = new GetDisposalRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDisposalRecordActionData.ValidFilterData), typeof(GetDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DisposalRecordFilter filter, List<Common.DisposalRecord.DisposalRecord> expected)
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
    [DynamicData(nameof(GetDisposalRecordActionData.ValidIdData), typeof(GetDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDisposalRecord(string id, Common.DisposalRecord.DisposalRecord expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DisposalMethod.Should().Be(expected.DisposalMethod);
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
