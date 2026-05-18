
![Finnhub-MCP-Server](https://github.com/SalZaki/finnhub-mcp/blob/main/assets/github-banner.png)

<h4>

![Build Status](https://github.com/salzaki/finnhub-mcp/actions/workflows/dotnet.yml/badge.svg)
![codecov](https://codecov.io/gh/salzaki/finnhub-mcp/branch/main/graph/badge.svg)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green)
![MCP](https://img.shields.io/badge/AI-MCP-purple)
![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)
[![GitHub issues with bug label](https://img.shields.io/github/issues-raw/salzaki/finnhub-mcp/bug?label=bugs&logo=github&color=red)](https://github.com/salzaki/finnhub-mcp/issues?q=is%3Aissue+is%3Aopen+label%3Abug)
![GitHub Repo stars](https://img.shields.io/github/stars/salzaki/finnhub-mcp?style=social)

</h4>

# 👋 Welcome to Finnhub MCP Server

## 🎯 Project Overview

A **Model Context Protocol (MCP) Server** built on the official [ModelContextProtocol C# SDK](https://www.nuget.org/packages/ModelContextProtocol) that exposes **[Finnhub](https://finnhub.io/)**'s financial-data APIs to MCP-compatible clients (Claude Desktop, IDE assistants, and other agents). The server can be hosted over HTTP or STDIO and follows Clean Architecture across three projects: `Server` (transport + MCP surface), `Server.Application` (domain), and `Server.Infrastructure` (Finnhub HTTP client + serialization).

## 🚀 Key Features

- ✅ **MCP Tools and Resources** wired to the official `ModelContextProtocol` and `ModelContextProtocol.AspNetCore` packages
- ✅ **Symbol Search Tool** with input validation and sanitization (regex-based length and character constraints)
- ✅ **Exchanges Resource** exposed at `finnhub://resources/exchanges` for venue metadata (currently a stub pending the live Finnhub `/stock/exchange` wiring)
- ✅ **Resilient HTTP Communication** — typed `HttpClient` with retry, timeout, and circuit-breaker policies via `Microsoft.Extensions.Http.Resilience` and Polly
- ✅ **Source-generated JSON** through `System.Text.Json` `JsonSerializerContext` for low-allocation, AOT-friendly (de)serialization
- ✅ **Strongly-typed configuration** — `FinnHubOptions` bound from `appsettings.json` with data-annotation validation on startup
- ✅ **API key kept out of source** — read from the `FINNHUB_API_KEY` environment variable (or a local `.env` in development via `DotNetEnv`)
- ✅ **Dual transport** — HTTP (`MapMcp`) for hosted scenarios and STDIO for desktop MCP clients

## 🚧 Available & Upcoming MCP Capabilities

### ✅ Currently Available
- **`search-symbol`** tool — search for financial symbols by ticker, company name, ISIN, or CUSIP, optionally filtered by exchange code (limit 1–100, default 10)
- **`finnhub://resources/exchanges`** resource — catalog of stock exchanges (code, name, country, MIC, timezone, trading hours)

### 🔄 In Development
- Wire `ExchangesResource` to the live Finnhub `/stock/exchange` endpoint
- **Real-Time Quote Tool** — live prices for stocks, FX, and crypto
- **Company Profile Tool** — company information and metrics
- **Basic Financials Tool** — key financial metrics and ratios

### 📋 Planned
- Earnings calendar, news & sentiment, insider trading, market status
- Technical indicators (RSI, MACD, moving averages)
- Economic data and crypto market data
- WebSocket transport for streaming Finnhub feeds

## 🚀 Getting Started

### 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- A valid [Finnhub API Key](https://finnhub.io/dashboard) (free tier available)

### 🔑 Getting Your Finnhub API Key

1. Visit [Finnhub.io](https://finnhub.io/) and create a free account
2. Open your [Dashboard](https://finnhub.io/dashboard) and copy the API key
3. Free tier provides 60 requests/minute and basic market data

### 📦 Installation

```bash
git clone https://github.com/SalZaki/finnhub-mcp.git
cd finnhub-mcp
dotnet restore
```

### 🔐 API Key Configuration

> **Security note:** never commit your API key. Use environment variables or a local `.env` file that is git-ignored.

#### Option 1: Environment Variable (Recommended)

**macOS/Linux**

```bash
export FINNHUB_API_KEY="your_api_key_here"
```

**Windows PowerShell**

```powershell
$env:FINNHUB_API_KEY="your_api_key_here"
```

**Windows Command Prompt**

```cmd
set FINNHUB_API_KEY=your_api_key_here
```

#### Option 2: `.env` File (Development Only)

Create a `.env` file at the repository root — `DotNetEnv` loads it automatically when the host environment is `Development`:

```bash
echo "FINNHUB_API_KEY=your_api_key_here" > .env
```

### 🚀 Running the Server

#### HTTP Transport (default port `8080`)

```bash
dotnet run --project src/FinnHub.MCP.Server
```

The server binds to `http://localhost:8080/` unless you override it:

```bash
dotnet run --project src/FinnHub.MCP.Server --urls http://localhost:5101
```

#### STDIO Transport

```bash
dotnet run --project src/FinnHub.MCP.Server -- --stdio
```

### 🌐 HTTP Endpoints

When running in HTTP mode the following endpoints are exposed:

| Endpoint | Purpose |
| --- | --- |
| `GET /` | Application banner — name, version, environment, status |
| `MapMcp()` routes | Official MCP HTTP transport endpoints |
| `GET /mcp/sse` | Server-Sent Events keep-alive stream |
| `POST /mcp/streamable` | StreamableHTTP transport stub |
| `GET /mcp/health` | Lightweight health probe |
| `/swagger` | OpenAPI UI (Development only) |

## 📖 Usage

The recommended way to interact with the server is through an MCP-compatible client (e.g. Claude Desktop, an MCP-aware IDE plugin, or `mcp-inspector`) configured to either spawn the binary over STDIO or connect to the HTTP transport.

Example STDIO entry for Claude Desktop's `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "finnhub": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/absolute/path/to/finnhub-mcp/src/FinnHub.MCP.Server",
        "--",
        "--stdio"
      ],
      "env": {
        "FINNHUB_API_KEY": "your_api_key_here"
      }
    }
  }
}
```

### Tool: `search-symbol`

Parameters:

- `query` *(string, required)* — ticker, company name, ISIN, or CUSIP. 1–500 chars, letters/digits/space/`-`/`_`/`.` only.
- `exchange` *(string, optional)* — uppercase exchange code matching `[A-Z0-9\-_]{1,50}`, e.g. `US`, `L`.
- `limit` *(int, optional)* — 1–100, defaults to 10.
- `view` *(string, optional)* — response detail level. One of `summary` (default, ~500-token ceiling), `standard` (~2000-token ceiling), `full` (no ceiling).
- `fields` *(string[], optional)* — sparse projection over the documented response fields. Unknown field names are rejected as a validation error.

### Tool response envelope

Every MCP tool returns the same envelope shape so consuming models get a predictable contract and the server can enforce per-view token budgets without forcing every tool to reimplement the same accounting.

| Field | Type | Purpose |
|---|---|---|
| `is_success` | bool | Operation succeeded. |
| `data` | T \| null | Domain payload when successful. |
| `error_message` | string \| null | Human-readable failure reason. |
| `error_type` | string \| null | Categorised error (e.g. `NotFound`, `BudgetExceeded`). |
| `view` | string | Echoes the requested view. |
| `next_actions` | array | Server-suggested follow-up tool calls. |
| `explanation` | string \| null | Short natural-language summary. |
| `approx_tokens` | int | Estimated serialized token count, set by the middleware. |
| `rate_limit` | object \| null | Upstream rate-limit snapshot (populated in a later phase). |
| `sentiment_source` | string \| null | Source label for sentiment values. |
| `premium` | bool | Whether the underlying upstream endpoint required a premium key. |

A response that exceeds its declared view's token ceiling is rebuilt by the tool invocation middleware as a `BudgetExceeded` failure envelope; retry with a broader `view` or a sparser `fields` projection.

### Resource: `finnhub://resources/exchanges`

Returns the list of stock exchanges available through the provider as `application/json`.

## ⚙️ Configuration

Configuration is loaded from `appsettings.json`, an optional environment-specific `appsettings.{Environment}.json`, environment variables, and command-line arguments — in that order. The `FINNHUB_API_KEY` environment variable, if present, overrides `FinnHub:ApiKey`.

`FinnHubOptions` (bound from the `FinnHub` section) drives the API key, base URL, and per-endpoint settings. CORS allows any origin in `Development`; in other environments it reads from the `AllowedOrigins` array.

## 🧪 Testing

The solution ships with three xUnit unit-test projects, mocked using **NSubstitute** and measured with **coverlet**:

```bash
# Run the full test suite
dotnet test

# Run a single project
dotnet test tests/FinnHub.MCP.Server.Application.Tests.Unit
dotnet test tests/FinnHub.MCP.Server.Infrastructure.Tests.Unit
dotnet test tests/FinnHub.MCP.Server.Tests.Unit

# With coverage (uses coverlet.runsettings at the repo root)
dotnet test --settings coverlet.runsettings
```

## 🔧 Development

### Build

```bash
dotnet build
```

### Hot reload

```bash
dotnet watch --project src/FinnHub.MCP.Server
```

### Format & analyzers

```bash
dotnet format
```

`TreatWarningsAsErrors` is enabled in `Directory.Build.props`, so any analyzer or compiler warning will fail the build.

## 🏗️ Project Structure

```
src/
├── FinnHub.MCP.Server/                    # ASP.NET Core host, MCP transport wiring, Tools, Resources
├── FinnHub.MCP.Server.Application/        # Domain models, queries, services, exceptions
└── FinnHub.MCP.Server.Infrastructure/     # Finnhub HTTP client, DTOs, JSON context, DI registration
tests/
├── FinnHub.MCP.Server.Application.Tests.Unit/
├── FinnHub.MCP.Server.Infrastructure.Tests.Unit/
└── FinnHub.MCP.Server.Tests.Unit/
```

## 🛠️ Tech Stack

- **Framework:** [.NET 10](https://dotnet.microsoft.com/) with ASP.NET Core
- **MCP SDK:** [`ModelContextProtocol`](https://www.nuget.org/packages/ModelContextProtocol) and [`ModelContextProtocol.AspNetCore`](https://www.nuget.org/packages/ModelContextProtocol.AspNetCore)
- **Resilience:** [Polly](https://github.com/App-vNext/Polly) via `Microsoft.Extensions.Http.Resilience` and `Microsoft.Extensions.Http.Polly`
- **Serialization:** `System.Text.Json` with source-generated `JsonSerializerContext`
- **Testing:** [xUnit](https://xunit.net/), [NSubstitute](https://nsubstitute.github.io/), [coverlet](https://github.com/coverlet-coverage/coverlet)
- **Configuration:** [DotNetEnv](https://github.com/tonerdo/dotnet-env)
- **OpenAPI:** `Microsoft.AspNetCore.OpenApi` + `Swashbuckle.AspNetCore`
- **CI/CD:** GitHub Actions with [release-please](https://github.com/googleapis/release-please) for automated versioning

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feat/short-description`)
3. Make your changes and add tests
4. Ensure `dotnet build` and `dotnet test` both pass
5. Use [Conventional Commits](https://www.conventionalcommits.org/) — release-please depends on them
6. Submit a pull request

## 📄 License

Licensed under the MIT License — see [LICENSE](LICENSE) for details.

## 🙏 Acknowledgments

- [Finnhub.io](https://finnhub.io/) for the financial-data APIs
- The [Model Context Protocol](https://github.com/modelcontextprotocol) team and the official C# SDK
- The .NET community for the surrounding ecosystem

## 📞 Support

- **Issues:** [GitHub Issues](https://github.com/SalZaki/finnhub-mcp/issues)
- **Discussions:** [GitHub Discussions](https://github.com/SalZaki/finnhub-mcp/discussions)
- **Finnhub API:** [Finnhub Documentation](https://finnhub.io/docs/api)
