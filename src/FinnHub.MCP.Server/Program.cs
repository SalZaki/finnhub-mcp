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
using Microsoft.AspNetCore.Mvc;

var assembly = Assembly.GetEntryAssembly();
var applicationName = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"finnhub-mcp-server-{Guid.NewGuid()}";
var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "1.0.0";

if (!args.Contains("--urls"))
{
    args = args
        .Append("--urls")
        .Append($"http://localhost:{8080}/")
        .ToArray();
}

var isStdio = args.Contains("--stdio");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = applicationName,
    Args = args
});

if (builder.Environment.IsDevelopment())
{
    Env.TraversePath().Load();
}

var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
var exeDirectory = exePath is not null ? Path.GetDirectoryName(exePath) : Directory.GetCurrentDirectory();

builder.Configuration
    .SetBasePath(exeDirectory!)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var apiKey = Environment.GetEnvironmentVariable("FINNHUB_API_KEY");

if (!string.IsNullOrWhiteSpace(apiKey))
{
    builder.Configuration["FinnHub:ApiKey"] = apiKey;
}

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

if (isStdio)
{
    builder.Logging.ClearProviders();
    builder.Services.AddMcpServer().WithStdioServerTransport();
}
else
{
    builder.Services.AddMcpServer().WithHttpTransport();
}

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

        policy.WithExposedHeaders("Content-Type", "Cache-Control", "Connection", "Access-Control-Allow-Origin");
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

app.MapGet("/", () => new
{
    application = applicationName,
    version,
    environment = app.Environment.EnvironmentName,
    status = "running"
});

if (!isStdio)
{
    app.MapMcp();

    app.MapPost("/mcp", async (HttpContext context, [FromServices] IServiceProvider serviceProvider) =>
    {
        try
        {
            context.Response.Headers.TryAdd("Content-Type", "application/json");
            context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
            context.Response.Headers.TryAdd("Access-Control-Allow-Methods", "POST, OPTIONS");
            context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type");

            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Request body is empty" }));
                return;
            }

            Console.Error.WriteLine($"MCP HTTP Request: {requestBody}");

            var response = new
            {
                jsonrpc = "2.0",
                id = 1,
                result = new { status = "MCP HTTP endpoint working" }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (JsonException jsonEx)
        {
            Console.Error.WriteLine($"MCP HTTP JSON Error: {jsonEx.Message}");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid JSON format" }));
        }
        catch (InvalidOperationException invalidOpEx)
        {
            Console.Error.WriteLine($"MCP HTTP Invalid Operation: {invalidOpEx.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid operation" }));
        }
        catch (IOException ioEx)
        {
            Console.Error.WriteLine($"MCP HTTP IO Error: {ioEx.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "IO error occurred" }));
        }
    });

    app.MapMethods("/mcp", ["OPTIONS"], async context =>
    {
        context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
        context.Response.Headers.TryAdd("Access-Control-Allow-Methods", "POST, OPTIONS");
        context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type");
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    });

    app.MapGet("/mcp/sse", async (HttpContext context, [FromServices] IServiceProvider serviceProvider) =>
    {
        try
        {
            context.Response.Headers.TryAdd("Content-Type", "text/event-stream");
            context.Response.Headers.TryAdd("Cache-Control", "no-cache");
            context.Response.Headers.TryAdd("Connection", "keep-alive");
            context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
            context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type");

            // Send initial connection event
            await context.Response.WriteAsync("data: {\"type\":\"connection\",\"status\":\"connected\"}\n\n");
            await context.Response.Body.FlushAsync();

            // Keep the connection alive and handle MCP communication
            var cancellationToken = context.RequestAborted;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Send keepalive ping every 30 seconds
                    await Task.Delay(30000, cancellationToken);

                    var pingMessage = JsonSerializer.Serialize(new
                    {
                        jsonrpc = "2.0",
                        method = "ping",
                        @params = new { timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
                    });

                    await context.Response.WriteAsync($"data: {pingMessage}\n\n");
                    await context.Response.Body.FlushAsync();
                }
            }
            catch (OperationCanceledException)
            {
                // Client disconnected, this is normal
            }
        }
        catch (InvalidOperationException invalidOpEx)
        {
            Console.Error.WriteLine($"SSE Invalid Operation: {invalidOpEx.Message}");
            try
            {
                var errorMessage = JsonSerializer.Serialize(new { error = "Invalid operation" });
                await context.Response.WriteAsync($"data: {errorMessage}\n\n");
                await context.Response.Body.FlushAsync();
            }
            catch (InvalidOperationException)
            {
                // Response might already be closed
            }
        }
        catch (IOException ioEx)
        {
            Console.Error.WriteLine($"SSE IO Error: {ioEx.Message}");
            try
            {
                var errorMessage = JsonSerializer.Serialize(new { error = "Connection error" });
                await context.Response.WriteAsync($"data: {errorMessage}\n\n");
                await context.Response.Body.FlushAsync();
            }
            catch (InvalidOperationException)
            {
                // Response might already be closed
            }
        }
    });

    app.MapPost("/mcp/streamable", async (HttpContext context, [FromServices] IServiceProvider serviceProvider) =>
    {
        try
        {
            context.Response.Headers.TryAdd("Content-Type", "application/json");
            context.Response.Headers.TryAdd("Transfer-Encoding", "chunked");
            context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
            context.Response.Headers.TryAdd("Access-Control-Allow-Methods", "POST, OPTIONS");
            context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type");

            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            Console.Error.WriteLine($"MCP Streamable Request: {requestBody}");

            var response = new
            {
                jsonrpc = "2.0",
                id = 1,
                result = new
                {
                    status = "MCP StreamableHttp endpoint working",
                    transport = "streamable-http"
                }
            };

            var responseJson = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseJson);
            await context.Response.Body.FlushAsync();
        }
        catch (JsonException jsonEx)
        {
            Console.Error.WriteLine($"StreamableHttp JSON Error: {jsonEx.Message}");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid JSON format" }));
        }
        catch (InvalidOperationException invalidOpEx)
        {
            Console.Error.WriteLine($"StreamableHttp Invalid Operation: {invalidOpEx.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid operation" }));
        }
        catch (IOException ioEx)
        {
            Console.Error.WriteLine($"StreamableHttp IO Error: {ioEx.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "IO error occurred" }));
        }
    });

    app.MapMethods("/mcp/streamable", ["OPTIONS"], async context =>
    {
        context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
        context.Response.Headers.TryAdd("Access-Control-Allow-Methods", "POST, OPTIONS");
        context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type");
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    });

    app.MapGet("/mcp/health", () => new
    {
        status = "healthy",
        transport = "http",
        endpoints = new[]
        {
            "/mcp",
            "/mcp/sse",
            "/mcp/streamable"
        }
    });
}

app.Run();
