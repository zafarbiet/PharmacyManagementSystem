using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000008)]
public class AddDrugUsageTable : Migration
{
    public override void Up()
    {
        Create.Table("DrugUsage")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("DosageInstructions").AsString(1000).Nullable()
            .WithColumn("Indications").AsString(1000).Nullable()
            .WithColumn("Contraindications").AsString(1000).Nullable()
            .WithColumn("SideEffects").AsString(1000).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_DrugUsage_Drugs_DrugId")
            .FromTable("DrugUsage").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DrugUsage_Drugs_DrugId").OnTable("DrugUsage").InSchema("PMS");
        Delete.Table("DrugUsage").InSchema("PMS");
    }
}
