// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Reflection;
using DotNetEnv;
using MCP.FinnHub.Server.SSE.Application.Features.HealthCheck;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Services;
using MCP.FinnHub.Server.SSE.Common;
using MCP.FinnHub.Server.SSE.Options;
using MCP.FinnHub.Server.SSE.Tools.Search;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

builder.Services.AddSingleton<ISearchService, SearchService>();
builder.Services.AddSingleton<SearchSymbolsTool>();

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
            ToolCollection = [serviceProvider.GetRequiredService<SearchSymbolsTool>()]
        }
    };
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
    .AddCheck<FinnHubHealthCheck>("FinnHub", tags: ["ready"]);

builder.Services.AddHttpClient("FinnHub", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["FinnHub:BaseUrl"] ?? "https://finnhub.io/api/v1");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.Timeout = TimeSpan.FromSeconds(30);
    })
    .AddStandardResilienceHandler();

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

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration
            }),
            totalDuration = report.TotalDuration,
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapMcp();
app.Run();
