using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Quotation;
using PharmacyManagementSystem.Server.Unit.Quotation.Data;

namespace PharmacyManagementSystem.Server.Unit.Quotation;

[TestClass]
public class TestSaveQuotationAction
{
    private readonly ILogger<SaveQuotationAction> _logger;
    private readonly IQuotationRepository _repository;
    private readonly SaveQuotationAction _action;

    public TestSaveQuotationAction()
    {
        _logger = Substitute.For<ILogger<SaveQuotationAction>>();
        _repository = Substitute.For<IQuotationRepository>();
        _action = new SaveQuotationAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationActionData.ValidAddData), typeof(SaveQuotationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidInput_ReturnsSaved(Common.Quotation.Quotation input, Common.Quotation.Quotation expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Quotation.Quotation>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Quotation.Quotation>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveQuotationActionData.InvalidAddData), typeof(SaveQuotationActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Quotation.Quotation input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SaveQuotationActionData.ValidUpdateData), typeof(SaveQuotationActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidInput_ReturnsUpdated(Common.Quotation.Quotation input, Common.Quotation.Quotation expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Quotation.Quotation>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Quotation.Quotation>(), Arg.Any<CancellationToken>());
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
    [DynamicData(nameof(SaveQuotationActionData.ValidRemoveData), typeof(SaveQuotationActionData), DynamicDataSourceType.Method)]
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
