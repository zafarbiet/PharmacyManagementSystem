using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.DrugInventoryRackAssignment;
using PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment;

[TestClass]
public class TestGetDrugInventoryRackAssignmentAction
{
    private readonly ILogger<GetDrugInventoryRackAssignmentAction> _logger;
    private readonly IDrugInventoryRackAssignmentRepository _repository;
    private readonly GetDrugInventoryRackAssignmentAction _action;

    public TestGetDrugInventoryRackAssignmentAction()
    {
        _logger = Substitute.For<ILogger<GetDrugInventoryRackAssignmentAction>>();
        _repository = Substitute.For<IDrugInventoryRackAssignmentRepository>();
        _action = new GetDrugInventoryRackAssignmentAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugInventoryRackAssignmentActionData.ValidFilterData), typeof(GetDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DrugInventoryRackAssignmentFilter filter, List<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment> expected)
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
    [DynamicData(nameof(GetDrugInventoryRackAssignmentActionData.ValidIdData), typeof(GetDrugInventoryRackAssignmentActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDrugInventoryRackAssignment(string id, Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DrugInventoryId.Should().Be(expected.DrugInventoryId);
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
