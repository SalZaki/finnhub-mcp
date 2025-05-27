# MCP FinnHub Server


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
