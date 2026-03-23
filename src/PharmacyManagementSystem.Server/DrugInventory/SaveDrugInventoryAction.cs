using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Notification;

namespace PharmacyManagementSystem.Server.DrugInventory;

public class SaveDrugInventoryAction(
    ILogger<SaveDrugInventoryAction> logger,
    IDrugInventoryRepository repository,
    IDrugRepository drugRepository,
    IExpiryAlertConfigurationRepository expiryAlertConfigRepository,
    ISaveNotificationAction notificationAction) : ISaveDrugInventoryAction
{
    private const int DefaultExpiryThresholdDays = 90;

    private readonly ILogger<SaveDrugInventoryAction> _logger = logger;
    private readonly IDrugInventoryRepository _repository = repository;
    private readonly IDrugRepository _drugRepository = drugRepository;
    private readonly IExpiryAlertConfigurationRepository _expiryAlertConfigRepository = expiryAlertConfigRepository;
    private readonly ISaveNotificationAction _notificationAction = notificationAction;

    public async Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        if (drugInventory.DrugId == Guid.Empty)
            throw new BadRequestException("DrugInventory DrugId is required.");

        if (string.IsNullOrWhiteSpace(drugInventory.BatchNumber))
            throw new BadRequestException("DrugInventory BatchNumber is required.");

        if (drugInventory.QuantityInStock < 0)
            throw new BadRequestException("DrugInventory QuantityInStock must be >= 0.");

        drugInventory.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        var result = await _repository.AddAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        if (result is not null)
        {
            await CheckAndRaiseInventoryAlertsAsync(result, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        if (drugInventory.DrugId == Guid.Empty)
            throw new BadRequestException("DrugInventory DrugId is required.");

        if (string.IsNullOrWhiteSpace(drugInventory.BatchNumber))
            throw new BadRequestException("DrugInventory BatchNumber is required.");

        if (drugInventory.QuantityInStock < 0)
            throw new BadRequestException("DrugInventory QuantityInStock must be >= 0.");

        drugInventory.UpdatedBy = "system";

        _logger.LogDebug("Updating drug inventory with id: {Id}.", drugInventory.Id);

        var result = await _repository.UpdateAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug inventory with id: {Id}.", drugInventory.Id);

        if (result is not null)
        {
            await CheckAndRaiseInventoryAlertsAsync(result, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug inventory with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug inventory with id: {Id}.", id);
    }

    private async Task CheckAndRaiseInventoryAlertsAsync(
        Common.DrugInventory.DrugInventory inventory,
        CancellationToken cancellationToken)
    {
        await CheckReorderLevelAsync(inventory, cancellationToken).ConfigureAwait(false);
        await CheckExpiryAlertAsync(inventory, cancellationToken).ConfigureAwait(false);
    }

    private async Task CheckReorderLevelAsync(
        Common.DrugInventory.DrugInventory inventory,
        CancellationToken cancellationToken)
    {
        var drug = await _drugRepository.GetByIdAsync(inventory.DrugId.ToString(), cancellationToken)
            .ConfigureAwait(false);

        if (drug is null)
        {
            _logger.LogDebug("Drug {DrugId} not found; skipping reorder check.", inventory.DrugId);
            return;
        }

        var allBatches = await _repository.GetByFilterCriteriaAsync(
            new DrugInventoryFilter { DrugId = inventory.DrugId },
            cancellationToken).ConfigureAwait(false);

        var totalStock = allBatches?.Sum(b => b.QuantityInStock) ?? 0;

        if (totalStock <= drug.ReorderLevel)
        {
            _logger.LogDebug(
                "Drug {DrugName} stock {Stock} is at or below reorder level {ReorderLevel}. Creating ReorderAlert.",
                drug.Name, totalStock, drug.ReorderLevel);

            await _notificationAction.AddAsync(new Common.Notification.Notification
            {
                NotificationType = "ReorderAlert",
                Channel = "InApp",
                RecipientType = "Role",
                RecipientId = Guid.Empty,
                Subject = $"Reorder Alert: {drug.Name}",
                Body = $"Stock for '{drug.Name}' (batch: {inventory.BatchNumber}) has fallen to {totalStock} units, " +
                       $"which is at or below the reorder level of {drug.ReorderLevel}.",
                ReferenceId = inventory.DrugId,
                ReferenceType = "Drug",
                ScheduledAt = DateTimeOffset.UtcNow,
                Status = "Pending"
            }, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task CheckExpiryAlertAsync(
        Common.DrugInventory.DrugInventory inventory,
        CancellationToken cancellationToken)
    {
        var thresholdDays = await GetExpiryThresholdDaysAsync(cancellationToken).ConfigureAwait(false);
        var alertDate = DateTimeOffset.UtcNow.AddDays(thresholdDays);

        if (inventory.ExpirationDate > alertDate)
            return;

        _logger.LogDebug(
            "Batch {BatchNumber} for drug {DrugId} expires on {ExpiryDate}, within {Days}-day threshold. Creating ExpiryAlert.",
            inventory.BatchNumber, inventory.DrugId, inventory.ExpirationDate, thresholdDays);

        await _notificationAction.AddAsync(new Common.Notification.Notification
        {
            NotificationType = "ExpiryAlert",
            Channel = "InApp",
            RecipientType = "Role",
            RecipientId = Guid.Empty,
            Subject = $"Expiry Alert: Batch {inventory.BatchNumber}",
            Body = $"Batch '{inventory.BatchNumber}' (drug id: {inventory.DrugId}) expires on " +
                   $"{inventory.ExpirationDate:yyyy-MM-dd}. Qty in stock: {inventory.QuantityInStock}.",
            ReferenceId = inventory.Id,
            ReferenceType = "DrugInventory",
            ScheduledAt = DateTimeOffset.UtcNow,
            Status = "Pending"
        }, cancellationToken).ConfigureAwait(false);
    }

    private async Task<int> GetExpiryThresholdDaysAsync(CancellationToken cancellationToken)
    {
        var configs = await _expiryAlertConfigRepository.GetByFilterCriteriaAsync(
            new ExpiryAlertConfigurationFilter { AlertType = "ExpiryAlert" },
            cancellationToken).ConfigureAwait(false);

        var activeConfig = configs?.FirstOrDefault(c => c.IsEnabled);
        return activeConfig?.ThresholdDays ?? DefaultExpiryThresholdDays;
    }
}
