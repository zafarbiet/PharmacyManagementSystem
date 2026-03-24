using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000033)]
public class AddCustomerSubscriptionItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("CustomerSubscriptionItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("CustomerSubscriptionId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("QuantityPerCycle").AsInt32().NotNullable()
            .WithColumn("FrequencyDays").AsInt32().Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_CustomerSubscriptionItems_CustomerSubscriptionId")
            .OnTable("CustomerSubscriptionItems").InSchema("PMS")
            .OnColumn("CustomerSubscriptionId").Ascending();

        Create.Index("IX_CustomerSubscriptionItems_DrugId")
            .OnTable("CustomerSubscriptionItems").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.ForeignKey("FK_CustomerSubscriptionItems_CustomerSubscriptions_CustomerSubscriptionId")
            .FromTable("CustomerSubscriptionItems").InSchema("PMS").ForeignColumn("CustomerSubscriptionId")
            .ToTable("CustomerSubscriptions").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_CustomerSubscriptionItems_Drugs_DrugId")
            .FromTable("CustomerSubscriptionItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_CustomerSubscriptionItems_Drugs_DrugId").OnTable("CustomerSubscriptionItems").InSchema("PMS");
        Delete.ForeignKey("FK_CustomerSubscriptionItems_CustomerSubscriptions_CustomerSubscriptionId").OnTable("CustomerSubscriptionItems").InSchema("PMS");
        Delete.Index("IX_CustomerSubscriptionItems_DrugId").OnTable("CustomerSubscriptionItems").InSchema("PMS");
        Delete.Index("IX_CustomerSubscriptionItems_CustomerSubscriptionId").OnTable("CustomerSubscriptionItems").InSchema("PMS");
        Delete.Table("CustomerSubscriptionItems").InSchema("PMS");
    }
}
