using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000025)]
public class AddDisposalRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table("DisposalRecords")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("ExpiryRecordId").AsGuid().NotNullable()
            .WithColumn("DisposedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("QuantityDisposed").AsInt32().NotNullable()
            .WithColumn("DisposalMethod").AsString(200).Nullable()
            .WithColumn("DisposedBy").AsString(200).Nullable()
            .WithColumn("WitnessedBy").AsString(200).Nullable()
            .WithColumn("RegulatoryReferenceNumber").AsString(100).Nullable()
            .WithColumn("Notes").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DisposalRecords_ExpiryRecordId")
            .OnTable("DisposalRecords").InSchema("PMS")
            .OnColumn("ExpiryRecordId").Ascending();

        Create.Index("IX_DisposalRecords_DisposedAt")
            .OnTable("DisposalRecords").InSchema("PMS")
            .OnColumn("DisposedAt").Ascending();

        Create.ForeignKey("FK_DisposalRecords_ExpiryRecords_ExpiryRecordId")
            .FromTable("DisposalRecords").InSchema("PMS").ForeignColumn("ExpiryRecordId")
            .ToTable("ExpiryRecords").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DisposalRecords_ExpiryRecords_ExpiryRecordId").OnTable("DisposalRecords").InSchema("PMS");
        Delete.Index("IX_DisposalRecords_DisposedAt").OnTable("DisposalRecords").InSchema("PMS");
        Delete.Index("IX_DisposalRecords_ExpiryRecordId").OnTable("DisposalRecords").InSchema("PMS");
        Delete.Table("DisposalRecords").InSchema("PMS");
    }
}
