namespace PharmacyManagementSystem.Common.PurchaseOrderItem;

public class PurchaseOrderItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid DrugId { get; set; }
    public int QuantityOrdered { get; set; }
    public int QuantityReceived { get; set; }
    public decimal UnitPrice { get; set; }
    public string? BatchNumber { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
}
