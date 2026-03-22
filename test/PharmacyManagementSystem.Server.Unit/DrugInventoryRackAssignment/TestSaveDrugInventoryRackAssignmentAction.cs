using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment;

[TestClass]
public class TestSaveDrugInventoryRackAssignmentAction
{
    private readonly ILogger<SaveDrugInventoryRackAssignmentAction> _logger;
    private readonly IDrugInventoryRackAssignmentRepository _repository;
    private readonly SaveDrugInventoryRackAssignmentAction _action;

    public TestSaveDrugInventoryRackAssignmentAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugInventoryRackAssignmentAction>>();
        _repository = Substitute.For<IDrugInventoryRackAssignmentRepository>();
        _action = new SaveDrugInventoryRackAssignmentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryRackAssignmentActionData.ValidAddData), typeof(SaveDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidDrugInventoryRackAssignment_ReturnsSaved(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment input, Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DrugInventoryId.Should().Be(expected.DrugInventoryId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullDrugInventoryRackAssignment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryRackAssignmentActionData.InvalidAddData), typeof(SaveDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryRackAssignmentActionData.ValidUpdateData), typeof(SaveDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidDrugInventoryRackAssignment_ReturnsUpdated(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment input, Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.QuantityPlaced.Should().Be(expected.QuantityPlaced);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullDrugInventoryRackAssignment_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugInventoryRackAssignmentActionData.ValidRemoveData), typeof(SaveDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
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
