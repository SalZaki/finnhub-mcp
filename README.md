
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

**Tools (8):**
- **`search-symbol`** — search for financial symbols by ticker, company name, ISIN, or CUSIP, optionally filtered by exchange code (limit 1–100, default 10). On a high-confidence exact match, suggests `get-quote`, `get-company-profile`, `get-news-pulse`, `get-financials-snapshot`, `get-price-summary`, and `get-peers` as next actions.
- **`get-quote`** — real-time price snapshot (current, change, percent change, session high/low/open, prev close, timestamp). Cached at the 10-second Quote tier.
- **`get-company-profile`** — company snapshot (name, ticker, country, currency, exchange, IPO, market cap, shares outstanding, industry). `view=summary` drops the cosmetic fields (logo, phone, weburl); `standard` and `full` include them.
- **`get-peers`** — peer ticker list for a symbol, optionally grouped by `industry` (default), `subindustry`, or `sector`. Summary view caps at 10 peers, standard at 25, full returns all.
- **`get-financials-snapshot`** — curated 10-KPI snapshot (market cap, P/E, P/B, EPS, dividend yield, 52-week high/low, 52-week return, beta, revenue per share). `view=full` adds the raw upstream metric dictionary.
- **`get-price-summary`** — aggregated price stats over a candle range (`min`, `max`, `mean`, `return_pct`, `vol`, `latest`). Period: `7d`, `30d` (default), `90d`, `1y`. `view=full` adds the raw OHLCV arrays.
- **`get-news-pulse`** — news pulse over the past 7 days: sentiment score (when available), top 5 headlines, article count, week-over-week delta. Gracefully degrades sentiment when the upstream `/news-sentiment` endpoint is premium-locked.
- **`get-calendar`** — parameter-dispatched calendar lookup. v1 ships `kind=earnings` with date window (max 90 days) and optional symbol filter; IPO and economic kinds land in follow-up releases. Summary view caps at 10 events, standard at 25, full returns the complete window.

Every tool returns the standard token-budgeted envelope with cross-linked `next_actions` and the most-recent observed Finnhub rate-limit headers.

**Resources (2):**
- **`finnhub://resources/exchanges`** — catalog of stock exchanges (code, name, country, MIC, timezone, trading hours)
- **`finnhub://resources/api-status`** — latest observed Finnhub upstream quota: `remaining`, `reset_at`, and a rolling 429 count

### 🔄 In Development
- Wire `ExchangesResource` to the live Finnhub `/stock/exchange` endpoint
- **`get-calendar`** — extend with `kind=ipo` and `kind=economic`

### 📋 Planned
- **`get-insider-signal`** — net buy/sell aggregation over the past 30 days plus notable insider names
- **`get-recommendations`** — analyst consensus with strong-buy/buy/hold/sell/strong-sell counts
- `search-tools` meta-tool for intent-based discovery (P7)
- Technical indicators (RSI, MACD, moving averages)
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

### Tool: `get-quote`

Parameters:

- `symbol` *(string, required)* — uppercase ticker, e.g. `AAPL`. 1–20 chars, starts with A–Z.
- `view` *(string, optional)* — `summary`/`standard`/`full` all return the same curated snapshot fields.

Response is intentionally compact (`current`, `change`, `percent_change`, `high`, `low`, `open`, `prev_close`, `timestamp_utc`). Cached at the 10-second Quote tier.

### Tool: `get-company-profile`

Parameters:

- `symbol` *(string, required)* — uppercase ticker.
- `view` *(string, optional)* — `summary` drops `logo`/`phone`/`weburl`; `standard` and `full` include them.

### Tool: `get-peers`

Parameters:

- `symbol` *(string, required)* — uppercase ticker, e.g. `AAPL`. 1–20 chars, starts with A–Z.
- `grouping` *(string, optional)* — `industry` (default), `subindustry`, or `sector`.
- `view` *(string, optional)* — `summary` (top 10), `standard` (top 25), `full` (all).

### Tool: `get-financials-snapshot`

Parameters:

- `symbol` *(string, required)* — uppercase ticker.
- `view` *(string, optional)* — `summary`/`standard` (10 curated KPIs), `full` (KPIs + raw upstream metric dictionary).

### Tool: `get-price-summary`

Parameters:

- `symbol` *(string, required)* — uppercase ticker.
- `period` *(string, optional)* — `7d`, `30d` (default), `90d`, or `1y`. The `1y` window uses weekly resolution; the others use daily.
- `view` *(string, optional)* — `summary`/`standard` (aggregated stats), `full` (stats + raw OHLCV arrays).

### Tool: `get-news-pulse`

Parameters:

- `symbol` *(string, required)* — uppercase ticker.
- `view` *(string, optional)* — `summary`/`standard` (top 5 headlines), `full` (all headlines from the past 7 days).

Sentiment fields (`sentiment_score`, `bullish_percent`, `bearish_percent`, `sentiment_source`) are populated only when the upstream `/news-sentiment` endpoint is reachable; they fall back to `null` on premium-locked keys without failing the call.

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
| `rate_limit` | object \| null | Upstream Finnhub quota snapshot `{ remaining, reset_at }` — populated after the first observed upstream response, `null` on cold start. |
| `sentiment_source` | string \| null | Source label for sentiment values. |
| `premium` | bool | Whether the underlying upstream endpoint required a premium key. |

A response that exceeds its declared view's token ceiling is rebuilt by the tool invocation middleware as a `BudgetExceeded` failure envelope; retry with a broader `view` or a sparser `fields` projection.

### Resource: `finnhub://resources/exchanges`

Returns the list of stock exchanges available through the provider as `application/json`.

### Resource: `finnhub://resources/api-status`

Returns the most-recent observed Finnhub upstream quota state as `application/json`:

```json
{
  "remaining": 42,
  "reset_at": "2026-12-31T23:59:59Z",
  "recent_throttled_count": 0
}
```

The `remaining` and `reset_at` fields are `null` before the server has made any upstream call; the `recent_throttled_count` resets to zero whenever the quota window rolls over. Clients can poll this resource to monitor headroom without invoking a tool.

### Response caching

Every Application service is fronted by `Microsoft.Extensions.Caching.Hybrid.HybridCache` with per-endpoint TTL tiers. Identical requests within the tier's TTL short-circuit the upstream Finnhub call.

Tiers (defaults shown):

| Tier | Default TTL | Used for |
|---|---|---|
| `Quote` | 10s | Live market data |
| `News` | 60s | News articles, sentiment, symbol search |
| `Financials` | 1h | Reported financials, KPI snapshots |
| `Profile` | 24h | Company profiles, peer lists |
| `Exchanges` | 7d | Stock exchange catalogues |

TTLs are tunable in `appsettings.json` under the `Cache` section. Bad values (zero or out-of-range) fail startup validation. Cache keys are namespaced with a `tenant=shared` prefix today; a future BYOK milestone partitions per user without a key-shape migration. See `.planning/specs/01-product-surface.md` §3 P2 for the full design.

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
