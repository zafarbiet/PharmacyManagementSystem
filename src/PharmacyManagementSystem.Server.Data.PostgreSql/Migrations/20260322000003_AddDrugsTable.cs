using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000003)]
public class AddDrugsTable : Migration
{
    public override void Up()
    {
        Create.Table("Drugs")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(200).Nullable()
            .WithColumn("GenericName").AsString(200).Nullable()
            .WithColumn("ManufacturerName").AsString(200).Nullable()
            .WithColumn("CategoryId").AsGuid().NotNullable()
            .WithColumn("UnitOfMeasure").AsString(50).Nullable()
            .WithColumn("ReorderLevel").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_Drugs_Name")
            .OnTable("Drugs").InSchema("PMS")
            .OnColumn("Name").Ascending();

        Create.ForeignKey("FK_Drugs_DrugCategories_CategoryId")
            .FromTable("Drugs").InSchema("PMS").ForeignColumn("CategoryId")
            .ToTable("DrugCategories").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Drugs_DrugCategories_CategoryId").OnTable("Drugs").InSchema("PMS");
        Delete.Index("IX_Drugs_Name").OnTable("Drugs").InSchema("PMS");
        Delete.Table("Drugs").InSchema("PMS");
    }
}
