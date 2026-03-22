using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.DrugPricing;
using PharmacyManagementSystem.Server.DrugPricing;
using PharmacyManagementSystem.Server.Unit.DrugPricing.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugPricing;

[TestClass]
public class TestGetDrugPricingAction
{
    private readonly ILogger<GetDrugPricingAction> _logger;
    private readonly IDrugPricingRepository _repository;
    private readonly GetDrugPricingAction _action;

    public TestGetDrugPricingAction()
    {
        _logger = Substitute.For<ILogger<GetDrugPricingAction>>();
        _repository = Substitute.For<IDrugPricingRepository>();
        _action = new GetDrugPricingAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugPricingActionData.ValidFilterData), typeof(GetDrugPricingActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(DrugPricingFilter filter, List<Common.DrugPricing.DrugPricing> expected)
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
    [DynamicData(nameof(GetDrugPricingActionData.ValidIdData), typeof(GetDrugPricingActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsDrugPricing(string id, Common.DrugPricing.DrugPricing expected)
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
