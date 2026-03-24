using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260324000002)]
public class AlterDamageRecordsFixColumns : Migration
{
    public override void Up()
    {
        // Drop old FK and index on DrugId before renaming
        Delete.ForeignKey("FK_DamageRecords_Drugs_DrugId").OnTable("DamageRecords").InSchema("PMS");
        Delete.Index("IX_DamageRecords_DrugId").OnTable("DamageRecords").InSchema("PMS");

        // Rename columns to match entity/command text
        Rename.Column("DrugId").OnTable("DamageRecords").InSchema("PMS").To("DrugInventoryId");
        Rename.Column("RecordDate").OnTable("DamageRecords").InSchema("PMS").To("DamagedAt");
        Rename.Column("Reason").OnTable("DamageRecords").InSchema("PMS").To("DamageType");

        // Make DrugInventoryId nullable (was NOT NULL as DrugId) before we can null stale rows
        Alter.Column("DrugInventoryId").OnTable("DamageRecords").InSchema("PMS")
            .AsGuid().Nullable();

        // Alter DamagedAt to allow nullable (matches DateTimeOffset? in entity)
        Alter.Column("DamagedAt").OnTable("DamageRecords").InSchema("PMS")
            .AsDateTimeOffset().Nullable();

        // Alter DamageType to match string(100)
        Alter.Column("DamageType").OnTable("DamageRecords").InSchema("PMS")
            .AsString(100).Nullable();

        // Add missing columns
        Create.Column("DiscoveredBy").OnTable("DamageRecords").InSchema("PMS")
            .AsString(200).Nullable();
        Create.Column("QuarantineRackId").OnTable("DamageRecords").InSchema("PMS")
            .AsGuid().Nullable();
        Create.Column("StockTransactionId").OnTable("DamageRecords").InSchema("PMS")
            .AsGuid().Nullable();
        Create.Column("ApprovedBy").OnTable("DamageRecords").InSchema("PMS")
            .AsString(200).Nullable();
        Create.Column("ApprovedAt").OnTable("DamageRecords").InSchema("PMS")
            .AsDateTimeOffset().Nullable();

        // Clear stale DrugId values that don't exist in DrugInventory
        Execute.Sql("UPDATE [PMS].[DamageRecords] SET [DrugInventoryId] = NULL WHERE [DrugInventoryId] NOT IN (SELECT [Id] FROM [PMS].[DrugInventory])");

        // New index on DrugInventoryId
        Create.Index("IX_DamageRecords_DrugInventoryId")
            .OnTable("DamageRecords").InSchema("PMS")
            .OnColumn("DrugInventoryId").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_DamageRecords_DrugInventoryId").OnTable("DamageRecords").InSchema("PMS");

        Delete.Column("ApprovedAt").FromTable("DamageRecords").InSchema("PMS");
        Delete.Column("ApprovedBy").FromTable("DamageRecords").InSchema("PMS");
        Delete.Column("StockTransactionId").FromTable("DamageRecords").InSchema("PMS");
        Delete.Column("QuarantineRackId").FromTable("DamageRecords").InSchema("PMS");
        Delete.Column("DiscoveredBy").FromTable("DamageRecords").InSchema("PMS");

        Rename.Column("DamageType").OnTable("DamageRecords").InSchema("PMS").To("Reason");
        Rename.Column("DamagedAt").OnTable("DamageRecords").InSchema("PMS").To("RecordDate");
        Rename.Column("DrugInventoryId").OnTable("DamageRecords").InSchema("PMS").To("DrugId");

        Create.Index("IX_DamageRecords_DrugId")
            .OnTable("DamageRecords").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.ForeignKey("FK_DamageRecords_Drugs_DrugId")
            .FromTable("DamageRecords").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }
}
