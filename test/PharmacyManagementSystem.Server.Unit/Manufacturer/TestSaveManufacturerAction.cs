using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Manufacturer;
using PharmacyManagementSystem.Server.Unit.Manufacturer.Data;

namespace PharmacyManagementSystem.Server.Unit.Manufacturer;

[TestClass]
public class TestSaveManufacturerAction
{
    private readonly ILogger<SaveManufacturerAction> _logger;
    private readonly IManufacturerRepository _repository;
    private readonly SaveManufacturerAction _action;

    public TestSaveManufacturerAction()
    {
        _logger = Substitute.For<ILogger<SaveManufacturerAction>>();
        _repository = Substitute.For<IManufacturerRepository>();
        _action = new SaveManufacturerAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveManufacturerActionData.ValidAddData), typeof(SaveManufacturerActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidManufacturer_ReturnsSavedManufacturer(Common.Manufacturer.Manufacturer input, Common.Manufacturer.Manufacturer expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Manufacturer.Manufacturer>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Manufacturer.Manufacturer>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullManufacturer_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveManufacturerActionData.InvalidAddData), typeof(SaveManufacturerActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Manufacturer.Manufacturer input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveManufacturerActionData.ValidUpdateData), typeof(SaveManufacturerActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidManufacturer_ReturnsUpdatedManufacturer(Common.Manufacturer.Manufacturer input, Common.Manufacturer.Manufacturer expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Manufacturer.Manufacturer>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Manufacturer.Manufacturer>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullManufacturer_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyName_ThrowsBadRequestException()
    {
        // Arrange
        var manufacturer = new Common.Manufacturer.Manufacturer { Id = Guid.NewGuid(), Name = string.Empty };

        // Act
        var act = async () => await _action.UpdateAsync(manufacturer, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveManufacturerActionData.ValidRemoveData), typeof(SaveManufacturerActionData), DynamicDataSourceType.Method)]
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
