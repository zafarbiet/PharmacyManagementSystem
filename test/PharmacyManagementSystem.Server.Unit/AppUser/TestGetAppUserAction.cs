using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.AppUser;
using PharmacyManagementSystem.Server.AppUser;
using PharmacyManagementSystem.Server.Unit.AppUser.Data;

namespace PharmacyManagementSystem.Server.Unit.AppUser;

[TestClass]
public class TestGetAppUserAction
{
    private readonly ILogger<GetAppUserAction> _logger;
    private readonly IAppUserRepository _repository;
    private readonly GetAppUserAction _action;

    public TestGetAppUserAction()
    {
        _logger = Substitute.For<ILogger<GetAppUserAction>>();
        _repository = Substitute.For<IAppUserRepository>();
        _action = new GetAppUserAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(GetAppUserActionData.ValidFilterData), typeof(GetAppUserActionData), DynamicDataSourceType.Method)]
    public async Task GetByFilterCriteriaAsync_ValidFilter_ReturnsData(AppUserFilter filter, List<Common.AppUser.AppUser> expected)
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
    [DynamicData(nameof(GetAppUserActionData.ValidIdData), typeof(GetAppUserActionData), DynamicDataSourceType.Method)]
    public async Task GetByIdAsync_ValidId_ReturnsAppUser(string id, Common.AppUser.AppUser expected)
    {
        // Arrange
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Username.Should().Be(expected.Username);
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
