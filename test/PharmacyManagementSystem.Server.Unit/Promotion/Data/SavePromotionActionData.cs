namespace PharmacyManagementSystem.Server.Unit.Promotion.Data;

public static class SavePromotionActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var validFrom = DateTimeOffset.UtcNow.AddDays(-1);
        var validTo = DateTimeOffset.UtcNow.AddDays(30);
        yield return new object[]
        {
            new Common.Promotion.Promotion { Name = "Summer Sale", DiscountPercentage = 10m, ValidFrom = validFrom, ValidTo = validTo },
            new Common.Promotion.Promotion { Id = Guid.NewGuid(), Name = "Summer Sale", DiscountPercentage = 10m, ValidFrom = validFrom, ValidTo = validTo, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var validFrom = DateTimeOffset.UtcNow;
        var validTo = DateTimeOffset.UtcNow.AddDays(30);

        yield return new object[]
        {
            new Common.Promotion.Promotion { Name = string.Empty, DiscountPercentage = 10m, ValidFrom = validFrom, ValidTo = validTo }
        };

        yield return new object[]
        {
            new Common.Promotion.Promotion { Name = "Sale", DiscountPercentage = 0m, ValidFrom = validFrom, ValidTo = validTo }
        };

        yield return new object[]
        {
            new Common.Promotion.Promotion { Name = "Sale", DiscountPercentage = 101m, ValidFrom = validFrom, ValidTo = validTo }
        };

        yield return new object[]
        {
            // ValidTo before ValidFrom
            new Common.Promotion.Promotion { Name = "Sale", DiscountPercentage = 10m, ValidFrom = validTo, ValidTo = validFrom }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var validFrom = DateTimeOffset.UtcNow.AddDays(-5);
        var validTo = DateTimeOffset.UtcNow.AddDays(25);
        yield return new object[]
        {
            new Common.Promotion.Promotion { Id = id, Name = "Festival Offer", DiscountPercentage = 15m, ValidFrom = validFrom, ValidTo = validTo },
            new Common.Promotion.Promotion { Id = id, Name = "Festival Offer", DiscountPercentage = 15m, ValidFrom = validFrom, ValidTo = validTo, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
