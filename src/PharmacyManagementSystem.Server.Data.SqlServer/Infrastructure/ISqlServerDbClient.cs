namespace PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

public interface ISqlServerDbClient
{
    Task<IEnumerable<T>> QueryAsync<T>(DatabaseSqlWithParameters sql, CancellationToken cancellationToken);
    Task ExecuteAsync(DatabaseSqlWithParameters sql, CancellationToken cancellationToken);
}
