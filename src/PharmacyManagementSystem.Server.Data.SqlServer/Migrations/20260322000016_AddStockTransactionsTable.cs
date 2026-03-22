using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000016)]
public class AddStockTransactionsTable : Migration
{
    public override void Up()
    {
        Create.Table("StockTransactions")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DrugId").AsGuid().NotNullable()
            .WithColumn("BatchNumber").AsString(100).Nullable()
            .WithColumn("TransactionType").AsString(50).NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("TransactionDate").AsDateTimeOffset().NotNullable()
            .WithColumn("ReferenceId").AsGuid().Nullable()
            .WithColumn("ReferenceType").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_StockTransactions_DrugId")
            .OnTable("StockTransactions").InSchema("PMS")
            .OnColumn("DrugId").Ascending();

        Create.Index("IX_StockTransactions_TransactionDate")
            .OnTable("StockTransactions").InSchema("PMS")
            .OnColumn("TransactionDate").Ascending();

        Create.Index("IX_StockTransactions_TransactionType")
            .OnTable("StockTransactions").InSchema("PMS")
            .OnColumn("TransactionType").Ascending();

        Create.ForeignKey("FK_StockTransactions_Drugs_DrugId")
            .FromTable("StockTransactions").InSchema("PMS").ForeignColumn("DrugId")
            .ToTable("Drugs").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_StockTransactions_Drugs_DrugId").OnTable("StockTransactions").InSchema("PMS");
        Delete.Index("IX_StockTransactions_TransactionType").OnTable("StockTransactions").InSchema("PMS");
        Delete.Index("IX_StockTransactions_TransactionDate").OnTable("StockTransactions").InSchema("PMS");
        Delete.Index("IX_StockTransactions_DrugId").OnTable("StockTransactions").InSchema("PMS");
        Delete.Table("StockTransactions").InSchema("PMS");
    }
}
