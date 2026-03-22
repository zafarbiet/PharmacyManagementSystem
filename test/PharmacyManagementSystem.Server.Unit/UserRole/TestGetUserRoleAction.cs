using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.UserRole;
using PharmacyManagementSystem.Server.UserRole;
using PharmacyManagementSystem.Server.Unit.UserRole.Data;

namespace PharmacyManagementSystem.Server.Unit.UserRole;

[TestClass]
public class TestGetUserRoleAction
{
    private readonly ILogger<GetUserRoleAction> _logger;
    private readonly IUserRoleRepository _repository;
    private readonly GetUserRoleAction _action;

    public TestGetUserRoleAction()
    {
        _logger = Substitute.For<ILogger<GetUserRoleAction>>();
        _repository = Substitute.For<IUserRoleRepository>();
        _action = new GetUserRoleAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetUserRoleActionData.ValidFilterData), typeof(GetUserRoleActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(UserRoleFilter filter, List<Common.UserRole.UserRole> expected)
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
    [DynamicData(nameof(GetUserRoleActionData.ValidIdData), typeof(GetUserRoleActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsUserRole(string id, Common.UserRole.UserRole expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UserId.Should().Be(expected.UserId);
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
