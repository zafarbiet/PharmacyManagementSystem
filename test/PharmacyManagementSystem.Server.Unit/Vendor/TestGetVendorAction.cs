using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Vendor;
using PharmacyManagementSystem.Server.Vendor;
using PharmacyManagementSystem.Server.Unit.Vendor.Data;

namespace PharmacyManagementSystem.Server.Unit.Vendor;

[TestClass]
public class TestGetVendorAction
{
    private readonly ILogger<GetVendorAction> _logger;
    private readonly IVendorRepository _repository;
    private readonly GetVendorAction _action;

    public TestGetVendorAction()
    {
        _logger = Substitute.For<ILogger<GetVendorAction>>();
        _repository = Substitute.For<IVendorRepository>();
        _action = new GetVendorAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetVendorActionData.ValidFilterData), typeof(GetVendorActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(VendorFilter filter, List<Common.Vendor.Vendor> expected)
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
    [DynamicData(nameof(GetVendorActionData.ValidIdData), typeof(GetVendorActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsVendor(string id, Common.Vendor.Vendor expected)
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
