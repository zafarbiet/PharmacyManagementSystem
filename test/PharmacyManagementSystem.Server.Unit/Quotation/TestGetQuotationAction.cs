using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Quotation;
using PharmacyManagementSystem.Server.Quotation;
using PharmacyManagementSystem.Server.Unit.Quotation.Data;

namespace PharmacyManagementSystem.Server.Unit.Quotation;

[TestClass]
public class TestGetQuotationAction
{
    private readonly ILogger<GetQuotationAction> _logger;
    private readonly IQuotationRepository _repository;
    private readonly GetQuotationAction _action;

    public TestGetQuotationAction()
    {
        _logger = Substitute.For<ILogger<GetQuotationAction>>();
        _repository = Substitute.For<IQuotationRepository>();
        _action = new GetQuotationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetQuotationActionData.ValidFilterData), typeof(GetQuotationActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(QuotationFilter filter, List<Common.Quotation.Quotation> expected)
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
    [DynamicData(nameof(GetQuotationActionData.ValidIdData), typeof(GetQuotationActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsQuotation(string id, Common.Quotation.Quotation expected)
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
