using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Promotion;
using PharmacyManagementSystem.Server.Unit.Promotion.Data;

namespace PharmacyManagementSystem.Server.Unit.Promotion;

[TestClass]
public class TestSavePromotionAction
{
    private readonly ILogger<SavePromotionAction> _logger;
    private readonly IPromotionRepository _repository;
    private readonly SavePromotionAction _action;

    public TestSavePromotionAction()
    {
        _logger = Substitute.For<ILogger<SavePromotionAction>>();
        _repository = Substitute.For<IPromotionRepository>();
        _action = new SavePromotionAction(_logger, _repository);
    }

    [TestMethod]
    [DynamicData(nameof(SavePromotionActionData.ValidAddData), typeof(SavePromotionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_ValidPromotion_ReturnsSavedPromotion(Common.Promotion.Promotion input, Common.Promotion.Promotion expected)
    {
        // Arrange
        _repository.AddAsync(Arg.Any<Common.Promotion.Promotion>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.AddAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
        result.DiscountPercentage.Should().Be(expected.DiscountPercentage);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).AddAsync(Arg.Any<Common.Promotion.Promotion>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task AddAsync_NullPromotion_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.AddAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePromotionActionData.InvalidAddData), typeof(SavePromotionActionData), DynamicDataSourceType.Method)]
    public async Task AddAsync_InvalidData_ThrowsBadRequestException(Common.Promotion.Promotion input)
    {
        // Act
        var act = async () => await _action.AddAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [TestMethod]
    [DynamicData(nameof(SavePromotionActionData.ValidUpdateData), typeof(SavePromotionActionData), DynamicDataSourceType.Method)]
    public async Task UpdateAsync_ValidPromotion_ReturnsUpdatedPromotion(Common.Promotion.Promotion input, Common.Promotion.Promotion expected)
    {
        // Arrange
        _repository.UpdateAsync(Arg.Any<Common.Promotion.Promotion>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _action.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expected.Id);
        result.Name.Should().Be(expected.Name);
        result.UpdatedBy.Should().Be("system");
        await _repository.Received(1).UpdateAsync(Arg.Any<Common.Promotion.Promotion>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task UpdateAsync_NullPromotion_ThrowsArgumentNullException()
    {
        // Act
        var act = async () => await _action.UpdateAsync(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task UpdateAsync_ValidToBeforeValidFrom_ThrowsBadRequestException()
    {
        // Arrange
        var promotion = new Common.Promotion.Promotion
        {
            Id = Guid.NewGuid(),
            Name = "Bad Promo",
            DiscountPercentage = 10m,
            ValidFrom = DateTimeOffset.UtcNow.AddDays(10),
            ValidTo = DateTimeOffset.UtcNow
        };

        // Act
        var act = async () => await _action.UpdateAsync(promotion, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*ValidTo*");
    }

    [TestMethod]
    [DynamicData(nameof(SavePromotionActionData.ValidRemoveData), typeof(SavePromotionActionData), DynamicDataSourceType.Method)]
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
