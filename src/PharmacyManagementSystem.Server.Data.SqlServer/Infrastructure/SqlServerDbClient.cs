using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

public class SqlServerDbClient(ILogger<SqlServerDbClient> logger, IDbConnection connection) : ISqlServerDbClient
{
    private readonly ILogger<SqlServerDbClient> _logger = logger;
    private readonly IDbConnection _connection = connection;

    public async Task<IEnumerable<T>> QueryAsync<T>(DatabaseSqlWithParameters sql, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sql);

        _logger.LogDebug("Executing query: {SqlStatement}", sql.SqlStatement);

        var commandDefinition = new CommandDefinition(sql.SqlStatement, sql.Parameters, cancellationToken: cancellationToken);
        var result = await _connection.QueryAsync<T>(commandDefinition).ConfigureAwait(false);

        return result;
    }

    public async Task ExecuteAsync(DatabaseSqlWithParameters sql, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sql);

        _logger.LogDebug("Executing command: {SqlStatement}", sql.SqlStatement);

        var commandDefinition = new CommandDefinition(sql.SqlStatement, sql.Parameters, cancellationToken: cancellationToken);
        await _connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);
    }
}
