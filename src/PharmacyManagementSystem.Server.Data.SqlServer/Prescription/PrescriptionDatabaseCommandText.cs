using Dapper;
using PharmacyManagementSystem.Common.Prescription;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Prescription;

public static class PrescriptionDatabaseCommandText
{
    private const string SelectColumns = "Id, PatientId, PrescribingDoctor, PrescriptionDate, Notes, UpdatedAt, UpdatedBy, IsActive";

    public static Task<DatabaseSqlWithParameters> GetSelectSql(PrescriptionFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var sql = $"SELECT {SelectColumns} FROM PMS.Prescriptions WHERE 1=1";
        var parameters = new DynamicParameters();

        if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
        {
            sql += " AND Id = @Id";
            parameters.Add("Id", filter.Id);
        }

        if (filter.PatientId.HasValue && filter.PatientId.Value != Guid.Empty)
        {
            sql += " AND PatientId = @PatientId";
            parameters.Add("PatientId", filter.PatientId);
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
            SqlStatement = $"SELECT {SelectColumns} FROM PMS.Prescriptions WHERE Id = @Id AND IsActive = 1",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetInsertSql(Common.Prescription.Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        var parameters = new DynamicParameters();
        parameters.Add("PatientId", prescription.PatientId);
        parameters.Add("PrescribingDoctor", prescription.PrescribingDoctor);
        parameters.Add("PrescriptionDate", prescription.PrescriptionDate);
        parameters.Add("Notes", prescription.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", prescription.UpdatedBy);
        parameters.Add("IsActive", true);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"INSERT INTO PMS.Prescriptions (Id, PatientId, PrescribingDoctor, PrescriptionDate, Notes, UpdatedAt, UpdatedBy, IsActive)
                             OUTPUT INSERTED.*
                             VALUES (NEWID(), @PatientId, @PrescribingDoctor, @PrescriptionDate, @Notes, @UpdatedAt, @UpdatedBy, @IsActive)",
            Parameters = parameters
        });
    }

    public static Task<DatabaseSqlWithParameters> GetUpdateSql(Common.Prescription.Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        var parameters = new DynamicParameters();
        parameters.Add("Id", prescription.Id);
        parameters.Add("PatientId", prescription.PatientId);
        parameters.Add("PrescribingDoctor", prescription.PrescribingDoctor);
        parameters.Add("PrescriptionDate", prescription.PrescriptionDate);
        parameters.Add("Notes", prescription.Notes);
        parameters.Add("UpdatedAt", DateTimeOffset.UtcNow);
        parameters.Add("UpdatedBy", prescription.UpdatedBy);

        return Task.FromResult(new DatabaseSqlWithParameters
        {
            SqlStatement = @"UPDATE PMS.Prescriptions
                             SET PatientId = @PatientId, PrescribingDoctor = @PrescribingDoctor,
                                 PrescriptionDate = @PrescriptionDate, Notes = @Notes,
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
            SqlStatement = @"UPDATE PMS.Prescriptions
                             SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                             OUTPUT INSERTED.*
                             WHERE Id = @Id",
            Parameters = parameters
        });
    }
}
