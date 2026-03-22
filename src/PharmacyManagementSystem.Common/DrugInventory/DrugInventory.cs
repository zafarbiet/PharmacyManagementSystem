namespace PharmacyManagementSystem.Common.DrugInventory;

public class DrugInventory : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugId { get; set; }
    public string? BatchNumber { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public int QuantityInStock { get; set; }
    public string? StorageConditions { get; set; }
}
