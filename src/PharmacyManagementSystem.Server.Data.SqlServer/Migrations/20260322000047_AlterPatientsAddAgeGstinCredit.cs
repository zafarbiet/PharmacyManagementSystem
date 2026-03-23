using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

[Migration(20260322000047)]
public class AlterPatientsAddAgeGstinCredit : Migration
{
    public override void Up()
    {
        Alter.Table("Patients").InSchema("PMS")
            .AddColumn("Age").AsInt32().Nullable()
            .AddColumn("Gstin").AsString(20).Nullable()
            .AddColumn("CreditBalance").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .AddColumn("IsSubscriber").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Column("Age").FromTable("Patients").InSchema("PMS");
        Delete.Column("Gstin").FromTable("Patients").InSchema("PMS");
        Delete.Column("CreditBalance").FromTable("Patients").InSchema("PMS");
        Delete.Column("IsSubscriber").FromTable("Patients").InSchema("PMS");
    }
}
