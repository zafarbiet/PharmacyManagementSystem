using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Patient;
using PharmacyManagementSystem.Server.Patient;
using PharmacyManagementSystem.Server.Unit.Patient.Data;

namespace PharmacyManagementSystem.Server.Unit.Patient;

[TestClass]
public class TestGetPatientAction
{
    private readonly ILogger<GetPatientAction> _logger;
    private readonly IPatientRepository _repository;
    private readonly GetPatientAction _action;

    public TestGetPatientAction()
    {
        _logger = Substitute.For<ILogger<GetPatientAction>>();
        _repository = Substitute.For<IPatientRepository>();
        _action = new GetPatientAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetPatientActionData.ValidFilterData), typeof(GetPatientActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_WhenValidFilter_ReturnsData(PatientFilter filter, List<Common.Patient.Patient> expected)
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
    [DynamicData(nameof(GetPatientActionData.ValidIdData), typeof(GetPatientActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_WhenValidId_ReturnsPatient(string id, Common.Patient.Patient expected)
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
    public async Task GetByIdAsync_WhenNullId_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.GetByIdAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
