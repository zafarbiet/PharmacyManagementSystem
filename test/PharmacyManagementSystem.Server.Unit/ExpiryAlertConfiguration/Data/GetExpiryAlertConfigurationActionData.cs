using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;

namespace PharmacyManagementSystem.Server.Unit.ExpiryAlertConfiguration.Data;

public static class GetExpiryAlertConfigurationActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new ExpiryAlertConfigurationFilter { AlertType = "Email" },
            new List<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>
            {
                new() { Id = Guid.NewGuid(), ThresholdDays = 30, AlertType = "Email", IsEnabled = true, IsActive = true }
            }
        };

        yield return new object[]
        {
            new ExpiryAlertConfigurationFilter(),
            new List<Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration>
            {
                new() { Id = Guid.NewGuid(), ThresholdDays = 30, AlertType = "Email", IsEnabled = true, IsActive = true },
                new() { Id = Guid.NewGuid(), ThresholdDays = 60, AlertType = "SMS", IsEnabled = false, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration { Id = id, ThresholdDays = 30, AlertType = "Email", IsEnabled = true, IsActive = true }
        };
    }
}
