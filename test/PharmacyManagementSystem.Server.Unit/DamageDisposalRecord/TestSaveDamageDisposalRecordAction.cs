using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Unit.DamageDisposalRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DamageDisposalRecord;

[TestClass]
public class TestSaveDamageDisposalRecordAction
{
    private readonly ILogger<SaveDamageDisposalRecordAction> _logger;
    private readonly IDamageDisposalRecordRepository _repository;
    private readonly SaveDamageDisposalRecordAction _action;

    public TestSaveDamageDisposalRecordAction()
    {
        _logger = Substitute.For<ILogger<SaveDamageDisposalRecordAction>>();
        _repository = Substitute.For<IDamageDisposalRecordRepository>();
        _action = new SaveDamageDisposalRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageDisposalRecordActionData.ValidAddData), typeof(SaveDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRecord_ReturnsSavedRecord(Common.DamageDisposalRecord.DamageDisposalRecord input, Common.DamageDisposalRecord.DamageDisposalRecord expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DamageDisposalRecord.DamageDisposalRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DisposalMethod.Should().Be(expected.DisposalMethod);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DamageDisposalRecord.DamageDisposalRecord>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveDamageDisposalRecordActionData.InvalidAddData), typeof(SaveDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DamageDisposalRecord.DamageDisposalRecord input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDamageDisposalRecordActionData.ValidUpdateData), typeof(SaveDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidRecord_ReturnsUpdatedRecord(Common.DamageDisposalRecord.DamageDisposalRecord input, Common.DamageDisposalRecord.DamageDisposalRecord expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DamageDisposalRecord.DamageDisposalRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DisposalMethod.Should().Be(expected.DisposalMethod);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DamageDisposalRecord.DamageDisposalRecord>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveDamageDisposalRecordActionData.ValidRemoveData), typeof(SaveDamageDisposalRecordActionData), DynamicDataSourceType.Method)]
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
