using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000038)]
public class AddDamageRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("DamageRecords")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("RecordDate").AsDateTimeOffset().NotNullable()
            .WithColumn("QuantityDamaged").AsInt32().NotNullable()
            .WithColumn("Reason").AsString(500).Nullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DamageRecords_DrugId")
            .OnTable("DamageRecords").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.Index("IX_DamageRecords_Status")
            .OnTable("DamageRecords").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_DamageRecords_Drugs_DrugId")
            .FromTable("DamageRecords").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DamageRecords_Drugs_DrugId").OnTable("DamageRecords").InSchema("PMS");
        Delete.Index("IX_DamageRecords_Status").OnTable("DamageRecords").InSchema("PMS");
        Delete.Index("IX_DamageRecords_DrugId").OnTable("DamageRecords").InSchema("PMS");
        Delete.Table("DamageRecords").InSchema("PMS");
    }
}
