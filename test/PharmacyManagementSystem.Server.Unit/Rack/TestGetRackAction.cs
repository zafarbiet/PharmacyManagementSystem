using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Rack;
using PharmacyManagementSystem.Server.Rack;
using PharmacyManagementSystem.Server.Unit.Rack.Data;

namespace PharmacyManagementSystem.Server.Unit.Rack;

[TestClass]
public class TestGetRackAction
{
    private readonly ILogger<GetRackAction> _logger;
    private readonly IRackRepository _repository;
    private readonly GetRackAction _action;

    public TestGetRackAction()
    {
        _logger = Substitute.For<ILogger<GetRackAction>>();
        _repository = Substitute.For<IRackRepository>();
        _action = new GetRackAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetRackActionData.ValidFilterData), typeof(GetRackActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(RackFilter filter, List<Common.Rack.Rack> expected)
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
    [DynamicData(nameof(GetRackActionData.ValidIdData), typeof(GetRackActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsRack(string id, Common.Rack.Rack expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Label.Should().Be(expected.Label);
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
