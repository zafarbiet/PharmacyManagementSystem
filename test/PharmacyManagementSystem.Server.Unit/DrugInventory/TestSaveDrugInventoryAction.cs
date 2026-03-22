using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.Unit.DrugInventory.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugInventory;

[TestClass]
public class TestSaveDrugInventoryAction
{
    private readonly ILogger<SaveDrugInventoryAction> _logger;
    private readonly IDrugInventoryRepository _repository;
    private readonly SaveDrugInventoryAction _action;

    public TestSaveDrugInventoryAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugInventoryAction>>();
        _repository = Substitute.For<IDrugInventoryRepository>();
        _action = new SaveDrugInventoryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryActionData.ValidAddData), typeof(SaveDrugInventoryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidDrugInventory_ReturnsSavedDrugInventory(Common.DrugInventory.DrugInventory input, Common.DrugInventory.DrugInventory expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DrugInventory.DrugInventory>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.BatchNumber.Should().Be(expected.BatchNumber);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DrugInventory.DrugInventory>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullDrugInventory_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryActionData.InvalidAddData), typeof(SaveDrugInventoryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.DrugInventory.DrugInventory input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryActionData.ValidUpdateData), typeof(SaveDrugInventoryActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidDrugInventory_ReturnsUpdatedDrugInventory(Common.DrugInventory.DrugInventory input, Common.DrugInventory.DrugInventory expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DrugInventory.DrugInventory>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.BatchNumber.Should().Be(expected.BatchNumber);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DrugInventory.DrugInventory>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullDrugInventory_ThrowsArgumentNullException()
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
