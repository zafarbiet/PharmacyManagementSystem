using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000006)]
public class AddDrugInventoryTable : Migration
{
    public override void Up()
    {
        Create.Table("DrugInventory")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("BatchNumber").AsString(100).NotNullable()
            .WithColumn("ExpirationDate").AsDateTimeOffset().NotNullable()
            .WithColumn("QuantityInStock").AsInt32().NotNullable()
            .WithColumn("StorageConditions").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_DrugInventory_Drugs_DrugId")
            .FromTable("DrugInventory").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DrugInventory_Drugs_DrugId").OnTable("DrugInventory").InSchema("PMS");
        Delete.Table("DrugInventory").InSchema("PMS");
    }
}
