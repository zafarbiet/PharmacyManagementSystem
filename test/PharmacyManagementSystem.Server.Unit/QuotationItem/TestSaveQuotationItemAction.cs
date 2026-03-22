using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.QuotationItem;
using PharmacyManagementSystem.Server.Unit.QuotationItem.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationItem;

[TestClass]
public class TestSaveQuotationItemAction
{
    private readonly ILogger<SaveQuotationItemAction> _logger;
    private readonly IQuotationItemRepository _repository;
    private readonly SaveQuotationItemAction _action;

    public TestSaveQuotationItemAction()
    {
        _logger = Substitute.For<ILogger<SaveQuotationItemAction>>();
        _repository = Substitute.For<IQuotationItemRepository>();
        _action = new SaveQuotationItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationItemActionData.ValidAddData), typeof(SaveQuotationItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidInput_ReturnsSaved(Common.QuotationItem.QuotationItem input, Common.QuotationItem.QuotationItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.QuotationItem.QuotationItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.QuotationItem.QuotationItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationItemActionData.InvalidAddData), typeof(SaveQuotationItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.QuotationItem.QuotationItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationItemActionData.ValidUpdateData), typeof(SaveQuotationItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidInput_ReturnsUpdated(Common.QuotationItem.QuotationItem input, Common.QuotationItem.QuotationItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.QuotationItem.QuotationItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.QuotationItem.QuotationItem>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationItemActionData.ValidRemoveData), typeof(SaveQuotationItemActionData), DynamicDataSourceType.Method)]
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
