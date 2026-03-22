using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000035)]
public class AddDebtRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("DebtRecords")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PatientId").AsGuid().NotNullable()
            .WithColumn("DebtDate").AsDateTimeOffset().NotNullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("RemainingAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DebtRecords_PatientId")
            .OnTable("DebtRecords").InSchema("PMS")
            .OnColumn("PatientId").Ascending();

        Create.Index("IX_DebtRecords_Status")
            .OnTable("DebtRecords").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_DebtRecords_Patients_PatientId")
            .FromTable("DebtRecords").InSchema("PMS").ForeignColumn("PatientId")
            .ToTable("Patients").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DebtRecords_Patients_PatientId").OnTable("DebtRecords").InSchema("PMS");
        Delete.Index("IX_DebtRecords_Status").OnTable("DebtRecords").InSchema("PMS");
        Delete.Index("IX_DebtRecords_PatientId").OnTable("DebtRecords").InSchema("PMS");
        Delete.Table("DebtRecords").InSchema("PMS");
    }
}
