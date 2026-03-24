using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260324000004)]
public class AlterNotificationsFixSchema : Migration
{
    public override void Up()
    {
        // Drop old index on RecipientId (string) before altering
        Delete.Index("IX_Notifications_RecipientId").OnTable("Notifications").InSchema("PMS");

        // Rename Title -> Subject, Message -> Body
        Rename.Column("Title").OnTable("Notifications").InSchema("PMS").To("Subject");
        Rename.Column("Message").OnTable("Notifications").InSchema("PMS").To("Body");

        // Widen Subject to 500
        Alter.Column("Subject").OnTable("Notifications").InSchema("PMS")
            .AsString(500).Nullable();

        // Widen Body to 2000
        Alter.Column("Body").OnTable("Notifications").InSchema("PMS")
            .AsString(2000).Nullable();

        // Drop IsRead (not in entity)
        Delete.Column("IsRead").FromTable("Notifications").InSchema("PMS");

        // Drop old string RecipientId; will re-add as Guid
        Delete.Column("RecipientId").FromTable("Notifications").InSchema("PMS");

        // Add all missing columns
        Create.Column("Channel").OnTable("Notifications").InSchema("PMS")
            .AsString(50).Nullable();
        Create.Column("RecipientType").OnTable("Notifications").InSchema("PMS")
            .AsString(100).Nullable();
        Create.Column("RecipientId").OnTable("Notifications").InSchema("PMS")
            .AsGuid().Nullable();
        Create.Column("RecipientContact").OnTable("Notifications").InSchema("PMS")
            .AsString(200).Nullable();
        Create.Column("ReferenceId").OnTable("Notifications").InSchema("PMS")
            .AsGuid().Nullable();
        Create.Column("ReferenceType").OnTable("Notifications").InSchema("PMS")
            .AsString(100).Nullable();
        Create.Column("ScheduledAt").OnTable("Notifications").InSchema("PMS")
            .AsDateTimeOffset().Nullable();
        Create.Column("Status").OnTable("Notifications").InSchema("PMS")
            .AsString(50).Nullable();
        Create.Column("FailureReason").OnTable("Notifications").InSchema("PMS")
            .AsString(500).Nullable();
        Create.Column("RetryCount").OnTable("Notifications").InSchema("PMS")
            .AsInt32().NotNullable().WithDefaultValue(0);

        // Recreate index on Guid RecipientId
        Create.Index("IX_Notifications_RecipientId")
            .OnTable("Notifications").InSchema("PMS")
            .OnColumn("RecipientId").Ascending();

        Create.Index("IX_Notifications_Status")
            .OnTable("Notifications").InSchema("PMS")
            .OnColumn("Status").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Notifications_Status").OnTable("Notifications").InSchema("PMS");
        Delete.Index("IX_Notifications_RecipientId").OnTable("Notifications").InSchema("PMS");

        Delete.Column("RetryCount").FromTable("Notifications").InSchema("PMS");
        Delete.Column("FailureReason").FromTable("Notifications").InSchema("PMS");
        Delete.Column("Status").FromTable("Notifications").InSchema("PMS");
        Delete.Column("ScheduledAt").FromTable("Notifications").InSchema("PMS");
        Delete.Column("ReferenceType").FromTable("Notifications").InSchema("PMS");
        Delete.Column("ReferenceId").FromTable("Notifications").InSchema("PMS");
        Delete.Column("RecipientContact").FromTable("Notifications").InSchema("PMS");
        Delete.Column("RecipientId").FromTable("Notifications").InSchema("PMS");
        Delete.Column("RecipientType").FromTable("Notifications").InSchema("PMS");
        Delete.Column("Channel").FromTable("Notifications").InSchema("PMS");

        Create.Column("RecipientId").OnTable("Notifications").InSchema("PMS")
            .AsString(100).Nullable();
        Create.Column("IsRead").OnTable("Notifications").InSchema("PMS")
            .AsBoolean().NotNullable().WithDefaultValue(false);

        Rename.Column("Body").OnTable("Notifications").InSchema("PMS").To("Message");
        Rename.Column("Subject").OnTable("Notifications").InSchema("PMS").To("Title");

        Create.Index("IX_Notifications_RecipientId")
            .OnTable("Notifications").InSchema("PMS")
            .OnColumn("RecipientId").Ascending();
    }
}
