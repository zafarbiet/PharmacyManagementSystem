using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Rack;
using PharmacyManagementSystem.Server.Unit.Rack.Data;

namespace PharmacyManagementSystem.Server.Unit.Rack;

[TestClass]
public class TestSaveRackAction
{
    private readonly ILogger<SaveRackAction> _logger;
    private readonly IRackRepository _repository;
    private readonly SaveRackAction _action;

    public TestSaveRackAction()
    {
        _logger = Substitute.For<ILogger<SaveRackAction>>();
        _repository = Substitute.For<IRackRepository>();
        _action = new SaveRackAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveRackActionData.ValidAddData), typeof(SaveRackActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRack_ReturnsSavedRack(Common.Rack.Rack input, Common.Rack.Rack expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Rack.Rack>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Label.Should().Be(expected.Label);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Rack.Rack>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullRack_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRackActionData.InvalidAddData), typeof(SaveRackActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Rack.Rack input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRackActionData.ValidUpdateData), typeof(SaveRackActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidRack_ReturnsUpdatedRack(Common.Rack.Rack input, Common.Rack.Rack expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Rack.Rack>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Label.Should().Be(expected.Label);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Rack.Rack>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullRack_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRackActionData.ValidRemoveData), typeof(SaveRackActionData), DynamicDataSourceType.Method)]
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
