using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.DrugCategory;
using PharmacyManagementSystem.Server.Unit.DrugCategory.Data;

namespace PharmacyManagementSystem.Server.Unit.DrugCategory;

[TestClass]
public class TestSaveDrugCategoryAction
{
    private readonly ILogger<SaveDrugCategoryAction> _logger;
    private readonly IDrugCategoryRepository _repository;
    private readonly SaveDrugCategoryAction _action;

    public TestSaveDrugCategoryAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugCategoryAction>>();
        _repository = Substitute.For<IDrugCategoryRepository>();
        _action = new SaveDrugCategoryAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugCategoryActionData.ValidAddData), typeof(SaveDrugCategoryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidCategory_ReturnsSavedCategory(
        Common.DrugCategory.DrugCategory input,
        Common.DrugCategory.DrugCategory expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.DrugCategory.DrugCategory>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.DrugCategory.DrugCategory>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullCategory_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugCategoryActionData.InvalidAddData), typeof(SaveDrugCategoryActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidName_ThrowsBadRequestException(Common.DrugCategory.DrugCategory input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugCategoryActionData.ValidUpdateData), typeof(SaveDrugCategoryActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidCategory_ReturnsUpdatedCategory(
        Common.DrugCategory.DrugCategory input,
        Common.DrugCategory.DrugCategory expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.DrugCategory.DrugCategory>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.DrugCategory.DrugCategory>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullCategory_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyName_ThrowsBadRequestException()
    {
        // Arrange
        var category = new Common.DrugCategory.DrugCategory { Id = Guid.NewGuid(), Name = string.Empty };

        // Act
        var act = async () => await _action.UpdateAsync(category, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugCategoryActionData.ValidRemoveData), typeof(SaveDrugCategoryActionData), DynamicDataSourceType.Method)]
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
