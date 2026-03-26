using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Manufacturer;
using PharmacyManagementSystem.Server.Manufacturer;
using PharmacyManagementSystem.Server.Unit.Manufacturer.Data;

namespace PharmacyManagementSystem.Server.Unit.Manufacturer;

[TestClass]
public class TestGetManufacturerAction
{
    private readonly ILogger<GetManufacturerAction> _logger;
    private readonly IManufacturerRepository _repository;
    private readonly GetManufacturerAction _action;

    public TestGetManufacturerAction()
    {
        _logger = Substitute.For<ILogger<GetManufacturerAction>>();
        _repository = Substitute.For<IManufacturerRepository>();
        _action = new GetManufacturerAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetManufacturerActionData.ValidFilterData), typeof(GetManufacturerActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(ManufacturerFilter filter, List<Common.Manufacturer.Manufacturer> expected)
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
    [DynamicData(nameof(GetManufacturerActionData.ValidIdData), typeof(GetManufacturerActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsManufacturer(string id, Common.Manufacturer.Manufacturer expected)
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
