using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Notification;

namespace PharmacyManagementSystem.Server.Host.Services;

public class NearExpiryBackgroundService(
    ILogger<NearExpiryBackgroundService> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private const int DefaultExpiryThresholdDays = 90;
    private static readonly TimeSpan RunInterval = TimeSpan.FromHours(24);

    private readonly ILogger<NearExpiryBackgroundService> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NearExpiryBackgroundService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunScanAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "NearExpiryBackgroundService: unhandled error during scan.");
            }

            try
            {
                await Task.Delay(RunInterval, stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("NearExpiryBackgroundService stopped.");
    }

    private async Task RunScanAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("NearExpiryBackgroundService: starting daily near-expiry scan.");

        await using var scope = _scopeFactory.CreateAsyncScope();

        var drugInventoryRepo = scope.ServiceProvider.GetRequiredService<IDrugInventoryRepository>();
        var expiryConfigRepo = scope.ServiceProvider.GetRequiredService<IExpiryAlertConfigurationRepository>();
        var notificationAction = scope.ServiceProvider.GetRequiredService<ISaveNotificationAction>();

        var configs = await expiryConfigRepo.GetByFilterCriteriaAsync(
            new ExpiryAlertConfigurationFilter { AlertType = "ExpiryAlert" },
            cancellationToken).ConfigureAwait(false);

        var thresholdDays = configs?.FirstOrDefault(c => c.IsEnabled)?.ThresholdDays ?? DefaultExpiryThresholdDays;
        var alertCutoff = DateTimeOffset.UtcNow.AddDays(thresholdDays);

        var allBatches = await drugInventoryRepo.GetByFilterCriteriaAsync(
            new DrugInventoryFilter(),
            cancellationToken).ConfigureAwait(false);

        var nearExpiry = (allBatches ?? [])
            .Where(b => b.QuantityInStock > 0 && b.ExpirationDate <= alertCutoff)
            .ToList();

        _logger.LogDebug("NearExpiryBackgroundService: {Count} near-expiry batches found (threshold: {Days} days).",
            nearExpiry.Count, thresholdDays);

        foreach (var batch in nearExpiry)
        {
            await notificationAction.AddAsync(new Common.Notification.Notification
            {
                NotificationType = "ExpiryAlert",
                Channel = "InApp",
                RecipientType = "Role",
                RecipientId = Guid.Empty,
                Subject = $"Expiry Alert: Batch {batch.BatchNumber}",
                Body = $"[Daily Scan] Batch '{batch.BatchNumber}' (drug id: {batch.DrugId}) expires on " +
                       $"{batch.ExpirationDate:yyyy-MM-dd}. Qty in stock: {batch.QuantityInStock}.",
                ReferenceId = batch.Id,
                ReferenceType = "DrugInventory",
                ScheduledAt = DateTimeOffset.UtcNow,
                Status = "Pending"
            }, cancellationToken).ConfigureAwait(false);
        }

        _logger.LogDebug("NearExpiryBackgroundService: scan complete. {Count} alerts raised.", nearExpiry.Count);
    }
}
