using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000041)]
public class AddNotificationsTable : Migration
{
    public override void Up()
    {
        Create.Table("Notifications")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Title").AsString(200).NotNullable()
            .WithColumn("Message").AsString(1000).Nullable()
            .WithColumn("NotificationType").AsString(100).Nullable()
            .WithColumn("RecipientId").AsString(100).Nullable()
            .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("SentAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Notifications_RecipientId")
            .OnTable("Notifications").InSchema("PMS")
            .OnColumn("RecipientId").Ascending();

        Create.Index("IX_Notifications_NotificationType")
            .OnTable("Notifications").InSchema("PMS")
            .OnColumn("NotificationType").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_Notifications_NotificationType").OnTable("Notifications").InSchema("PMS");
        Delete.Index("IX_Notifications_RecipientId").OnTable("Notifications").InSchema("PMS");
        Delete.Table("Notifications").InSchema("PMS");
    }
}
