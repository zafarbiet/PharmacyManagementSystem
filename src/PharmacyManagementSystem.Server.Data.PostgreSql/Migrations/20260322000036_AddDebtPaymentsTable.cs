using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000036)]
public class AddDebtPaymentsTable : Migration
{
    public override void Up()
    {
        Create.Table("DebtPayments")
            .InSchema("PMS")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DebtRecordId").AsGuid().NotNullable()
            .WithColumn("PaymentDate").AsDateTimeOffset().NotNullable()
            .WithColumn("AmountPaid").AsDecimal(18, 2).NotNullable()
            .WithColumn("PaymentMethod").AsString(50).Nullable()
            .WithColumn("Notes").AsString(500).Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedBy").AsString(100).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

        Create.Index("IX_DebtPayments_DebtRecordId")
            .OnTable("DebtPayments").InSchema("PMS")
            .OnColumn("DebtRecordId").Ascending();

        Create.ForeignKey("FK_DebtPayments_DebtRecords_DebtRecordId")
            .FromTable("DebtPayments").InSchema("PMS").ForeignColumn("DebtRecordId")
            .ToTable("DebtRecords").InSchema("PMS").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DebtPayments_DebtRecords_DebtRecordId").OnTable("DebtPayments").InSchema("PMS");
        Delete.Index("IX_DebtPayments_DebtRecordId").OnTable("DebtPayments").InSchema("PMS");
        Delete.Table("DebtPayments").InSchema("PMS");
    }
}
