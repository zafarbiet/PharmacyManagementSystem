using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000046)]
public class AlterPrescriptionsAddDoctorRegPatientAge : Migration
{
    public override void Up()
    {
        Alter.Table("Prescriptions").InSchema("PMS")
            .AddColumn("DoctorRegistrationNumber").AsString(50).Nullable()
            .AddColumn("PatientAge").AsInt32().Nullable();
    }

    public override void Down()
    {
        Delete.Column("DoctorRegistrationNumber").FromTable("Prescriptions").InSchema("PMS");
        Delete.Column("PatientAge").FromTable("Prescriptions").InSchema("PMS");
    }
}
