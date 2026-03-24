using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000022)]
public class AddDrugInventoryRackAssignmentTable : Migration
{
    public override void Up()
    {
        Create.Table("DrugInventoryRackAssignment")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugInventoryId").AsGuid().NotNullable()
            .WithColumn("RackId").AsGuid().NotNullable()
            .WithColumn("QuantityPlaced").AsInt32().NotNullable()
            .WithColumn("PlacedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DrugInventoryRackAssignment_DrugInventoryId")
            .OnTable("DrugInventoryRackAssignment").InSchema("PMS")
            .OnColumn("DrugInventoryId").Ascending();

        Create.Index("IX_DrugInventoryRackAssignment_RackId")
            .OnTable("DrugInventoryRackAssignment").InSchema("PMS")
            .OnColumn("RackId").Ascending();

        Create.ForeignKey("FK_DrugInventoryRackAssignment_DrugInventory_DrugInventoryId")
            .FromTable("DrugInventoryRackAssignment").InSchema("PMS").ForeignColumn("DrugInventoryId")
            .ToTable("DrugInventory").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_DrugInventoryRackAssignment_Racks_RackId")
            .FromTable("DrugInventoryRackAssignment").InSchema("PMS").ForeignColumn("RackId")
            .ToTable("Racks").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DrugInventoryRackAssignment_Racks_RackId").OnTable("DrugInventoryRackAssignment").InSchema("PMS");
        Delete.ForeignKey("FK_DrugInventoryRackAssignment_DrugInventory_DrugInventoryId").OnTable("DrugInventoryRackAssignment").InSchema("PMS");
        Delete.Index("IX_DrugInventoryRackAssignment_RackId").OnTable("DrugInventoryRackAssignment").InSchema("PMS");
        Delete.Index("IX_DrugInventoryRackAssignment_DrugInventoryId").OnTable("DrugInventoryRackAssignment").InSchema("PMS");
        Delete.Table("DrugInventoryRackAssignment").InSchema("PMS");
    }
}
