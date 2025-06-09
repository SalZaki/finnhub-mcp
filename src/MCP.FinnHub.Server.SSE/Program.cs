using System.Diagnostics;
using System.Reflection;
using MCP.FinnHub.Server.SSE.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ModelContextProtocol.Protocol;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services
    .AddOptions<FinnHubOptions>()
    .Bind(builder.Configuration.GetSection("FinnHub"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Type", "Cache-Control", "Connection");
    });
});

var assembly = Assembly.GetExecutingAssembly();
builder.Services
    .AddMcpServer(o =>
    {
        var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        var version = fvi.FileVersion ?? "v0.0.1";

        o.ServerInfo = new Implementation
        {
            Name = "Custom MCP Server (SSE)",
            Version = version
        };
        o.ServerInstructions =
            "If no programming language is specified, assume C#. Keep your responses brief and professional.";
    })
    .WithHttpTransport()
    .WithResourcesFromAssembly(assembly)
    .WithPromptsFromAssembly(assembly)
    .WithToolsFromAssembly(assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

HealthCheckOptions CreateHealthOptions() => new()
{
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
            totalDuration = report.TotalDuration
        };
        await context.Response.WriteAsJsonAsync(response);
    }
};

app.MapHealthChecks("/health/ready", CreateHealthOptions());
app.MapHealthChecks("/health/live", CreateHealthOptions());
app.UseCors();
app.UseHttpsRedirection();
app.MapMcp();
app.Run();
