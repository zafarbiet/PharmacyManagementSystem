using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Role;
using PharmacyManagementSystem.Server.Role;
using PharmacyManagementSystem.Server.Unit.Role.Data;

namespace PharmacyManagementSystem.Server.Unit.Role;

[TestClass]
public class TestGetRoleAction
{
    private readonly ILogger<GetRoleAction> _logger;
    private readonly IRoleRepository _repository;
    private readonly GetRoleAction _action;

    public TestGetRoleAction()
    {
        _logger = Substitute.For<ILogger<GetRoleAction>>();
        _repository = Substitute.For<IRoleRepository>();
        _action = new GetRoleAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetRoleActionData.ValidFilterData), typeof(GetRoleActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(RoleFilter filter, List<Common.Role.Role> expected)
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
    [DynamicData(nameof(GetRoleActionData.ValidIdData), typeof(GetRoleActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsRole(string id, Common.Role.Role expected)
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
