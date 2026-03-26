using Dapper;
using PharmacyManagementSystem.Common.DrugUsage;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugUsage;

public static class DrugUsageDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugId, DosageInstructions, Indications, Contraindications, SideEffects, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugUsageFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DrugUsage WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
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

        sql += " AND IsActive = true";

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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DrugUsage WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DrugUsage.DrugUsage drugUsage)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        var parameters = new DynamicParameters();
        parameters.Add("DrugId", drugUsage.DrugId);
        parameters.Add("DosageInstructions", drugUsage.DosageInstructions);
        parameters.Add("Indications", drugUsage.Indications);
        parameters.Add("Contraindications", drugUsage.Contraindications);
        parameters.Add("SideEffects", drugUsage.SideEffects);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugUsage.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DrugUsage (Id, DrugId, DosageInstructions, Indications, Contraindications, SideEffects, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @DrugId, @DosageInstructions, @Indications, @Contraindications, @SideEffects, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DrugUsage.DrugUsage drugUsage)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        var parameters = new DynamicParameters();
        parameters.Add("Id", drugUsage.Id);
        parameters.Add("DrugId", drugUsage.DrugId);
        parameters.Add("DosageInstructions", drugUsage.DosageInstructions);
        parameters.Add("Indications", drugUsage.Indications);
        parameters.Add("Contraindications", drugUsage.Contraindications);
        parameters.Add("SideEffects", drugUsage.SideEffects);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugUsage.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DrugUsage
                             SET DrugId = @DrugId, DosageInstructions = @DosageInstructions, Indications = @Indications,
                                 Contraindications = @Contraindications, SideEffects = @SideEffects,
                                 UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
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
            SqlStatement = @"UPDATE PMS.DrugUsage
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
