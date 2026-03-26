using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DebtRecord;
using PharmacyManagementSystem.Server.DebtRecord;
using PharmacyManagementSystem.Server.Unit.DebtRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtRecord;

[TestClass]
public class TestGetDebtRecordAction
{
    private readonly ILogger<GetDebtRecordAction> _logger;
    private readonly IDebtRecordRepository _repository;
    private readonly GetDebtRecordAction _action;

    public TestGetDebtRecordAction()
    {
        _logger = Substitute.For<ILogger<GetDebtRecordAction>>();
        _repository = Substitute.For<IDebtRecordRepository>();
        _action = new GetDebtRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDebtRecordActionData.ValidFilterData), typeof(GetDebtRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DebtRecordFilter filter, List<Common.DebtRecord.DebtRecord> expected)
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
    [DynamicData(nameof(GetDebtRecordActionData.ValidIdData), typeof(GetDebtRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDebtRecord(string id, Common.DebtRecord.DebtRecord expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.OriginalAmount.Should().Be(expected.OriginalAmount);
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
