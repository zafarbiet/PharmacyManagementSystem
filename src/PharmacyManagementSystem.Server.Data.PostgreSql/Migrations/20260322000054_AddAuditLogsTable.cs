using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000054)]
public class AddAuditLogsTable : Migration
{
    public override void Up()
    {
        Create.Table("AuditLogs").InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("DrugName").AsString(200).Nullable()
            .WithColumn("ScheduleCategory").AsString(10).Nullable()
            .WithColumn("CustomerInvoiceId").AsGuid().NotNullable()
            .WithColumn("PrescriptionId").AsGuid().Nullable()
            .WithColumn("PatientId").AsGuid().Nullable()
            .WithColumn("QuantityDispensed").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("PerformedBy").AsString(100).Nullable()
            .WithColumn("PerformedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("RetentionUntil").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedBy").AsString(100).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
    }

    public override void Down()
    {
        Delete.Table("AuditLogs").InSchema("PMS");
    }
}
