using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Prescription;
using PharmacyManagementSystem.Server.Unit.Prescription.Data;

namespace PharmacyManagementSystem.Server.Unit.Prescription;

[TestClass]
public class TestSavePrescriptionAction
{
    private readonly ILogger<SavePrescriptionAction> _logger;
    private readonly IPrescriptionRepository _repository;
    private readonly SavePrescriptionAction _action;

    public TestSavePrescriptionAction()
    {
        _logger = Substitute.For<ILogger<SavePrescriptionAction>>();
        _repository = Substitute.For<IPrescriptionRepository>();
        _action = new SavePrescriptionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionActionData.ValidAddData), typeof(SavePrescriptionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenValidPrescription_ReturnsSavedPrescription(Common.Prescription.Prescription input, Common.Prescription.Prescription expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Prescription.Prescription>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PatientId.Should().Be(expected.PatientId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Prescription.Prescription>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_WhenNullPrescription_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionActionData.InvalidAddData), typeof(SavePrescriptionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_WhenInvalidData_ThrowsBadRequestException(Common.Prescription.Prescription input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePrescriptionActionData.ValidUpdateData), typeof(SavePrescriptionActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_WhenValidPrescription_ReturnsUpdatedPrescription(Common.Prescription.Prescription input, Common.Prescription.Prescription expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Prescription.Prescription>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.PatientId.Should().Be(expected.PatientId);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Prescription.Prescription>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_WhenNullPrescription_ThrowsArgumentNullException()
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
