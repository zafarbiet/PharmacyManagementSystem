using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000020)]
public class AddStorageZonesTable : Migration
{
    public override void Up()
    {
        Create.Table("StorageZones")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).Nullable()
            .WithColumn("ZoneType").AsString(100).Nullable()
            .WithColumn("Description").AsString(500).Nullable()
            .WithColumn("TemperatureRangeMin").AsDecimal(18, 2).Nullable()
            .WithColumn("TemperatureRangeMax").AsDecimal(18, 2).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("UX_StorageZones_Name")
            .OnTable("StorageZones").InSchema("PMS")
            .OnColumn("Name").Ascending()
            .WithOptions().Unique();

        Create.Index("IX_StorageZones_ZoneType")
            .OnTable("StorageZones").InSchema("PMS")
            .OnColumn("ZoneType").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_StorageZones_ZoneType").OnTable("StorageZones").InSchema("PMS");
        Delete.Index("UX_StorageZones_Name").OnTable("StorageZones").InSchema("PMS");
        Delete.Table("StorageZones").InSchema("PMS");
    }
}
