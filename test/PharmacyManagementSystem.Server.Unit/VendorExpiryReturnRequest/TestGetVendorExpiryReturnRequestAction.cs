using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest.Data;

namespace PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest;

[TestClass]
public class TestGetVendorExpiryReturnRequestAction
{
    private readonly ILogger<GetVendorExpiryReturnRequestAction> _logger;
    private readonly IVendorExpiryReturnRequestRepository _repository;
    private readonly GetVendorExpiryReturnRequestAction _action;

    public TestGetVendorExpiryReturnRequestAction()
    {
        _logger = Substitute.For<ILogger<GetVendorExpiryReturnRequestAction>>();
        _repository = Substitute.For<IVendorExpiryReturnRequestRepository>();
        _action = new GetVendorExpiryReturnRequestAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetVendorExpiryReturnRequestActionData.ValidFilterData), typeof(GetVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(VendorExpiryReturnRequestFilter filter, List<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest> expected)
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
    [DynamicData(nameof(GetVendorExpiryReturnRequestActionData.ValidIdData), typeof(GetVendorExpiryReturnRequestActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsVendorExpiryReturnRequest(string id, Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Status.Should().Be(expected.Status);
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
