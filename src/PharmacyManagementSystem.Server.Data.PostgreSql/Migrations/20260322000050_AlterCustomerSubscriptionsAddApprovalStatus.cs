using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000050)]
public class AlterCustomerSubscriptionsAddApprovalStatus : Migration
{
    public override void Up()
    {
        Alter.Table("CustomerSubscriptions").InSchema("PMS")
            .AddColumn("ApprovalStatus").AsString(20).Nullable();
    }

    public override void Down()
    {
        Delete.Column("ApprovalStatus").FromTable("CustomerSubscriptions").InSchema("PMS");
    }
}
