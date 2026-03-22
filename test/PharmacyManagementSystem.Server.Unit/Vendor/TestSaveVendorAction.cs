using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Vendor;
using PharmacyManagementSystem.Server.Unit.Vendor.Data;

namespace PharmacyManagementSystem.Server.Unit.Vendor;

[TestClass]
public class TestSaveVendorAction
{
    private readonly ILogger<SaveVendorAction> _logger;
    private readonly IVendorRepository _repository;
    private readonly SaveVendorAction _action;

    public TestSaveVendorAction()
    {
        _logger = Substitute.For<ILogger<SaveVendorAction>>();
        _repository = Substitute.For<IVendorRepository>();
        _action = new SaveVendorAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorActionData.ValidAddData), typeof(SaveVendorActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidVendor_ReturnsSavedVendor(Common.Vendor.Vendor input, Common.Vendor.Vendor expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullVendor_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorActionData.InvalidAddData), typeof(SaveVendorActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidName_ThrowsBadRequestException(Common.Vendor.Vendor input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorActionData.ValidUpdateData), typeof(SaveVendorActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidVendor_ReturnsUpdatedVendor(Common.Vendor.Vendor input, Common.Vendor.Vendor expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Vendor.Vendor>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullVendor_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyName_ThrowsBadRequestException()
    {
        // Arrange
        var vendor = new Common.Vendor.Vendor { Id = Guid.NewGuid(), Name = string.Empty };

        // Act
        var act = async () => await _action.UpdateAsync(vendor, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorActionData.ValidRemoveData), typeof(SaveVendorActionData), DynamicDataSourceType.Method)]
    public async Task RemoveAsync_ValidId_CallsRepository(Guid id, string updatedBy)
    {
        // Arrange
        _repository.RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _action.RemoveAsync(id, updatedBy, CancellationToken.None);

        // Assert
        await _repository.Received(1).RemoveAsync(id, updatedBy, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task RemoveAsync_NullUpdatedBy_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.RemoveAsync(Guid.NewGuid(), null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
