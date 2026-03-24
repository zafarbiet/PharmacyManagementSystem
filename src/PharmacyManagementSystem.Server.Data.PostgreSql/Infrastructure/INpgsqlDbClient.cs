namespace PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

public interface INpgsqlDbClient
{
    Task<IEnumerable<T>> QueryAsync<T>(DatabaseSqlWithParameters sql, CancellationToken cancellationToken);
    Task ExecuteAsync(DatabaseSqlWithParameters sql, CancellationToken cancellationToken);
}
