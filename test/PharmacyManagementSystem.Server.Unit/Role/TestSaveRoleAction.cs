using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Role;
using PharmacyManagementSystem.Server.Unit.Role.Data;

namespace PharmacyManagementSystem.Server.Unit.Role;

[TestClass]
public class TestSaveRoleAction
{
    private readonly ILogger<SaveRoleAction> _logger;
    private readonly IRoleRepository _repository;
    private readonly SaveRoleAction _action;

    public TestSaveRoleAction()
    {
        _logger = Substitute.For<ILogger<SaveRoleAction>>();
        _repository = Substitute.For<IRoleRepository>();
        _action = new SaveRoleAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleActionData.ValidAddData), typeof(SaveRoleActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidRole_ReturnsSavedRole(Common.Role.Role input, Common.Role.Role expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Role.Role>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Role.Role>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullRole_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleActionData.InvalidAddData), typeof(SaveRoleActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Role.Role input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleActionData.ValidUpdateData), typeof(SaveRoleActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidRole_ReturnsUpdatedRole(Common.Role.Role input, Common.Role.Role expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Role.Role>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Role.Role>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullRole_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_EmptyName_ThrowsBadRequestException()
    {
        // Arrange
        var role = new Common.Role.Role { Id = Guid.NewGuid(), Name = string.Empty };

        // Act
        var act = async () => await _action.UpdateAsync(role, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Name*");
    }

    [TestMethod]
    [DynamicData(nameof(SaveRoleActionData.ValidRemoveData), typeof(SaveRoleActionData), DynamicDataSourceType.Method)]
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
