using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Patient;
using PharmacyManagementSystem.Server.Unit.Patient.Data;

namespace PharmacyManagementSystem.Server.Unit.Patient;

[TestClass]
public class TestSavePatientAction
{
    private readonly ILogger<SavePatientAction> _logger;
    private readonly IPatientRepository _repository;
    private readonly SavePatientAction _action;

    public TestSavePatientAction()
    {
        _logger = Substitute.For<ILogger<SavePatientAction>>();
        _repository = Substitute.For<IPatientRepository>();
        _action = new SavePatientAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePatientActionData.ValidAddData), typeof(SavePatientActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidPatient_ReturnsSavedPatient(Common.Patient.Patient input, Common.Patient.Patient expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Patient.Patient>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Patient.Patient>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullPatient_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePatientActionData.InvalidAddData), typeof(SavePatientActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.Patient.Patient input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePatientActionData.ValidUpdateData), typeof(SavePatientActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidPatient_ReturnsUpdatedPatient(Common.Patient.Patient input, Common.Patient.Patient expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Patient.Patient>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Patient.Patient>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullPatient_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task RemoveAsync_WhenValidId_CallsRepository()
    {
        // Arrange
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var updatedBy = "system";
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_WhenNullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
