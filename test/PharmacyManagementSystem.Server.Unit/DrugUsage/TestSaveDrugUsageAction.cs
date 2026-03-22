using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugUsage;
using PharmacyManagementSystem.Server.Unit.DrugUsage.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugUsage;

[TestClass]
public class TestSaveDrugUsageAction
{
    private readonly ILogger<SaveDrugUsageAction> _logger;
    private readonly IDrugUsageRepository _repository;
    private readonly SaveDrugUsageAction _action;

    public TestSaveDrugUsageAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugUsageAction>>();
        _repository = Substitute.For<IDrugUsageRepository>();
        _action = new SaveDrugUsageAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugUsageActionData.ValidAddData), typeof(SaveDrugUsageActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidDrugUsage_ReturnsSavedDrugUsage(Common.DrugUsage.DrugUsage input, Common.DrugUsage.DrugUsage expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DrugUsage.DrugUsage>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DrugId.Should().Be(expected.DrugId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DrugUsage.DrugUsage>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullDrugUsage_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugUsageActionData.InvalidAddData), typeof(SaveDrugUsageActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.DrugUsage.DrugUsage input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugUsageActionData.ValidUpdateData), typeof(SaveDrugUsageActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidDrugUsage_ReturnsUpdatedDrugUsage(Common.DrugUsage.DrugUsage input, Common.DrugUsage.DrugUsage expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DrugUsage.DrugUsage>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DrugId.Should().Be(expected.DrugId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DrugUsage.DrugUsage>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullDrugUsage_ThrowsArgumentNullException()
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
