using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.AuditLog;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Unit.AuditLog.Data;

namespace PharmacyManagementSystem.Server.Unit.AuditLog;

[TestClass]
public class TestGetAuditLogAction
{
    private readonly ILogger<GetAuditLogAction> _logger;
    private readonly IAuditLogRepository _repository;
    private readonly GetAuditLogAction _action;

    public TestGetAuditLogAction()
    {
        _logger = Substitute.For<ILogger<GetAuditLogAction>>();
        _repository = Substitute.For<IAuditLogRepository>();
        _action = new GetAuditLogAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetAuditLogActionData.ValidFilterData), typeof(GetAuditLogActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(AuditLogFilter filter, List<Common.AuditLog.AuditLog> expected)
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
    [DynamicData(nameof(GetAuditLogActionData.ValidIdData), typeof(GetAuditLogActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsAuditLog(string id, Common.AuditLog.AuditLog expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.DrugName.Should().Be(expected.DrugName);
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
