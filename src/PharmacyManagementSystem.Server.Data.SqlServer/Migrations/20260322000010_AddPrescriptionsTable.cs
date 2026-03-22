using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000010)]
public class AddPrescriptionsTable : Migration
{
    public override void Up()
    {
        Create.Table("Prescriptions")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PatientId").AsGuid().NotNullable()
            .WithColumn("PrescribingDoctor").AsString(200).Nullable()
            .WithColumn("PrescriptionDate").AsDateTimeOffset().NotNullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Prescriptions_PatientId")
            .OnTable("Prescriptions").InSchema("PMS")
            .OnColumn("PatientId").Ascending();

        Create.ForeignKey("FK_Prescriptions_Patients_PatientId")
            .FromTable("Prescriptions").InSchema("PMS").ForeignColumn("PatientId")
            .ToTable("Patients").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Prescriptions_Patients_PatientId").OnTable("Prescriptions").InSchema("PMS");
        Delete.Index("IX_Prescriptions_PatientId").OnTable("Prescriptions").InSchema("PMS");
        Delete.Table("Prescriptions").InSchema("PMS");
    }
}
