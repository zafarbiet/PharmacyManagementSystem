namespace PharmacyManagementSystem.Server.Unit.DrugInventoryRackAssignment.Data;

public static class SaveDrugInventoryRackAssignmentActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugInventoryId = Guid.NewGuid();
        var rackId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { DrugInventoryId = drugInventoryId, RackId = rackId, QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow },
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, RackId = rackId, QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { DrugInventoryId = Guid.Empty, RackId = Guid.NewGuid(), QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { DrugInventoryId = Guid.NewGuid(), RackId = Guid.Empty, QuantityPlaced = 50, PlacedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { DrugInventoryId = Guid.NewGuid(), RackId = Guid.NewGuid(), QuantityPlaced = 0, PlacedAt = DateTimeOffset.UtcNow }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var drugInventoryId = Guid.NewGuid();
        var rackId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { Id = id, DrugInventoryId = drugInventoryId, RackId = rackId, QuantityPlaced = 75, PlacedAt = DateTimeOffset.UtcNow },
            new Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment { Id = id, DrugInventoryId = drugInventoryId, RackId = rackId, QuantityPlaced = 75, PlacedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
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
