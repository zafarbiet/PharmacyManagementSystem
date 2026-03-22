using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Drug;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.Unit.Drug.Data;

namespace PharmacyManagementSystem.Server.Unit.Drug;

[TestClass]
public class TestGetDrugAction
{
    private readonly ILogger<GetDrugAction> _logger;
    private readonly IDrugRepository _repository;
    private readonly GetDrugAction _action;

    public TestGetDrugAction()
    {
        _logger = Substitute.For<ILogger<GetDrugAction>>();
        _repository = Substitute.For<IDrugRepository>();
        _action = new GetDrugAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetDrugActionData.ValidFilterData), typeof(GetDrugActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(DrugFilter filter, List<Common.Drug.Drug> expected)
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
    [DynamicData(nameof(GetDrugActionData.ValidIdData), typeof(GetDrugActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsDrug(string id, Common.Drug.Drug expected)
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
