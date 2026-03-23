using Dapper;
using PharmacyManagementSystem.Common.ExpiryAlertConfiguration;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.ExpiryAlertConfiguration;

public static class ExpiryAlertConfigurationDatabaseCommandText
{
    private const string SelectColumns = "Id, ThresholdDays, AlertType, IsEnabled, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(ExpiryAlertConfigurationFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.ExpiryAlertConfiguration WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.AlertType))
        {
            sql += " AND AlertType = @AlertType";
            parameters.Add("AlertType", filter.AlertType);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.ExpiryAlertConfiguration WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expiryAlertConfiguration)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        var parameters = new DynamicParameters();
        parameters.Add("ThresholdDays", expiryAlertConfiguration.ThresholdDays);
        parameters.Add("AlertType", expiryAlertConfiguration.AlertType);
        parameters.Add("IsEnabled", expiryAlertConfiguration.IsEnabled);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", expiryAlertConfiguration.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.ExpiryAlertConfiguration (Id, ThresholdDays, AlertType, IsEnabled, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @ThresholdDays, @AlertType, @IsEnabled, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.ExpiryAlertConfiguration.ExpiryAlertConfiguration expiryAlertConfiguration)
    {
        ArgumentNullException.ThrowIfNull(expiryAlertConfiguration);

        var parameters = new DynamicParameters();
        parameters.Add("Id", expiryAlertConfiguration.Id);
        parameters.Add("ThresholdDays", expiryAlertConfiguration.ThresholdDays);
        parameters.Add("AlertType", expiryAlertConfiguration.AlertType);
        parameters.Add("IsEnabled", expiryAlertConfiguration.IsEnabled);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", expiryAlertConfiguration.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.ExpiryAlertConfiguration
                             SET ThresholdDays = @ThresholdDays, AlertType = @AlertType, IsEnabled = @IsEnabled,
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
            SqlStatement = @"UPDATE PMS.ExpiryAlertConfiguration
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
