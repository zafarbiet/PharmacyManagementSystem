using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.Unit.Drug.Data;

namespace PharmacyManagementSystem.Server.Unit.Drug;

[TestClass]
public class TestSaveDrugAction
{
    private readonly ILogger<SaveDrugAction> _logger;
    private readonly IDrugRepository _repository;
    private readonly SaveDrugAction _action;

    public TestSaveDrugAction()
    {
        _logger = Substitute.For<ILogger<SaveDrugAction>>();
        _repository = Substitute.For<IDrugRepository>();
        _action = new SaveDrugAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugActionData.ValidAddData), typeof(SaveDrugActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidDrug_ReturnsSavedDrug(Common.Drug.Drug input, Common.Drug.Drug expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Drug.Drug>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Drug.Drug>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullDrug_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugActionData.InvalidAddData), typeof(SaveDrugActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Drug.Drug input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugActionData.ValidUpdateData), typeof(SaveDrugActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidDrug_ReturnsUpdatedDrug(Common.Drug.Drug input, Common.Drug.Drug expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Drug.Drug>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Drug.Drug>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullDrug_ThrowsArgumentNullException()
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
        var drug = new Common.Drug.Drug { Id = Guid.NewGuid(), Name = string.Empty, CategoryId = Guid.NewGuid() };

        // Act
        var act = async () => await _action.UpdateAsync(drug, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveDrugActionData.ValidRemoveData), typeof(SaveDrugActionData), DynamicDataSourceType.Method)]
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
