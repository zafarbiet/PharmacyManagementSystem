using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment.Data;

public static class GetDrugInventoryRackAssignmentActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new DrugInventoryRackAssignmentFilter { DrugInventoryId = drugInventoryId },
            new List<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, RackId = Guid.NewGuid(), QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };

        yield return new object[]
        {
            new DrugInventoryRackAssignmentFilter(),
            new List<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, RackId = Guid.NewGuid(), QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow, IsActive = true },
                new() { Id = Guid.NewGuid(), DrugInventoryId = Guid.NewGuid(), RackId = Guid.NewGuid(), QuantityPlaced = 25, PlacedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { Id = id, DrugInventoryId = Guid.NewGuid(), RackId = Guid.NewGuid(), QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow, IsActive = true }
        };
    }
}
