using Dapper;
using PharmacyManagementSystem.Common.Promotion;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Promotion;

public static class PromotionDatabaseCommandText
{
    private const string SelectColumns = "Id, Name, Description, DiscountPercentage, MaxDiscountAmount, ValidFrom, ValidTo, ApplicableDrugId, ApplicableCategoryId, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(PromotionFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Promotions WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            sql += " AND Name LIKE @Name";
            parameters.Add("Name", $"%{filter.Name}%");
        }

        if (filter.ApplicableDrugId.HasValue && filter.ApplicableDrugId.Value != Guid.Empty)
        {
            sql += " AND ApplicableDrugId = @ApplicableDrugId";
            parameters.Add("ApplicableDrugId", filter.ApplicableDrugId);
        }

        if (filter.ApplicableCategoryId.HasValue && filter.ApplicableCategoryId.Value != Guid.Empty)
        {
            sql += " AND ApplicableCategoryId = @ApplicableCategoryId";
            parameters.Add("ApplicableCategoryId", filter.ApplicableCategoryId);
        }

        if (filter.ActiveOnly == true)
        {
            sql += " AND ValidFrom <= GETUTCDATE() AND ValidTo >= GETUTCDATE()";
        }

        sql += " AND IsActive = 1";

        return Task.FromResult(new DatabaseSqlWithParameters { SqlStatement = sql, Parameters = parameters });
    }

    public static Task<DatabaseSqlWithParameters> GetSelectByIdSql(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Promotions WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Promotion.Promotion promotion)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        var parameters = new DynamicParameters();
        parameters.Add("Name", promotion.Name);
        parameters.Add("Description", promotion.Description);
        parameters.Add("DiscountPercentage", promotion.DiscountPercentage);
        parameters.Add("MaxDiscountAmount", promotion.MaxDiscountAmount);
        parameters.Add("ValidFrom", promotion.ValidFrom);
        parameters.Add("ValidTo", promotion.ValidTo);
        parameters.Add("ApplicableDrugId", promotion.ApplicableDrugId);
        parameters.Add("ApplicableCategoryId", promotion.ApplicableCategoryId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", promotion.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Promotions (Id, Name, Description, DiscountPercentage, MaxDiscountAmount, ValidFrom, ValidTo, ApplicableDrugId, ApplicableCategoryId, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @Name, @Description, @DiscountPercentage, @MaxDiscountAmount, @ValidFrom, @ValidTo, @ApplicableDrugId, @ApplicableCategoryId, @UpdatedAt, @UpdatedBy, 1)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Promotion.Promotion promotion)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        var parameters = new DynamicParameters();
        parameters.Add("Id", promotion.Id);
        parameters.Add("Name", promotion.Name);
        parameters.Add("Description", promotion.Description);
        parameters.Add("DiscountPercentage", promotion.DiscountPercentage);
        parameters.Add("MaxDiscountAmount", promotion.MaxDiscountAmount);
        parameters.Add("ValidFrom", promotion.ValidFrom);
        parameters.Add("ValidTo", promotion.ValidTo);
        parameters.Add("ApplicableDrugId", promotion.ApplicableDrugId);
        parameters.Add("ApplicableCategoryId", promotion.ApplicableCategoryId);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", promotion.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Promotions
                             SET Name = @Name, Description = @Description, DiscountPercentage = @DiscountPercentage,
                                 MaxDiscountAmount = @MaxDiscountAmount, ValidFrom = @ValidFrom, ValidTo = @ValidTo,
                                 ApplicableDrugId = @ApplicableDrugId, ApplicableCategoryId = @ApplicableCategoryId,
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
            SqlStatement = @"UPDATE PMS.Promotions SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
