using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000032)]
public class AddCustomerSubscriptionsTable : Migration
{
    public override void Up()
    {
        Create.Table("CustomerSubscriptions")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PatientId").AsGuid().NotNullable()
            .WithColumn("StartDate").AsDateTimeOffset().NotNullable()
            .WithColumn("EndDate").AsDateTimeOffset().Nullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_CustomerSubscriptions_PatientId")
            .OnTable("CustomerSubscriptions").InSchema("PMS")
            .OnColumn("PatientId").Ascending();

        Create.Index("IX_CustomerSubscriptions_Status")
            .OnTable("CustomerSubscriptions").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_CustomerSubscriptions_Patients_PatientId")
            .FromTable("CustomerSubscriptions").InSchema("PMS").ForeignColumn("PatientId")
            .ToTable("Patients").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_CustomerSubscriptions_Patients_PatientId").OnTable("CustomerSubscriptions").InSchema("PMS");
        Delete.Index("IX_CustomerSubscriptions_Status").OnTable("CustomerSubscriptions").InSchema("PMS");
        Delete.Index("IX_CustomerSubscriptions_PatientId").OnTable("CustomerSubscriptions").InSchema("PMS");
        Delete.Table("CustomerSubscriptions").InSchema("PMS");
    }
}
