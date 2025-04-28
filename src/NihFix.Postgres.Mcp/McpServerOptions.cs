namespace NihFix.Postgres.Mcp;

public class McpServerOptions
{
    public const string SectionName = "McpServerOptions";

    public ServerType ServerType { get; set; } = ServerType.Stdio;
    
    public string ConnectionString { get; set; }
}