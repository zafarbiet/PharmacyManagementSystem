using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000005)]
public class UpdateDrugsTable : Migration
{
    public override void Up()
    {
        Alter.Table("Drugs").InSchema("PMS")
            .AddColumn("BrandName").AsString(200).Nullable()
            .AddColumn("DosageForm").AsString(100).Nullable()
            .AddColumn("Strength").AsString(100).Nullable()
            .AddColumn("Description").AsString(1000).Nullable()
            .AddColumn("DrugLicenseNumber").AsString(100).Nullable()
            .AddColumn("ApprovalDate").AsDateTimeOffset().Nullable()
            .AddColumn("ScheduleCategory").AsString(100).Nullable()
            .AddColumn("PrescriptionRequired").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Column("BrandName").FromTable("Drugs").InSchema("PMS");
        Delete.Column("DosageForm").FromTable("Drugs").InSchema("PMS");
        Delete.Column("Strength").FromTable("Drugs").InSchema("PMS");
        Delete.Column("Description").FromTable("Drugs").InSchema("PMS");
        Delete.Column("DrugLicenseNumber").FromTable("Drugs").InSchema("PMS");
        Delete.Column("ApprovalDate").FromTable("Drugs").InSchema("PMS");
        Delete.Column("ScheduleCategory").FromTable("Drugs").InSchema("PMS");
        Delete.Column("PrescriptionRequired").FromTable("Drugs").InSchema("PMS");
    }
}
