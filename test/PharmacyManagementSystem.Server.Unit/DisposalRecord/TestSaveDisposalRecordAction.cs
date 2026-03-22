using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DisposalRecord;
using PharmacyManagementSystem.Server.Unit.DisposalRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DisposalRecord;

[TestClass]
public class TestSaveDisposalRecordAction
{
    private readonly ILogger<SaveDisposalRecordAction> _logger;
    private readonly IDisposalRecordRepository _repository;
    private readonly SaveDisposalRecordAction _action;

    public TestSaveDisposalRecordAction()
    {
        _logger = Substitute.For<ILogger<SaveDisposalRecordAction>>();
        _repository = Substitute.For<IDisposalRecordRepository>();
        _action = new SaveDisposalRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDisposalRecordActionData.ValidAddData), typeof(SaveDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidDisposalRecord_ReturnsSavedDisposalRecord(Common.DisposalRecord.DisposalRecord input, Common.DisposalRecord.DisposalRecord expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DisposalRecord.DisposalRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DisposalMethod.Should().Be(expected.DisposalMethod);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DisposalRecord.DisposalRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullDisposalRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDisposalRecordActionData.InvalidAddData), typeof(SaveDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DisposalRecord.DisposalRecord input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDisposalRecordActionData.ValidUpdateData), typeof(SaveDisposalRecordActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidDisposalRecord_ReturnsUpdatedDisposalRecord(Common.DisposalRecord.DisposalRecord input, Common.DisposalRecord.DisposalRecord expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DisposalRecord.DisposalRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DisposalMethod.Should().Be(expected.DisposalMethod);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DisposalRecord.DisposalRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullDisposalRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyDisposalMethod_ThrowsBadRequestException()
    {
        // Arrange
        var record = new Common.DisposalRecord.DisposalRecord { Id = Guid.NewGuid(), ExpiryRecordId = Guid.NewGuid(), DisposalMethod = string.Empty, DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow };

        // Act
        var act = async () => await _action.UpdateAsync(record, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*DisposalMethod*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveDisposalRecordActionData.ValidRemoveData), typeof(SaveDisposalRecordActionData), DynamicDataSourceType.Method)]
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
