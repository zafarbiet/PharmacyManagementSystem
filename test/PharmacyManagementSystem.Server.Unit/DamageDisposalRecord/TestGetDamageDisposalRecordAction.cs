using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DamageDisposalRecord;
using PharmacyManagementSystem.Server.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Unit.DamageDisposalRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DamageDisposalRecord;

[TestClass]
public class TestGetDamageDisposalRecordAction
{
    private readonly ILogger<GetDamageDisposalRecordAction> _logger;
    private readonly IDamageDisposalRecordRepository _repository;
    private readonly GetDamageDisposalRecordAction _action;

    public TestGetDamageDisposalRecordAction()
    {
        _logger = Substitute.For<ILogger<GetDamageDisposalRecordAction>>();
        _repository = Substitute.For<IDamageDisposalRecordRepository>();
        _action = new GetDamageDisposalRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDamageDisposalRecordActionData.ValidFilterData), typeof(GetDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DamageDisposalRecordFilter filter, List<Common.DamageDisposalRecord.DamageDisposalRecord> expected)
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
    [DynamicData(nameof(GetDamageDisposalRecordActionData.ValidIdData), typeof(GetDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDamageDisposalRecord(string id, Common.DamageDisposalRecord.DamageDisposalRecord expected)
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
