using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000002)]
public class AddDrugCategoriesTable : Migration
{
    public override void Up()
    {
        Create.Table("DrugCategories")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).Nullable()
            .WithColumn("Description").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DrugCategories_Name")
            .OnTable("DrugCategories").InSchema("PMS")
            .OnColumn("Name").Ascending();
    }

    public override void Down()
    {
        Delete.Index("IX_DrugCategories_Name").OnTable("DrugCategories").InSchema("PMS");
        Delete.Table("DrugCategories").InSchema("PMS");
    }
}
