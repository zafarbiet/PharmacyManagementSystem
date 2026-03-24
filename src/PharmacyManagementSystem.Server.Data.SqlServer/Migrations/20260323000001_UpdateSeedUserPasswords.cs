using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Migrations;

/// <summary>
/// Sets real SHA-256 password hashes for seeded users.
/// Hash format: SHA256(username + ":" + password) as uppercase hex.
/// Credentials after this migration:
///   admin        / admin123
///   pharmacist1  / pharma123
/// </summary>
[Migration(20260323000001)]
public class UpdateSeedUserPasswords : Migration
{
    public override void Up()
    {
        // SHA256("admin:admin123") — computed by SQL HASHBYTES and stored as NVARCHAR hex
        Execute.Sql(@"
            UPDATE PMS.Users
            SET PasswordHash = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', 'admin:admin123'), 2),
                UpdatedAt    = SYSDATETIMEOFFSET(),
                UpdatedBy    = 'migration'
            WHERE Username = 'admin' AND IsActive = 1;
        ");

        // SHA256("pharmacist1:pharma123")
        Execute.Sql(@"
            UPDATE PMS.Users
            SET PasswordHash = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', 'pharmacist1:pharma123'), 2),
                UpdatedAt    = SYSDATETIMEOFFSET(),
                UpdatedBy    = 'migration'
            WHERE Username = 'pharmacist1' AND IsActive = 1;
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            UPDATE PMS.Users
            SET PasswordHash = 'AQAAAAIAAYagAAAAEHashedPasswordHere==',
                UpdatedAt    = SYSDATETIMEOFFSET(),
                UpdatedBy    = 'migration'
            WHERE Username IN ('admin', 'pharmacist1') AND IsActive = 1;
        ");
    }
}
