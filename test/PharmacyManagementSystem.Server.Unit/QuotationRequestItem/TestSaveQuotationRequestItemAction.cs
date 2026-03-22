using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.QuotationRequestItem;
using PharmacyManagementSystem.Server.Unit.QuotationRequestItem.Data;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequestItem;

[TestClass]
public class TestSaveQuotationRequestItemAction
{
    private readonly ILogger<SaveQuotationRequestItemAction> _logger;
    private readonly IQuotationRequestItemRepository _repository;
    private readonly SaveQuotationRequestItemAction _action;

    public TestSaveQuotationRequestItemAction()
    {
        _logger = Substitute.For<ILogger<SaveQuotationRequestItemAction>>();
        _repository = Substitute.For<IQuotationRequestItemRepository>();
        _action = new SaveQuotationRequestItemAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestItemActionData.ValidAddData), typeof(SaveQuotationRequestItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidInput_ReturnsSaved(Common.QuotationRequestItem.QuotationRequestItem input, Common.QuotationRequestItem.QuotationRequestItem expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.QuotationRequestItem.QuotationRequestItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.QuotationRequestItem.QuotationRequestItem>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveQuotationRequestItemActionData.InvalidAddData), typeof(SaveQuotationRequestItemActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.QuotationRequestItem.QuotationRequestItem input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationRequestItemActionData.ValidUpdateData), typeof(SaveQuotationRequestItemActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidInput_ReturnsUpdated(Common.QuotationRequestItem.QuotationRequestItem input, Common.QuotationRequestItem.QuotationRequestItem expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.QuotationRequestItem.QuotationRequestItem>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.QuotationRequestItem.QuotationRequestItem>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveQuotationRequestItemActionData.ValidRemoveData), typeof(SaveQuotationRequestItemActionData), DynamicDataSourceType.Method)]
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
