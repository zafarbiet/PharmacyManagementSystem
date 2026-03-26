using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DamageRecord;
using PharmacyManagementSystem.Server.Unit.DamageRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DamageRecord;

[TestClass]
public class TestSaveDamageRecordAction
{
    private readonly ILogger<SaveDamageRecordAction> _logger;
    private readonly IDamageRecordRepository _repository;
    private readonly SaveDamageRecordAction _action;

    public TestSaveDamageRecordAction()
    {
        _logger = Substitute.For<ILogger<SaveDamageRecordAction>>();
        _repository = Substitute.For<IDamageRecordRepository>();
        _action = new SaveDamageRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageRecordActionData.ValidAddData), typeof(SaveDamageRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRecord_ReturnsSavedRecord(Common.DamageRecord.DamageRecord input, Common.DamageRecord.DamageRecord expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DamageRecord.DamageRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DamageType.Should().Be(expected.DamageType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DamageRecord.DamageRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageRecordActionData.InvalidAddData), typeof(SaveDamageRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DamageRecord.DamageRecord input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageRecordActionData.ValidUpdateData), typeof(SaveDamageRecordActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidRecord_ReturnsUpdatedRecord(Common.DamageRecord.DamageRecord input, Common.DamageRecord.DamageRecord expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DamageRecord.DamageRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DamageRecord.DamageRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageRecordActionData.ValidRemoveData), typeof(SaveDamageRecordActionData), DynamicDataSourceType.Method)]
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
