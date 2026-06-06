
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
- ✅ **Exchanges Resource** exposed at `finnhub://resources/exchanges` — the full catalog of stock venues Finnhub supports (79 exchanges), served from Finnhub's published reference list
- ✅ **Resilient HTTP Communication** — typed `HttpClient` with retry, timeout, and circuit-breaker policies via `Microsoft.Extensions.Http.Resilience` and Polly
- ✅ **Source-generated JSON** through `System.Text.Json` `JsonSerializerContext` for low-allocation, AOT-friendly (de)serialization
- ✅ **Strongly-typed configuration** — `FinnHubOptions` bound from `appsettings.json` with data-annotation validation on startup
- ✅ **API key kept out of source** — read from the `FINNHUB_API_KEY` environment variable (or a local `.env` in development via `DotNetEnv`)
- ✅ **Dual transport** — HTTP (`MapMcp`) for hosted scenarios and STDIO for desktop MCP clients

## 🚧 Available & Upcoming MCP Capabilities

### ✅ Currently Available

**Tools (12):**
- **`search-tools`** — intent-based tool discovery: pass a natural-language `intent` (max 200 chars) and get back the most relevant tools, ranked by a pure-C# BM25 keyword index over each tool's name, title, description, and curated example intents. Keeps full tool schemas off the wire until a tool is actually needed. `summary` view omits per-tool descriptions to stay token-light; `standard`/`full` include them.
- **`search-symbol`** — search for financial symbols by ticker, company name, ISIN, or CUSIP, optionally filtered by exchange code (limit 1–100, default 10). On a high-confidence exact match, suggests `get-quote`, `get-company-profile`, `get-news-pulse`, `get-financials-snapshot`, `get-price-summary`, and `get-peers` as next actions.
- **`get-quote`** — real-time price snapshot (current, change, percent change, session high/low/open, prev close, timestamp). Cached at the 10-second Quote tier.
- **`get-company-profile`** — company snapshot (name, ticker, country, currency, exchange, IPO, market cap, shares outstanding, industry). `view=summary` drops the cosmetic fields (logo, phone, weburl); `standard` and `full` include them.
- **`get-peers`** — peer ticker list for a symbol, optionally grouped by `industry` (default), `subindustry`, or `sector`. Summary view caps at 10 peers, standard at 25, full returns all.
- **`get-financials-snapshot`** — curated 10-KPI snapshot (market cap, P/E, P/B, EPS, dividend yield, 52-week high/low, 52-week return, beta, revenue per share). `view=full` adds the raw upstream metric dictionary.
- **`get-price-summary`** — aggregated price stats over a candle range (`min`, `max`, `mean`, `return_pct`, `vol`, `latest`). Period: `7d`, `30d` (default), `90d`, `1y`. `view=full` adds the raw OHLCV arrays.
- **`get-news-pulse`** — news pulse over the past 7 days: sentiment score (when available), top 5 headlines, article count, week-over-week delta. Gracefully degrades sentiment when the upstream `/news-sentiment` endpoint is premium-locked.
- **`get-calendar`** — parameter-dispatched calendar lookup across three feeds: `kind=earnings` (max 90-day window, optional symbol filter; suggests `get-financials-snapshot` + `get-news-pulse`), `kind=ipo` (max 365-day window, no symbol filter; suggests `get-company-profile` for the most recent tradable IPO), and `kind=economic` (max 90-day window, optional ISO 3166-1 alpha-2 country filter applied server-side since the upstream doesn't accept it). Summary view caps at 10 events, standard at 25, full returns the complete window.
- **`get-insider-signal`** — aggregated insider-transaction signal for a symbol over the trailing 30 days (`from`/`to` optional, max 90-day window). Returns `net_buy_sell_30d` (signed share delta), `notable_names` (top 5 by absolute trade volume), `total_count`, and `latest`; `view=full` includes the full transaction array. Suggests `get-company-profile` and `get-quote` as next actions.
- **`get-recommendations`** — analyst-consensus snapshot for a symbol with `change_vs_prev` (per-bucket delta + single-label sentiment shift). Returns `consensus` ('Strong Buy' / 'Buy' / 'Hold' / 'Sell' / 'Strong Sell'), the 5 rating-bucket counts, and `total`; `view=full` includes the per-period history. Cached at the Profile tier — one upstream call serves both the current and previous-period values. Suggests `get-financials-snapshot` and `get-peers` as next actions.
- **`get-exchange-symbols`** — aggregated, token-conscious view of the symbols listed on an exchange (`exchange` code, e.g. `US`): `total_count`, a `type_breakdown` (count per security type), and a capped sample — **not** the raw list (a major exchange lists tens of thousands of symbols). `summary` returns count + breakdown only, `standard` adds 25 sample rows, `full` adds up to 100. Cached at the 7-day Exchanges tier. Free Finnhub plans only support `US`; other exchanges are premium-gated. Suggests `search-symbol` as the next action for resolving a specific ticker.

Every tool returns the standard token-budgeted envelope with cross-linked `next_actions` and the most-recent observed Finnhub rate-limit headers.

**Resources (3):**
- **`finnhub://resources/capabilities`** — the full machine-readable catalog of every registered tool (`name`, `title`, `description`, `category`, `examples`, `premium`) plus `total_count`. Enumerate the whole surface in one read instead of issuing repeated `search-tools` calls; backed by the same `IToolRegistry` the meta-tool ranks over.
- **`finnhub://resources/exchanges`** — the full catalog of stock venues Finnhub supports (79 exchanges: code, name, country, MIC, timezone, market hours). `url` is `null` for the few venues Finnhub lists without a reference link.
- **`finnhub://resources/api-status`** — latest observed Finnhub upstream quota: `remaining`, `reset_at`, and a rolling 429 count

**Prompts (1):**
- **`/research-ticker {symbol}`** — Claude Desktop slash command that renders a deterministic research workflow: resolve the symbol (via `search-symbol`), then pull `get-price-summary`, `get-financials-snapshot`, and `get-news-pulse`, and synthesise a brief. A pure template — no server-side LLM calls, so the same symbol always renders byte-identical text.

### 📋 Planned
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

### Tool: `search-tools`

Parameters:

- `intent` *(string, required)* — natural-language description of the task, 1–200 chars (letters/digits/space and `- _ . , ' ? & / ( )`). Over 200 chars or other characters are rejected as a validation error.
- `view` *(string, optional)* — `summary` (default) omits per-tool descriptions; `standard`/`full` include them.

Returns the ranked matches (`name`, `title`, `score`, `category`, `premium`, and — outside `summary` — `description`) plus `total_matches`. Ranking is a pure-C# BM25 keyword index over name + title + description + curated example intents (no embeddings, no external dependency); examples are weighted above the description so curated intents drive the match. The `search-tools` entry itself is excluded from its own results. A drift test fails the build if a tool is registered on the server without a catalog descriptor.

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

### Resource: `finnhub://resources/capabilities`

Returns the full tool catalog as `application/json` — one entry per registered tool (`name`, `title`, `description`, `category`, `examples`, `premium`) plus `total_count`. It reads from the same `IToolRegistry` the `search-tools` meta-tool ranks over, so the catalog and the ranker never disagree; a `CapabilitiesResourceTests` drift test fails the build if a registered tool is missing from the payload.

```json
{
  "tools": [
    {
      "name": "get-quote",
      "title": "Get Quote",
      "description": "…",
      "category": "Pricing",
      "examples": ["current stock price right now", "latest real-time quote"],
      "premium": false
    }
  ],
  "total_count": 11
}
```

### Resource: `finnhub://resources/exchanges`

Returns the full catalog of stock venues Finnhub supports (79 exchanges) as `application/json`. Finnhub exposes no `/stock/exchange` API endpoint — the supported-exchange list is published only as a reference document — so the catalog ships as in-process reference data captured from Finnhub's published "Supported Exchanges" sheet rather than a live upstream call.

```json
{
  "exchanges": [
    {
      "code": "US",
      "name": "US exchanges (NYSE, Nasdaq)",
      "mic": "XNYS,XASE,BATS,ARCX,XNMS,XNCM,XNGS,IEXG,XNAS, OTCM, OOTC",
      "time_zone": "America/New_York",
      "pre_market_hours": "04:00-09:30",
      "trading_hours": "09:30-16:00",
      "post_market_hours": "16:00-20:00",
      "close_date": "7,0",
      "country_code": "US",
      "country_name": "US",
      "url": "https://www.tradinghours.com/exchanges/nyse"
    }
  ],
  "total_count": 79,
  "has_results": true
}
```

`url` is `null` for the few venues Finnhub lists without a reference link.

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
