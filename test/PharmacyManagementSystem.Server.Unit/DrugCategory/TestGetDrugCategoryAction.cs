using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DrugCategory;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Unit.DrugCategory.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugCategory;

[TestClass]
public class TestGetDrugCategoryAction
{
    private readonly ILogger<GetDrugCategoryAction> _logger;
    private readonly IDrugCategoryRepository _repository;
    private readonly GetDrugCategoryAction _action;

    public TestGetDrugCategoryAction()
    {
        _logger = Substitute.For<ILogger<GetDrugCategoryAction>>();
        _repository = Substitute.For<IDrugCategoryRepository>();
        _action = new GetDrugCategoryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugCategoryActionData.ValidFilterData), typeof(GetDrugCategoryActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DrugCategoryFilter filter, List<Common.DrugCategory.DrugCategory> expected)
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
    [DynamicData(nameof(GetDrugCategoryActionData.ValidIdData), typeof(GetDrugCategoryActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDrugCategory(string id, Common.DrugCategory.DrugCategory expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
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
