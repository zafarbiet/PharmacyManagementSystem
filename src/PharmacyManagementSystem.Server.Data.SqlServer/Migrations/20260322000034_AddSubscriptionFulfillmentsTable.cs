using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000034)]
public class AddSubscriptionFulfillmentsTable : Migration
{
    public override void Up()
    {
        Create.Table("SubscriptionFulfillments")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("CustomerSubscriptionId").AsGuid().NotNullable()
            .WithColumn("FulfillmentDate").AsDateTimeOffset().NotNullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_SubscriptionFulfillments_CustomerSubscriptionId")
            .OnTable("SubscriptionFulfillments").InSchema("PMS")
            .OnColumn("CustomerSubscriptionId").Ascending();

        Create.Index("IX_SubscriptionFulfillments_Status")
            .OnTable("SubscriptionFulfillments").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.ForeignKey("FK_SubscriptionFulfillments_CustomerSubscriptions_CustomerSubscriptionId")
            .FromTable("SubscriptionFulfillments").InSchema("PMS").ForeignColumn("CustomerSubscriptionId")
            .ToTable("CustomerSubscriptions").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_SubscriptionFulfillments_CustomerSubscriptions_CustomerSubscriptionId").OnTable("SubscriptionFulfillments").InSchema("PMS");
        Delete.Index("IX_SubscriptionFulfillments_Status").OnTable("SubscriptionFulfillments").InSchema("PMS");
        Delete.Index("IX_SubscriptionFulfillments_CustomerSubscriptionId").OnTable("SubscriptionFulfillments").InSchema("PMS");
        Delete.Table("SubscriptionFulfillments").InSchema("PMS");
    }
}
