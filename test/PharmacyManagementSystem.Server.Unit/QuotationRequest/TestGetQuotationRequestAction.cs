using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.QuotationRequest;
using PharmacyManagementSystem.Server.QuotationRequest;
using PharmacyManagementSystem.Server.Unit.QuotationRequest.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequest;

[TestClass]
public class TestGetQuotationRequestAction
{
    private readonly ILogger<GetQuotationRequestAction> _logger;
    private readonly IQuotationRequestRepository _repository;
    private readonly GetQuotationRequestAction _action;

    public TestGetQuotationRequestAction()
    {
        _logger = Substitute.For<ILogger<GetQuotationRequestAction>>();
        _repository = Substitute.For<IQuotationRequestRepository>();
        _action = new GetQuotationRequestAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetQuotationRequestActionData.ValidFilterData), typeof(GetQuotationRequestActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(QuotationRequestFilter filter, List<Common.QuotationRequest.QuotationRequest> expected)
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
    [DynamicData(nameof(GetQuotationRequestActionData.ValidIdData), typeof(GetQuotationRequestActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsQuotationRequest(string id, Common.QuotationRequest.QuotationRequest expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
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
