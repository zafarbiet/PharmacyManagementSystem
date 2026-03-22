using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000039)]
public class AddDamageDisposalRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("DamageDisposalRecords")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DamageRecordId").AsGuid().NotNullable()
            .WithColumn("DisposalDate").AsDateTimeOffset().NotNullable()
            .WithColumn("DisposalMethod").AsString(100).Nullable()
            .WithColumn("DisposedBy").AsString(100).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DamageDisposalRecords_DamageRecordId")
            .OnTable("DamageDisposalRecords").InSchema("PMS")
            .OnColumn("DamageRecordId").Ascending();

        Create.ForeignKey("FK_DamageDisposalRecords_DamageRecords_DamageRecordId")
            .FromTable("DamageDisposalRecords").InSchema("PMS").ForeignColumn("DamageRecordId")
            .ToTable("DamageRecords").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DamageDisposalRecords_DamageRecords_DamageRecordId").OnTable("DamageDisposalRecords").InSchema("PMS");
        Delete.Index("IX_DamageDisposalRecords_DamageRecordId").OnTable("DamageDisposalRecords").InSchema("PMS");
        Delete.Table("DamageDisposalRecords").InSchema("PMS");
    }
}
