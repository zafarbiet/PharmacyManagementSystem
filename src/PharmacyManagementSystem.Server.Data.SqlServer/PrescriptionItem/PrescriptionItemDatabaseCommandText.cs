using Dapper;
using PharmacyManagementSystem.Common.PrescriptionItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PrescriptionItem;

public static class PrescriptionItemDatabaseCommandText
{
    private const string SelectColumns = "Id, PrescriptionId, DrugId, Dosage, Quantity, Instructions, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(PrescriptionItemFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.PrescriptionItems WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.PrescriptionId.HasValue && filter.PrescriptionId.Value != Guid.Empty)
        {
            sql += " AND PrescriptionId = @PrescriptionId";
            parameters.Add("PrescriptionId", filter.PrescriptionId);
        }

        if (filter.DrugId.HasValue && filter.DrugId.Value != Guid.Empty)
        {
            sql += " AND DrugId = @DrugId";
            parameters.Add("DrugId", filter.DrugId);
        }

        if (filter.DateFrom.HasValue)
        {
            sql += " AND UpdatedAt >= @DateFrom";
            parameters.Add("DateFrom", filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            sql += " AND UpdatedAt <= @DateTo";
            parameters.Add("DateTo", filter.DateTo.Value);
        }

        sql += " AND IsActive = 1";

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = sql,
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.PrescriptionItems WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.PrescriptionItem.PrescriptionItem prescriptionItem)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        var parameters = new DynamicParameters();
        parameters.Add("PrescriptionId", prescriptionItem.PrescriptionId);
        parameters.Add("DrugId", prescriptionItem.DrugId);
        parameters.Add("Dosage", prescriptionItem.Dosage);
        parameters.Add("Quantity", prescriptionItem.Quantity);
        parameters.Add("Instructions", prescriptionItem.Instructions);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", prescriptionItem.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.PrescriptionItems (Id, PrescriptionId, DrugId, Dosage, Quantity, Instructions, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @PrescriptionId, @DrugId, @Dosage, @Quantity, @Instructions, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.PrescriptionItem.PrescriptionItem prescriptionItem)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        var parameters = new DynamicParameters();
        parameters.Add("Id", prescriptionItem.Id);
        parameters.Add("PrescriptionId", prescriptionItem.PrescriptionId);
        parameters.Add("DrugId", prescriptionItem.DrugId);
        parameters.Add("Dosage", prescriptionItem.Dosage);
        parameters.Add("Quantity", prescriptionItem.Quantity);
        parameters.Add("Instructions", prescriptionItem.Instructions);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", prescriptionItem.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.PrescriptionItems
                             SET PrescriptionId = @PrescriptionId, DrugId = @DrugId, Dosage = @Dosage,
                                 Quantity = @Quantity, Instructions = @Instructions,
                                 UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetSoftDeleteSql(Guid id, string updatedBy)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", updatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.PrescriptionItems
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
