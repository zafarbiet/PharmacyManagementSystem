using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.PrescriptionItem;
using PharmacyManagementSystem.Server.PrescriptionItem;
using PharmacyManagementSystem.Server.Unit.PrescriptionItem.Data;

namespace PharmacyManagementSystem.Server.Unit.PrescriptionItem;

[TestClass]
public class TestGetPrescriptionItemAction
{
    private readonly ILogger<GetPrescriptionItemAction> _logger;
    private readonly IPrescriptionItemRepository _repository;
    private readonly GetPrescriptionItemAction _action;

    public TestGetPrescriptionItemAction()
    {
        _logger = Substitute.For<ILogger<GetPrescriptionItemAction>>();
        _repository = Substitute.For<IPrescriptionItemRepository>();
        _action = new GetPrescriptionItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPrescriptionItemActionData.ValidFilterData), typeof(GetPrescriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(PrescriptionItemFilter filter, List<Common.PrescriptionItem.PrescriptionItem> expected)
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
    public async Task GetByFilterCriteriaAsync_WhenNullFilter_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByFilterCriteriaAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(GetPrescriptionItemActionData.ValidIdData), typeof(GetPrescriptionItemActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsPrescriptionItem(string id, Common.PrescriptionItem.PrescriptionItem expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PrescriptionId.Should().Be(expected.PrescriptionId);
        await _repository.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenNullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
