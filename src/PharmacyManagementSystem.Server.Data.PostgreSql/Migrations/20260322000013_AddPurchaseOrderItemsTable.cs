using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000013)]
public class AddPurchaseOrderItemsTable : Migration
{
    public override void Up()
    {
        Create.Table("PurchaseOrderItems")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PurchaseOrderId").AsGuid().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("QuantityOrdered").AsInt32().NotNullable()
            .WithColumn("QuantityReceived").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("UnitPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("BatchNumber").AsString(100).Nullable()
            .WithColumn("ExpirationDate").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.ForeignKey("FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId")
            .FromTable("PurchaseOrderItems").InSchema("PMS").ForeignColumn("PurchaseOrderId")
            .ToTable("PurchaseOrders").InSchema("PMS").PrimaryColumn("Id");

        Create.ForeignKey("FK_PurchaseOrderItems_Drugs_DrugId")
            .FromTable("PurchaseOrderItems").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_PurchaseOrderItems_Drugs_DrugId").OnTable("PurchaseOrderItems").InSchema("PMS");
        Delete.ForeignKey("FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId").OnTable("PurchaseOrderItems").InSchema("PMS");
        Delete.Table("PurchaseOrderItems").InSchema("PMS");
    }
}
