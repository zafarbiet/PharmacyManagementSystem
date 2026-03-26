using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DamageRecord;
using PharmacyManagementSystem.Server.DamageRecord;
using PharmacyManagementSystem.Server.Unit.DamageRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DamageRecord;

[TestClass]
public class TestGetDamageRecordAction
{
    private readonly ILogger<GetDamageRecordAction> _logger;
    private readonly IDamageRecordRepository _repository;
    private readonly GetDamageRecordAction _action;

    public TestGetDamageRecordAction()
    {
        _logger = Substitute.For<ILogger<GetDamageRecordAction>>();
        _repository = Substitute.For<IDamageRecordRepository>();
        _action = new GetDamageRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDamageRecordActionData.ValidFilterData), typeof(GetDamageRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DamageRecordFilter filter, List<Common.DamageRecord.DamageRecord> expected)
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
    [DynamicData(nameof(GetDamageRecordActionData.ValidIdData), typeof(GetDamageRecordActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDamageRecord(string id, Common.DamageRecord.DamageRecord expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DamageType.Should().Be(expected.DamageType);
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
