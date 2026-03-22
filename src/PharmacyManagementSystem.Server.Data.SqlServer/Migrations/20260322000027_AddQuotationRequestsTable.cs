using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000027)]
public class AddQuotationRequestsTable : Migration
{
    public override void Up()
    {
        Create.Table("QuotationRequests")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("RequestDate").AsDateTimeOffset().NotNullable()
            .WithColumn("RequiredByDate").AsDateTimeOffset().Nullable()
            .WithColumn("Status").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("RequestedBy").AsString(100).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_QuotationRequests_Status")
            .OnTable("QuotationRequests").InSchema("PMS")
            .OnColumn("Status").Ascending();

        Create.Index("IX_QuotationRequests_RequestDate")
            .OnTable("QuotationRequests").InSchema("PMS")
            .OnColumn("RequestDate").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_QuotationRequests_RequestDate").OnTable("QuotationRequests").InSchema("PMS");
        Delete.Index("IX_QuotationRequests_Status").OnTable("QuotationRequests").InSchema("PMS");
        Delete.Table("QuotationRequests").InSchema("PMS");
    }
}
