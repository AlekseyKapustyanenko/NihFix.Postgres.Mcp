using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using Npgsql;

namespace NihFix.Postgres.Mcp;

[McpServerToolType]
public static class PostgresTool
{
    [McpServerTool, Description("Executes SQL scripts for postgres and returns results.")]
    public static async Task<string> ExecuteSql(
        IOptionsSnapshot<McpServerOptions> serverOptions,
        [Description("Sql script.")]string sql,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connString = serverOptions.Value.ConnectionString;

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync(cancellationToken);

            var result = new List<Dictionary<string, object>>();
            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {

                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new Dictionary<string, object>();
                    foreach (var column in await reader.GetColumnSchemaAsync(cancellationToken))
                    {
                        if (column.ColumnOrdinal.HasValue)
                        {
                            row.Add(column.ColumnName, reader.GetValue(column.ColumnOrdinal.Value));
                        }
                    }

                    result.Add(row);
                }

            }

            return JsonSerializer.Serialize(result, JsonSerializerOptions.Web);
        }
        catch (Exception e)
        {
            return $"Failed to execute SQL script. Error: {e.Message}. Please check and revise errors and try again.";
        }
    
    }
}