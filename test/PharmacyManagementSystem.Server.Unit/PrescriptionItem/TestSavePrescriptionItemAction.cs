using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.PrescriptionItem;
using PharmacyManagementSystem.Server.Unit.PrescriptionItem.Data;

namespace PharmacyManagementSystem.Server.Unit.PrescriptionItem;

[TestClass]
public class TestSavePrescriptionItemAction
{
    private readonly ILogger<SavePrescriptionItemAction> _logger;
    private readonly IPrescriptionItemRepository _repository;
    private readonly SavePrescriptionItemAction _action;

    public TestSavePrescriptionItemAction()
    {
        _logger = Substitute.For<ILogger<SavePrescriptionItemAction>>();
        _repository = Substitute.For<IPrescriptionItemRepository>();
        _action = new SavePrescriptionItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionItemActionData.ValidAddData), typeof(SavePrescriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidPrescriptionItem_ReturnsSavedPrescriptionItem(Common.PrescriptionItem.PrescriptionItem input, Common.PrescriptionItem.PrescriptionItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.PrescriptionItem.PrescriptionItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PrescriptionId.Should().Be(expected.PrescriptionId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.PrescriptionItem.PrescriptionItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullPrescriptionItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionItemActionData.InvalidAddData), typeof(SavePrescriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.PrescriptionItem.PrescriptionItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionItemActionData.ValidUpdateData), typeof(SavePrescriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidPrescriptionItem_ReturnsUpdatedPrescriptionItem(Common.PrescriptionItem.PrescriptionItem input, Common.PrescriptionItem.PrescriptionItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.PrescriptionItem.PrescriptionItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PrescriptionId.Should().Be(expected.PrescriptionId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.PrescriptionItem.PrescriptionItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullPrescriptionItem_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task RemoveAsync_WhenValidId_CallsRepository()
    {
        // Arrange
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var updatedBy = "system";
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_WhenNullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
