using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Prescription;
using PharmacyManagementSystem.Server.Prescription;
using PharmacyManagementSystem.Server.Unit.Prescription.Data;

namespace PharmacyManagementSystem.Server.Unit.Prescription;

[TestClass]
public class TestGetPrescriptionAction
{
    private readonly ILogger<GetPrescriptionAction> _logger;
    private readonly IPrescriptionRepository _repository;
    private readonly GetPrescriptionAction _action;

    public TestGetPrescriptionAction()
    {
        _logger = Substitute.For<ILogger<GetPrescriptionAction>>();
        _repository = Substitute.For<IPrescriptionRepository>();
        _action = new GetPrescriptionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPrescriptionActionData.ValidFilterData), typeof(GetPrescriptionActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(PrescriptionFilter filter, List<Common.Prescription.Prescription> expected)
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
    [DynamicData(nameof(GetPrescriptionActionData.ValidIdData), typeof(GetPrescriptionActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsPrescription(string id, Common.Prescription.Prescription expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PatientId.Should().Be(expected.PatientId);
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
