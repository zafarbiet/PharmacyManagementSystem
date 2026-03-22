using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000011)]
public class AddPrescriptionItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("PrescriptionItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PrescriptionId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("Dosage").AsString(200).Nullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("Instructions").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_PrescriptionItems_Prescriptions_PrescriptionId")
            .FromTable("PrescriptionItems").InSchema("PMS").ForeignColumn("PrescriptionId")
            .ToTable("Prescriptions").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_PrescriptionItems_Drugs_DrugId")
            .FromTable("PrescriptionItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_PrescriptionItems_Drugs_DrugId").OnTable("PrescriptionItems").InSchema("PMS");
        Delete.ForeignKey("FK_PrescriptionItems_Prescriptions_PrescriptionId").OnTable("PrescriptionItems").InSchema("PMS");
        Delete.Table("PrescriptionItems").InSchema("PMS");
    }
}
