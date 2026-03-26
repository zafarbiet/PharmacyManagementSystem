using PharmacyManagementSystem.Common.AuditLog;

namespace PharmacyManagementSystem.Server.Unit.AuditLog.Data;

public static class GetAuditLogActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugId = Guid.NewGuid();
        var invoiceId = Guid.NewGuid();

        yield return new object[]
        {
            new AuditLogFilter { DrugId = drugId },
            new List<Common.AuditLog.AuditLog>
            {
                new() { Id = Guid.NewGuid(), DrugId = drugId, DrugName = "Paracetamol", ScheduleCategory = "H", QuantityDispensed = 10, PerformedBy = "pharmacist1", IsActive = true }
            }
        };

        yield return new object[]
        {
            new AuditLogFilter(),
            new List<Common.AuditLog.AuditLog>
            {
                new() { Id = Guid.NewGuid(), DrugId = drugId, DrugName = "Paracetamol", QuantityDispensed = 10, PerformedBy = "pharmacist1", IsActive = true },
                new() { Id = Guid.NewGuid(), DrugId = Guid.NewGuid(), DrugName = "Amoxicillin", QuantityDispensed = 5, PerformedBy = "pharmacist2", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.AuditLog.AuditLog { Id = id, DrugId = drugId, DrugName = "Paracetamol", QuantityDispensed = 10, PerformedBy = "pharmacist1", IsActive = true }
        };
    }
}
