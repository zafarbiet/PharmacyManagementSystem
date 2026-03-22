using Dapper;
using PharmacyManagementSystem.Common.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DailyDiaryEntry;

public static class DailyDiaryEntryDatabaseCommandText
{
    private const string SelectColumns = "Id, EntryDate, Category, Title, Body, DrugId, VendorId, PatientId, ReferenceId, ReferenceType, Priority, CreatedBy, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(DailyDiaryEntryFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.DailyDiaryEntries WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            sql += " AND Category = @Category";
            parameters.Add("Category", filter.Category);
        }

        if (filter.DrugId != Guid.Empty)
        {
            sql += " AND DrugId = @DrugId";
            parameters.Add("DrugId", filter.DrugId);
        }

        if (filter.PatientId != Guid.Empty)
        {
            sql += " AND PatientId = @PatientId";
            parameters.Add("PatientId", filter.PatientId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Priority))
        {
            sql += " AND Priority = @Priority";
            parameters.Add("Priority", filter.Priority);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.DailyDiaryEntries WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.DailyDiaryEntry.DailyDiaryEntry dailyDiaryEntry)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        var parameters = new DynamicParameters();
        parameters.Add("EntryDate", dailyDiaryEntry.EntryDate);
        parameters.Add("Category", dailyDiaryEntry.Category);
        parameters.Add("Title", dailyDiaryEntry.Title);
        parameters.Add("Body", dailyDiaryEntry.Body);
        parameters.Add("DrugId", dailyDiaryEntry.DrugId);
        parameters.Add("VendorId", dailyDiaryEntry.VendorId);
        parameters.Add("PatientId", dailyDiaryEntry.PatientId);
        parameters.Add("ReferenceId", dailyDiaryEntry.ReferenceId);
        parameters.Add("ReferenceType", dailyDiaryEntry.ReferenceType);
        parameters.Add("Priority", dailyDiaryEntry.Priority);
        parameters.Add("CreatedBy", dailyDiaryEntry.CreatedBy);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", dailyDiaryEntry.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.DailyDiaryEntries (Id, EntryDate, Category, Title, Body, DrugId, VendorId, PatientId, ReferenceId, ReferenceType, Priority, CreatedBy, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @EntryDate, @Category, @Title, @Body, @DrugId, @VendorId, @PatientId, @ReferenceId, @ReferenceType, @Priority, @CreatedBy, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.DailyDiaryEntry.DailyDiaryEntry dailyDiaryEntry)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        var parameters = new DynamicParameters();
        parameters.Add("Id", dailyDiaryEntry.Id);
        parameters.Add("EntryDate", dailyDiaryEntry.EntryDate);
        parameters.Add("Category", dailyDiaryEntry.Category);
        parameters.Add("Title", dailyDiaryEntry.Title);
        parameters.Add("Body", dailyDiaryEntry.Body);
        parameters.Add("DrugId", dailyDiaryEntry.DrugId);
        parameters.Add("VendorId", dailyDiaryEntry.VendorId);
        parameters.Add("PatientId", dailyDiaryEntry.PatientId);
        parameters.Add("ReferenceId", dailyDiaryEntry.ReferenceId);
        parameters.Add("ReferenceType", dailyDiaryEntry.ReferenceType);
        parameters.Add("Priority", dailyDiaryEntry.Priority);
        parameters.Add("CreatedBy", dailyDiaryEntry.CreatedBy);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", dailyDiaryEntry.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.DailyDiaryEntries
                             SET EntryDate = @EntryDate, Category = @Category, Title = @Title, Body = @Body,
                                 DrugId = @DrugId, VendorId = @VendorId, PatientId = @PatientId,
                                 ReferenceId = @ReferenceId, ReferenceType = @ReferenceType,
                                 Priority = @Priority, CreatedBy = @CreatedBy,
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
            SqlStatement = @"UPDATE PMS.DailyDiaryEntries
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
