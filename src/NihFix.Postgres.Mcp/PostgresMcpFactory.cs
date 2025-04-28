using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NihFix.Postgres.Mcp;

public class PostgresMcpFactory
{
    public static IHost CreateSseServer(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .AddMcpServer()
            .WithHttpTransport()
            .WithToolsFromAssembly();
        builder.Services.Configure<McpServerOptions>(builder.Configuration.GetSection(McpServerOptions.SectionName));

        var app = builder.Build();
        app.MapMcp();
        return app;
    }
    
    public static IHost CreateStdioServer(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();
        builder.Services.Configure<McpServerOptions>(builder.Configuration.GetSection(McpServerOptions.SectionName));

        var app = builder.Build();
        return app;
    }
}