using Dapper;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

public class DatabaseSqlWithParameters
{
    public string SqlStatement { get; set; } = string.Empty;
    public DynamicParameters Parameters { get; set; } = new();
}
