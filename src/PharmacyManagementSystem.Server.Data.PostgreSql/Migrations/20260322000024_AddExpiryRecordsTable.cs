using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000024)]
public class AddExpiryRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("ExpiryRecords")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugInventoryId").AsGuid().NotNullable()
            .WithColumn("DetectedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("ExpirationDate").AsDateTimeOffset().NotNullable()
            .WithColumn("QuantityAffected").AsInt32().NotNullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("InitiatedBy").AsString(200).Nullable()
            .WithColumn("ApprovedBy").AsString(200).Nullable()
            .WithColumn("ApprovedAt").AsDateTimeOffset().Nullable()
            .WithColumn("QuarantineRackId").AsGuid().Nullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_ExpiryRecords_DrugInventoryId")
            .OnTable("ExpiryRecords").InSchema("PMS")
            .OnColumn("DrugInventoryId").Ascending();

        Create.Index("IX_ExpiryRecords_Status")
            .OnTable("ExpiryRecords").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.Index("IX_ExpiryRecords_DetectedAt")
            .OnTable("ExpiryRecords").InSchema("PMS")
            .OnColumn("DetectedAt").Ascending();

        Create.ForeignKey("FK_ExpiryRecords_DrugInventory_DrugInventoryId")
            .FromTable("ExpiryRecords").InSchema("PMS").ForeignColumn("DrugInventoryId")
            .ToTable("DrugInventory").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_ExpiryRecords_Racks_QuarantineRackId")
            .FromTable("ExpiryRecords").InSchema("PMS").ForeignColumn("QuarantineRackId")
            .ToTable("Racks").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_ExpiryRecords_Racks_QuarantineRackId").OnTable("ExpiryRecords").InSchema("PMS");
        Delete.ForeignKey("FK_ExpiryRecords_DrugInventory_DrugInventoryId").OnTable("ExpiryRecords").InSchema("PMS");
        Delete.Index("IX_ExpiryRecords_DetectedAt").OnTable("ExpiryRecords").InSchema("PMS");
        Delete.Index("IX_ExpiryRecords_Status").OnTable("ExpiryRecords").InSchema("PMS");
        Delete.Index("IX_ExpiryRecords_DrugInventoryId").OnTable("ExpiryRecords").InSchema("PMS");
        Delete.Table("ExpiryRecords").InSchema("PMS");
    }
}
