using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DebtRecord;
using PharmacyManagementSystem.Server.Unit.DebtRecord.Data;

namespace PharmacyManagementSystem.Server.Unit.DebtRecord;

[TestClass]
public class TestSaveDebtRecordAction
{
    private readonly ILogger<SaveDebtRecordAction> _logger;
    private readonly IDebtRecordRepository _repository;
    private readonly SaveDebtRecordAction _action;

    public TestSaveDebtRecordAction()
    {
        _logger = Substitute.For<ILogger<SaveDebtRecordAction>>();
        _repository = Substitute.For<IDebtRecordRepository>();
        _action = new SaveDebtRecordAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtRecordActionData.ValidAddData), typeof(SaveDebtRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRecord_ReturnsSavedRecord(Common.DebtRecord.DebtRecord input, Common.DebtRecord.DebtRecord expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DebtRecord.DebtRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.OriginalAmount.Should().Be(expected.OriginalAmount);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DebtRecord.DebtRecord>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveDebtRecordActionData.InvalidAddData), typeof(SaveDebtRecordActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DebtRecord.DebtRecord input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDebtRecordActionData.ValidUpdateData), typeof(SaveDebtRecordActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidRecord_ReturnsUpdatedRecord(Common.DebtRecord.DebtRecord input, Common.DebtRecord.DebtRecord expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DebtRecord.DebtRecord>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DebtRecord.DebtRecord>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveDebtRecordActionData.ValidRemoveData), typeof(SaveDebtRecordActionData), DynamicDataSourceType.Method)]
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
