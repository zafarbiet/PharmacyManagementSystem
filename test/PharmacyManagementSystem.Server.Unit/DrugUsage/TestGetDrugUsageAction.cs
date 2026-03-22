using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DrugUsage;
using PharmacyManagementSystem.Server.DrugUsage;
using PharmacyManagementSystem.Server.Unit.DrugUsage.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugUsage;

[TestClass]
public class TestGetDrugUsageAction
{
    private readonly ILogger<GetDrugUsageAction> _logger;
    private readonly IDrugUsageRepository _repository;
    private readonly GetDrugUsageAction _action;

    public TestGetDrugUsageAction()
    {
        _logger = Substitute.For<ILogger<GetDrugUsageAction>>();
        _repository = Substitute.For<IDrugUsageRepository>();
        _action = new GetDrugUsageAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugUsageActionData.ValidFilterData), typeof(GetDrugUsageActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(DrugUsageFilter filter, List<Common.DrugUsage.DrugUsage> expected)
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
    [DynamicData(nameof(GetDrugUsageActionData.ValidIdData), typeof(GetDrugUsageActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsDrugUsage(string id, Common.DrugUsage.DrugUsage expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DrugId.Should().Be(expected.DrugId);
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
