using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NihFix.Postgres.Mcp;

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();
var configuration = configurationBuilder.Build();
var serverType = configuration.GetSection(McpServerOptions.SectionName).GetValue<ServerType>(nameof(McpServerOptions.ServerType));
Console.WriteLine(serverType);
var app = serverType switch
{
    ServerType.Sse => PostgresMcpFactory.CreateSseServer(args),
    ServerType.Stdio => PostgresMcpFactory.CreateStdioServer(args),
    _ => throw new ArgumentOutOfRangeException()
};

app.Run();