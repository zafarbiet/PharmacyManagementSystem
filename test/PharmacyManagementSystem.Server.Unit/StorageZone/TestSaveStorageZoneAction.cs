using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.StorageZone;
using PharmacyManagementSystem.Server.Unit.StorageZone.Data;

namespace PharmacyManagementSystem.Server.Unit.StorageZone;

[TestClass]
public class TestSaveStorageZoneAction
{
    private readonly ILogger<SaveStorageZoneAction> _logger;
    private readonly IStorageZoneRepository _repository;
    private readonly SaveStorageZoneAction _action;

    public TestSaveStorageZoneAction()
    {
        _logger = Substitute.For<ILogger<SaveStorageZoneAction>>();
        _repository = Substitute.For<IStorageZoneRepository>();
        _action = new SaveStorageZoneAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveStorageZoneActionData.ValidAddData), typeof(SaveStorageZoneActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidStorageZone_ReturnsSavedStorageZone(Common.StorageZone.StorageZone input, Common.StorageZone.StorageZone expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.StorageZone.StorageZone>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.StorageZone.StorageZone>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullStorageZone_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveStorageZoneActionData.InvalidAddData), typeof(SaveStorageZoneActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.StorageZone.StorageZone input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveStorageZoneActionData.ValidUpdateData), typeof(SaveStorageZoneActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidStorageZone_ReturnsUpdatedStorageZone(Common.StorageZone.StorageZone input, Common.StorageZone.StorageZone expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.StorageZone.StorageZone>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.StorageZone.StorageZone>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullStorageZone_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveStorageZoneActionData.ValidRemoveData), typeof(SaveStorageZoneActionData), DynamicDataSourceType.Method)]
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
