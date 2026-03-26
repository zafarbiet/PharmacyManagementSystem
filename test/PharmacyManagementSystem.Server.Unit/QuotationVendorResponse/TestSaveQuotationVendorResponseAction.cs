using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.QuotationVendorResponse;
using PharmacyManagementSystem.Server.Unit.QuotationVendorResponse.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationVendorResponse;

[TestClass]
public class TestSaveQuotationVendorResponseAction
{
    private readonly ILogger<SaveQuotationVendorResponseAction> _logger;
    private readonly IQuotationVendorResponseRepository _repository;
    private readonly SaveQuotationVendorResponseAction _action;

    public TestSaveQuotationVendorResponseAction()
    {
        _logger = Substitute.For<ILogger<SaveQuotationVendorResponseAction>>();
        _repository = Substitute.For<IQuotationVendorResponseRepository>();
        _action = new SaveQuotationVendorResponseAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationVendorResponseActionData.ValidAddData), typeof(SaveQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidResponse_ReturnsSavedResponse(Common.QuotationVendorResponse.QuotationVendorResponse input, Common.QuotationVendorResponse.QuotationVendorResponse expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.QuotationVendorResponse.QuotationVendorResponse>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("pending");
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.QuotationVendorResponse.QuotationVendorResponse>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullResponse_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationVendorResponseActionData.InvalidAddData), typeof(SaveQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.QuotationVendorResponse.QuotationVendorResponse input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    public async Task AddAsync_NoStatus_DefaultsToPending()
    {
        // Arrange
        var response = new Common.QuotationVendorResponse.QuotationVendorResponse
        {
            QuotationRequestId = Guid.NewGuid(),
            VendorId = Guid.NewGuid(),
            Status = null
        };
        var expected = new Common.QuotationVendorResponse.QuotationVendorResponse
        {
            Id = Guid.NewGuid(), QuotationRequestId = response.QuotationRequestId,
            VendorId = response.VendorId, Status = "pending", UpdatedBy = "system"
        };
        _repository.AddAsync(Arg.Any<Common.QuotationVendorResponse.QuotationVendorResponse>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        await _action.AddAsync(response, CancellationToken.None);

        // Assert — action should set Status = "pending" before calling repo
        await _repository.Received(1).AddAsync(
            Arg.Is<Common.QuotationVendorResponse.QuotationVendorResponse>(r => r.Status == "pending"),
            Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationVendorResponseActionData.ValidUpdateData), typeof(SaveQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidResponse_ReturnsUpdatedResponse(Common.QuotationVendorResponse.QuotationVendorResponse input, Common.QuotationVendorResponse.QuotationVendorResponse expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.QuotationVendorResponse.QuotationVendorResponse>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.QuotationVendorResponse.QuotationVendorResponse>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullResponse_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationVendorResponseActionData.ValidRemoveData), typeof(SaveQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
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
