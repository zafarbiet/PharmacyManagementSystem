using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000037)]
public class AddDebtRemindersTable : Migration
{
    public override void Up()
    {
        Create.Table("DebtReminders")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DebtRecordId").AsGuid().NotNullable()
            .WithColumn("ReminderDate").AsDateTimeOffset().NotNullable()
            .WithColumn("ReminderType").AsString(50).Nullable()
            .WithColumn("Message").AsString(500).Nullable()
            .WithColumn("IsSent").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DebtReminders_DebtRecordId")
            .OnTable("DebtReminders").InSchema("PMS")
            .OnColumn("DebtRecordId").Ascending();

        Create.ForeignKey("FK_DebtReminders_DebtRecords_DebtRecordId")
            .FromTable("DebtReminders").InSchema("PMS").ForeignColumn("DebtRecordId")
            .ToTable("DebtRecords").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DebtReminders_DebtRecords_DebtRecordId").OnTable("DebtReminders").InSchema("PMS");
        Delete.Index("IX_DebtReminders_DebtRecordId").OnTable("DebtReminders").InSchema("PMS");
        Delete.Table("DebtReminders").InSchema("PMS");
    }
}
