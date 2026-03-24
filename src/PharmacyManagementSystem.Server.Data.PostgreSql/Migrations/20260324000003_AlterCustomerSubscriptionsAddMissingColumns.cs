using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260324000003)]
public class AlterCustomerSubscriptionsAddMissingColumns : Migration
{
    public override void Up()
    {
        Create.Column("CycleDayOfMonth").OnTable("CustomerSubscriptions").InSchema("PMS")
            .AsInt32().Nullable();
        Create.Column("ApprovedBy").OnTable("CustomerSubscriptions").InSchema("PMS")
            .AsString(200).Nullable();
        Create.Column("ApprovedAt").OnTable("CustomerSubscriptions").InSchema("PMS")
            .AsDateTimeOffset().Nullable();
    }

    public override void Down()
    {
        Delete.Column("ApprovedAt").FromTable("CustomerSubscriptions").InSchema("PMS");
        Delete.Column("ApprovedBy").FromTable("CustomerSubscriptions").InSchema("PMS");
        Delete.Column("CycleDayOfMonth").FromTable("CustomerSubscriptions").InSchema("PMS");
    }
}
