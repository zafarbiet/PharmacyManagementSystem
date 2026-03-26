using Dapper;
using PharmacyManagementSystem.Common.DrugPricing;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugPricing;

public static class DrugPricingDatabaseCommandText
{
    private const string SelectColumns = "Id, DrugId, CostPrice, SellingPrice, Discount, GstRate, EffectiveFrom, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DrugPricingFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DrugPricing WHERE 1=1";
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DrugPricing WHERE Id = @Id AND IsActive = true",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DrugPricing.DrugPricing drugPricing)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        var parameters = new DynamicParameters();
        parameters.Add("DrugId", drugPricing.DrugId);
        parameters.Add("CostPrice", drugPricing.CostPrice);
        parameters.Add("SellingPrice", drugPricing.SellingPrice);
        parameters.Add("Discount", drugPricing.Discount);
        parameters.Add("GstRate", drugPricing.GstRate);
        parameters.Add("EffectiveFrom", drugPricing.EffectiveFrom);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugPricing.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DrugPricing (Id, DrugId, CostPrice, SellingPrice, Discount, GstRate, EffectiveFrom, UpdatedAt, UpdatedBy, IsActive)
                             RETURNING *
                             VALUES (gen_random_uuid(), @DrugId, @CostPrice, @SellingPrice, @Discount, @GstRate, @EffectiveFrom, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DrugPricing.DrugPricing drugPricing)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        var parameters = new DynamicParameters();
        parameters.Add("Id", drugPricing.Id);
        parameters.Add("DrugId", drugPricing.DrugId);
        parameters.Add("CostPrice", drugPricing.CostPrice);
        parameters.Add("SellingPrice", drugPricing.SellingPrice);
        parameters.Add("Discount", drugPricing.Discount);
        parameters.Add("GstRate", drugPricing.GstRate);
        parameters.Add("EffectiveFrom", drugPricing.EffectiveFrom);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", drugPricing.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DrugPricing
                             SET DrugId = @DrugId, CostPrice = @CostPrice, SellingPrice = @SellingPrice,
                                 Discount = @Discount, GstRate = @GstRate, EffectiveFrom = @EffectiveFrom,
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
            SqlStatement = @"UPDATE PMS.DrugPricing
                             SET IsActive = false, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             RETURNING *
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
