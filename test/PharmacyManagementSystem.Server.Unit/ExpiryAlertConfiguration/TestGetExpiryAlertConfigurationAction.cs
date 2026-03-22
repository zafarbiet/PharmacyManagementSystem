using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration.Data;

namespace PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration;

[TestClass]
public class TestGetExpiryAlertConfigurationAction
{
    private readonly ILogger<GetExpiryAlertConfigurationAction> _logger;
    private readonly IExpiryAlertConfigurationRepository _repository;
    private readonly GetExpiryAlertConfigurationAction _action;

    public TestGetExpiryAlertConfigurationAction()
    {
        _logger = Substitute.For<ILogger<GetExpiryAlertConfigurationAction>>();
        _repository = Substitute.For<IExpiryAlertConfigurationRepository>();
        _action = new GetExpiryAlertConfigurationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetExpiryAlertConfigurationActionData.ValidFilterData), typeof(GetExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(ExpiryAlertConfigurationFilter filter, List<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration> expected)
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
    [DynamicData(nameof(GetExpiryAlertConfigurationActionData.ValidIdData), typeof(GetExpiryAlertConfigurationActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsExpiryAlertConfiguration(string id, Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.AlertType.Should().Be(expected.AlertType);
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
