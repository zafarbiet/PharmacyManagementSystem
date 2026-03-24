using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000023)]
public class AddExpiryAlertConfigurationTable : Migration
{
    public override void Up()
    {
        Create.Table("ExpiryAlertConfiguration")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("ThresholdDays").AsInt32().NotNullable()
            .WithColumn("AlertType").AsString(100).Nullable()
            .WithColumn("IsEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
    }

    public override void Down()
    {
        Delete.Table("ExpiryAlertConfiguration").InSchema("PMS");
    }
}
