using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000021)]
public class AddRacksTable : Migration
{
    public override void Up()
    {
        Create.Table("Racks")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("StorageZoneId").AsGuid().NotNullable()
            .WithColumn("Label").AsString(100).Nullable()
            .WithColumn("Description").AsString(500).Nullable()
            .WithColumn("Capacity").AsInt32().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Racks_StorageZoneId")
            .OnTable("Racks").InSchema("PMS")
            .OnColumn("StorageZoneId").Ascending();

        Create.Index("UX_Racks_StorageZoneId_Label")
            .OnTable("Racks").InSchema("PMS")
            .OnColumn("StorageZoneId").Ascending()
            .OnColumn("Label").Ascending()
            .WithOptions().Unique();

        Create.ForeignKey("FK_Racks_StorageZones_StorageZoneId")
            .FromTable("Racks").InSchema("PMS").ForeignColumn("StorageZoneId")
            .ToTable("StorageZones").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Racks_StorageZones_StorageZoneId").OnTable("Racks").InSchema("PMS");
        Delete.Index("UX_Racks_StorageZoneId_Label").OnTable("Racks").InSchema("PMS");
        Delete.Index("IX_Racks_StorageZoneId").OnTable("Racks").InSchema("PMS");
        Delete.Table("Racks").InSchema("PMS");
    }
}
