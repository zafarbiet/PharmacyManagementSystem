using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000040)]
public class AddDailyDiaryEntriesTable : Migration
{
    public override void Up()
    {
        Create.Table("DailyDiaryEntries")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("EntryDate").AsDateTimeOffset().NotNullable()
            .WithColumn("Title").AsString(200).Nullable()
            .WithColumn("Content").AsString(int.MaxValue).Nullable()
            .WithColumn("Category").AsString(100).Nullable()
            .WithColumn("CreatedBy").AsString(100).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DailyDiaryEntries_EntryDate")
            .OnTable("DailyDiaryEntries").InSchema("PMS")
            .OnColumn("EntryDate").Ascending();

        Create.Index("IX_DailyDiaryEntries_Category")
            .OnTable("DailyDiaryEntries").InSchema("PMS")
            .OnColumn("Category").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_DailyDiaryEntries_Category").OnTable("DailyDiaryEntries").InSchema("PMS");
        Delete.Index("IX_DailyDiaryEntries_EntryDate").OnTable("DailyDiaryEntries").InSchema("PMS");
        Delete.Table("DailyDiaryEntries").InSchema("PMS");
    }
}
