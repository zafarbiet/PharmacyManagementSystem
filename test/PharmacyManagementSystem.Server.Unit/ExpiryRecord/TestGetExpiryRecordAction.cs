using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.ExpiryRecord;
using PharmacyManagementSystem.Server.ExpiryRecord;
using PharmacyManagementSystem.Server.Unit.ExpiryRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.ExpiryRecord;

[TestClass]
public class TestGetExpiryRecordAction
{
    private readonly ILogger<GetExpiryRecordAction> _logger;
    private readonly IExpiryRecordRepository _repository;
    private readonly GetExpiryRecordAction _action;

    public TestGetExpiryRecordAction()
    {
        _logger = Substitute.For<ILogger<GetExpiryRecordAction>>();
        _repository = Substitute.For<IExpiryRecordRepository>();
        _action = new GetExpiryRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetExpiryRecordActionData.ValidFilterData), typeof(GetExpiryRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(ExpiryRecordFilter filter, List<Common.ExpiryRecord.ExpiryRecord> expected)
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
    [DynamicData(nameof(GetExpiryRecordActionData.ValidIdData), typeof(GetExpiryRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsExpiryRecord(string id, Common.ExpiryRecord.ExpiryRecord expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
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
