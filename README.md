# MCP FinnHub Server

![Build Status](https://github.com/SalZaki/mcp-finnhub/actions/workflows/dotnet.yml/badge.svg)
[![codecov](https://codecov.io/gh/SalZaki/mcp-finnhub/branch/main/graph/badge.svg)](https://codecov.io/gh/SalZaki/mcp-finnhub)

A lightweight Model Context Protocol (MCP) Server that integrates with Finnhub’s financial data APIs using both Server-Sent Events (SSE).

### 🔧 CORS Configuration

The API enables [Cross-Origin Resource Sharing (CORS)](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) to support requests from external clients and tools (e.g., frontends, Swagger UI).

#### 🚀 Default Policy

Configured in `Program.cs`:

```csharp
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
```

Middleware:

```csharp
app.UseCors();
```

#### ✅ Purpose

* Supports browser-based tools and frontends
* Enables interaction with the MCP server across origins

#### 🔒 Note

In production, replace `AllowAnyOrigin()` with `WithOrigins("https://your-frontend.com")` to restrict access.
