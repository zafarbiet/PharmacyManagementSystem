using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.StorageZone;
using PharmacyManagementSystem.Server.StorageZone;
using PharmacyManagementSystem.Server.Unit.StorageZone.Data;

namespace PharmacyManagementSystem.Server.Unit.StorageZone;

[TestClass]
public class TestGetStorageZoneAction
{
    private readonly ILogger<GetStorageZoneAction> _logger;
    private readonly IStorageZoneRepository _repository;
    private readonly GetStorageZoneAction _action;

    public TestGetStorageZoneAction()
    {
        _logger = Substitute.For<ILogger<GetStorageZoneAction>>();
        _repository = Substitute.For<IStorageZoneRepository>();
        _action = new GetStorageZoneAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetStorageZoneActionData.ValidFilterData), typeof(GetStorageZoneActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(StorageZoneFilter filter, List<Common.StorageZone.StorageZone> expected)
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
    [DynamicData(nameof(GetStorageZoneActionData.ValidIdData), typeof(GetStorageZoneActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsStorageZone(string id, Common.StorageZone.StorageZone expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
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
