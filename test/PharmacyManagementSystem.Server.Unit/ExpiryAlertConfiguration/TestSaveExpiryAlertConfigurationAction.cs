using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration.Data;

namespace PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration;

[TestClass]
public class TestSaveExpiryAlertConfigurationAction
{
    private readonly ILogger<SaveExpiryAlertConfigurationAction> _logger;
    private readonly IExpiryAlertConfigurationRepository _repository;
    private readonly SaveExpiryAlertConfigurationAction _action;

    public TestSaveExpiryAlertConfigurationAction()
    {
        _logger = Substitute.For<ILogger<SaveExpiryAlertConfigurationAction>>();
        _repository = Substitute.For<IExpiryAlertConfigurationRepository>();
        _action = new SaveExpiryAlertConfigurationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryAlertConfigurationActionData.ValidAddData), typeof(SaveExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidExpiryAlertConfiguration_ReturnsSaved(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration input, Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.AlertType.Should().Be(expected.AlertType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullExpiryAlertConfiguration_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryAlertConfigurationActionData.InvalidAddData), typeof(SaveExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryAlertConfigurationActionData.ValidUpdateData), typeof(SaveExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidExpiryAlertConfiguration_ReturnsUpdated(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration input, Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.AlertType.Should().Be(expected.AlertType);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullExpiryAlertConfiguration_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveExpiryAlertConfigurationActionData.ValidRemoveData), typeof(SaveExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
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
