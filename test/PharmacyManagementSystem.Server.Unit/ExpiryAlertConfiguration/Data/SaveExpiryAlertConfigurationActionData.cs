namespace PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration.Data;

public static class SaveExpiryAlertConfigurationActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { ThresholdDays = 30, AlertType = "Email", IsEnabled = true },
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { Id = Guid.NewGuid(), ThresholdDays = 30, AlertType = "Email", IsEnabled = true, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { ThresholdDays = 0, AlertType = "Email" }
        };

        yield return new object[]
        {
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { ThresholdDays = 30, AlertType = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { Id = id, ThresholdDays = 45, AlertType = "SMS", IsEnabled = true },
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { Id = id, ThresholdDays = 45, AlertType = "SMS", IsEnabled = true, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            Guid.NewGuid(),
            "system"
        };
    }
}
