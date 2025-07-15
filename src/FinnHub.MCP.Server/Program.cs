// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using DotNetEnv;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Common;
using FinnHub.MCP.Server.Infrastructure.Extensions;
using FinnHub.MCP.Server.Tools.Search;

var assembly = Assembly.GetEntryAssembly();
var applicationName = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"finnhub.mcp.server.{Guid.NewGuid()}.sse";
var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "1.0.0";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = applicationName,
    Args = args
});

Env.TraversePath().Load();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services
    .AddOptions<FinnHubOptions>()
    .Bind(builder.Configuration.GetSection("FinnHub"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.RegisterInfrastructure();

builder.Services.AddSingleton<ISearchService, SearchService>();
builder.Services.AddSingleton<SearchSymbolTool>();

var mcpServerOptionsBuilder = builder.Services.AddOptions<McpServerOptions>();

mcpServerOptionsBuilder.Configure<IServiceProvider>((mcpServerOptions, serviceProvider) =>
{
    mcpServerOptions.ProtocolVersion = Constants.Server.ProtocolVersion;
    mcpServerOptions.ServerInstructions = Constants.Server.Instructions;
    mcpServerOptions.ServerInfo = new Implementation
    {
        Name = applicationName,
        Version = version
    };
    mcpServerOptions.Capabilities = new ServerCapabilities
    {
        Tools = new ToolsCapability
        {
            ToolCollection = [serviceProvider.GetRequiredService<SearchSymbolTool>()]
        }
    };
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
        else
        {
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        }

        policy.WithExposedHeaders("Content-Type", "Cache-Control", "Connection");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.Map("/", () => new
{
    application = applicationName,
    version,
    environment = app.Environment.EnvironmentName,
    status = "running"
});

app.MapMcp();
app.Run();
