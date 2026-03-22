using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugPricing;
using PharmacyManagementSystem.Server.Unit.DrugPricing.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugPricing;

[TestClass]
public class TestSaveDrugPricingAction
{
    private readonly ILogger<SaveDrugPricingAction> _logger;
    private readonly IDrugPricingRepository _repository;
    private readonly SaveDrugPricingAction _action;

    public TestSaveDrugPricingAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugPricingAction>>();
        _repository = Substitute.For<IDrugPricingRepository>();
        _action = new SaveDrugPricingAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugPricingActionData.ValidAddData), typeof(SaveDrugPricingActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidDrugPricing_ReturnsSavedDrugPricing(Common.DrugPricing.DrugPricing input, Common.DrugPricing.DrugPricing expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DrugPricing.DrugPricing>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DrugId.Should().Be(expected.DrugId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DrugPricing.DrugPricing>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullDrugPricing_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugPricingActionData.InvalidAddData), typeof(SaveDrugPricingActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.DrugPricing.DrugPricing input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugPricingActionData.ValidUpdateData), typeof(SaveDrugPricingActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidDrugPricing_ReturnsUpdatedDrugPricing(Common.DrugPricing.DrugPricing input, Common.DrugPricing.DrugPricing expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DrugPricing.DrugPricing>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DrugId.Should().Be(expected.DrugId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DrugPricing.DrugPricing>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullDrugPricing_ThrowsArgumentNullException()
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
