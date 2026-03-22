using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.Unit.DrugInventory.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugInventory;

[TestClass]
public class TestGetDrugInventoryAction
{
    private readonly ILogger<GetDrugInventoryAction> _logger;
    private readonly IDrugInventoryRepository _repository;
    private readonly GetDrugInventoryAction _action;

    public TestGetDrugInventoryAction()
    {
        _logger = Substitute.For<ILogger<GetDrugInventoryAction>>();
        _repository = Substitute.For<IDrugInventoryRepository>();
        _action = new GetDrugInventoryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugInventoryActionData.ValidFilterData), typeof(GetDrugInventoryActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(DrugInventoryFilter filter, List<Common.DrugInventory.DrugInventory> expected)
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
    public async Task GetByFilterCriteriaAsync_WhenNullFilter_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByFilterCriteriaAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugInventoryActionData.ValidIdData), typeof(GetDrugInventoryActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsDrugInventory(string id, Common.DrugInventory.DrugInventory expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.BatchNumber.Should().Be(expected.BatchNumber);
        await _repository.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenNullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
