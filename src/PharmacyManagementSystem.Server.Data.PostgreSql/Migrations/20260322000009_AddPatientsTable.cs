using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000009)]
public class AddPatientsTable : Migration
{
    public override void Up()
    {
        Create.Table("Patients")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("ContactNumber").AsString(20).Nullable()
            .WithColumn("Email").AsString(200).Nullable()
            .WithColumn("Address").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Patients_Name")
            .OnTable("Patients").InSchema("PMS")
            .OnColumn("Name").Ascending();

        Create.Index("IX_Patients_ContactNumber")
            .OnTable("Patients").InSchema("PMS")
            .OnColumn("ContactNumber").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Patients_ContactNumber").OnTable("Patients").InSchema("PMS");
        Delete.Index("IX_Patients_Name").OnTable("Patients").InSchema("PMS");
        Delete.Table("Patients").InSchema("PMS");
    }
}
