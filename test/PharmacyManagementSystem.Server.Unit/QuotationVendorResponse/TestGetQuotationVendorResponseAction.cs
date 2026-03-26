using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.QuotationVendorResponse;
using PharmacyManagementSystem.Server.QuotationVendorResponse;
using PharmacyManagementSystem.Server.Unit.QuotationVendorResponse.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationVendorResponse;

[TestClass]
public class TestGetQuotationVendorResponseAction
{
    private readonly ILogger<GetQuotationVendorResponseAction> _logger;
    private readonly IQuotationVendorResponseRepository _repository;
    private readonly GetQuotationVendorResponseAction _action;

    public TestGetQuotationVendorResponseAction()
    {
        _logger = Substitute.For<ILogger<GetQuotationVendorResponseAction>>();
        _repository = Substitute.For<IQuotationVendorResponseRepository>();
        _action = new GetQuotationVendorResponseAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetQuotationVendorResponseActionData.ValidFilterData), typeof(GetQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(QuotationVendorResponseFilter filter, List<Common.QuotationVendorResponse.QuotationVendorResponse> expected)
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
    [DynamicData(nameof(GetQuotationVendorResponseActionData.ValidIdData), typeof(GetQuotationVendorResponseActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsResponse(string id, Common.QuotationVendorResponse.QuotationVendorResponse expected)
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
