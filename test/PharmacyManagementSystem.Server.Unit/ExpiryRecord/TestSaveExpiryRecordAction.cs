using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.ExpiryRecord;
using PharmacyManagementSystem.Server.Unit.ExpiryRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.ExpiryRecord;

[TestClass]
public class TestSaveExpiryRecordAction
{
    private readonly ILogger<SaveExpiryRecordAction> _logger;
    private readonly IExpiryRecordRepository _repository;
    private readonly SaveExpiryRecordAction _action;

    public TestSaveExpiryRecordAction()
    {
        _logger = Substitute.For<ILogger<SaveExpiryRecordAction>>();
        _repository = Substitute.For<IExpiryRecordRepository>();
        _action = new SaveExpiryRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryRecordActionData.ValidAddData), typeof(SaveExpiryRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidExpiryRecord_ReturnsSavedExpiryRecord(Common.ExpiryRecord.ExpiryRecord input, Common.ExpiryRecord.ExpiryRecord expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.ExpiryRecord.ExpiryRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.ExpiryRecord.ExpiryRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullExpiryRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryRecordActionData.InvalidAddData), typeof(SaveExpiryRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.ExpiryRecord.ExpiryRecord input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryRecordActionData.ValidUpdateData), typeof(SaveExpiryRecordActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidExpiryRecord_ReturnsUpdatedExpiryRecord(Common.ExpiryRecord.ExpiryRecord input, Common.ExpiryRecord.ExpiryRecord expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.ExpiryRecord.ExpiryRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.ExpiryRecord.ExpiryRecord>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullExpiryRecord_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyStatus_ThrowsBadRequestException()
    {
        // Arrange
        var record = new Common.ExpiryRecord.ExpiryRecord { Id = Guid.NewGuid(), DrugInventoryId = Guid.NewGuid(), Status = string.Empty, InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) };

        // Act
        var act = async () => await _action.UpdateAsync(record, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Status*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryRecordActionData.ValidRemoveData), typeof(SaveExpiryRecordActionData), DynamicDataSourceType.Method)]
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
