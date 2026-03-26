using PharmacyManagementSystem.Common.Promotion;

namespace PharmacyManagementSystem.Server.Unit.Promotion.Data;

public static class GetPromotionActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var validFrom = DateTimeOffset.UtcNow.AddDays(-10);
        var validTo = DateTimeOffset.UtcNow.AddDays(20);

        yield return new object[]
        {
            new PromotionFilter { Name = "Summer Sale" },
            new List<Common.Promotion.Promotion>
            {
                new() { Id = Guid.NewGuid(), Name = "Summer Sale", DiscountPercentage = 10m, ValidFrom = validFrom, ValidTo = validTo, IsActive = true }
            }
        };

        yield return new object[]
        {
            new PromotionFilter(),
            new List<Common.Promotion.Promotion>
            {
                new() { Id = Guid.NewGuid(), Name = "Summer Sale", DiscountPercentage = 10m, ValidFrom = validFrom, ValidTo = validTo, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Festival Offer", DiscountPercentage = 15m, ValidFrom = validFrom, ValidTo = validTo, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Promotion.Promotion { Id = id, Name = "Summer Sale", DiscountPercentage = 10m, ValidFrom = DateTimeOffset.UtcNow, ValidTo = DateTimeOffset.UtcNow.AddDays(30), IsActive = true }
        };
    }
}
