using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest.Data;

namespace PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest;

[TestClass]
public class TestSaveVendorExpiryReturnRequestAction
{
    private readonly ILogger<SaveVendorExpiryReturnRequestAction> _logger;
    private readonly IVendorExpiryReturnRequestRepository _repository;
    private readonly SaveVendorExpiryReturnRequestAction _action;

    public TestSaveVendorExpiryReturnRequestAction()
    {
        _logger = Substitute.For<ILogger<SaveVendorExpiryReturnRequestAction>>();
        _repository = Substitute.For<IVendorExpiryReturnRequestRepository>();
        _action = new SaveVendorExpiryReturnRequestAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorExpiryReturnRequestActionData.ValidAddData), typeof(SaveVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidVendorExpiryReturnRequest_ReturnsSavedVendorExpiryReturnRequest(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest input, Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullVendorExpiryReturnRequest_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorExpiryReturnRequestActionData.InvalidAddData), typeof(SaveVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorExpiryReturnRequestActionData.ValidUpdateData), typeof(SaveVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidVendorExpiryReturnRequest_ReturnsUpdatedVendorExpiryReturnRequest(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest input, Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullVendorExpiryReturnRequest_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyStatus_ThrowsBadRequestException()
    {
        // Arrange
        var request = new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { Id = Guid.NewGuid(), ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = string.Empty, QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow };

        // Act
        var act = async () => await _action.UpdateAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Status*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveVendorExpiryReturnRequestActionData.ValidRemoveData), typeof(SaveVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
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
