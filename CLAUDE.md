# CLAUDE.md

Guidance for AI coding agents Claude Code, Cursor, etc. working in this repository.

## Project overview

`finnhub-mcp` is a Model Context Protocol (MCP) server, built on the official `ModelContextProtocol` C# SDK, that exposes Finnhub's financial-data APIs to MCP-compatible clients (Claude Desktop, IDE assistants, agents). The server supports HTTP and STDIO transports and follows Clean Architecture across three projects.

## Build, test, format

```bash
dotnet build                                  # build all projects
dotnet test                                   # run all xUnit test projects
dotnet test --settings coverlet.runsettings   # tests with coverage
dotnet format                                 # apply formatting + analyzers
dotnet watch --project src/FinnHub.MCP.Server # hot reload (HTTP transport)
```

`TreatWarningsAsErrors` is enabled in `Directory.Build.props` - any analyzer or compiler warning fails the build. Don't suppress warnings; fix them.

## Running locally

```bash
dotnet run --project src/FinnHub.MCP.Server                  # HTTP on :8080
dotnet run --project src/FinnHub.MCP.Server -- --stdio       # STDIO transport
```

The Finnhub API key is read from the `FINNHUB_API_KEY` environment variable, or from a local `.env` (loaded by `DotNetEnv` in `Development` only). Never hardcode the key.

## Project layout

```
src/
├── FinnHub.MCP.Server/                # ASP.NET Core host, MCP transport, Tools, Resources
├── FinnHub.MCP.Server.Application/    # Domain models, queries, services, exceptions
└── FinnHub.MCP.Server.Infrastructure/ # Finnhub HTTP client, DTOs, JSON context, DI
tests/
├── FinnHub.MCP.Server.Application.Tests.Unit/
├── FinnHub.MCP.Server.Infrastructure.Tests.Unit/
└── FinnHub.MCP.Server.Tests.Unit/
```

Dependency direction: `Server` → `Application` ← `Infrastructure`. The `Application` project must not reference `Server` or `Infrastructure`.

## Conventions

- **Conventional Commits** — release-please depends on them. Use `feat:`, `fix:`, `chore:`, `docs:`, `refactor:`, `test:`, `ci:`, `build:`. Breaking changes via `!` or a `BREAKING CHANGE:` footer.
- **Source-generated JSON** — every DTO must have an entry in the `JsonSerializerContext` partial class in `Infrastructure/Serialization`. No reflection-based `JsonSerializer.Serialize<T>` without a context.
- **Strongly-typed options** — bind from `appsettings.json` via `IOptions<T>` with data-annotation validation on startup.
- **No secrets in source** — API keys, tokens, anything sensitive: environment variables only.
- **Input validation at the tool boundary** — regex-based length and character constraints on every tool argument. Look at the existing `search-symbol` tool for the pattern.
- **Tests use NSubstitute** — not Moq. Existing patterns in `tests/` are authoritative.

## Adding a new MCP tool (recipe)

1. **Define the tool** in `src/FinnHub.MCP.Server/Tools/<Group>/`. Mirror the structure of `Tools/Search/SearchSymbolTool.cs`.
2. **Add input validation** with `[GeneratedRegex]` patterns and length bounds matching the existing style.
3. **Add the request DTO and response DTO** in `Infrastructure/Dtos/` and register both in `JsonSerializerContext`.
4. **Add an Application service / query** for the domain logic in `Application/<Feature>/`.
5. **Register everything in DI** — `Server/Program.cs` for the tool, `Infrastructure/Extensions/` for the client/services.
6. **Write unit tests in all three test projects** — tool-level, application-level, infrastructure-level. NSubstitute mocks at the boundary you're testing.
7. **Update README.md** "Currently Available" section.

## Adding a new MCP resource

Same shape as a tool, but lives in `Server/Resources/<Group>/` and is exposed via `finnhub://resources/<name>` rather than as a callable tool. Resources are read-only and should be cacheable.

## Performance & token discipline

This server is consumed by LLMs — tokens are the constraint, not latency.

- **Default to slim projections.** Tools should return a curated summary by default; opt into `view: "full"` for the raw payload.
- **Aggregate server-side.** Don't return 250 candle rows when a summary stat is what the LLM wants.
- **Source-generated JSON only.** Reflection-based serialization defeats AOT and inflates startup.
- **Cache by mutability tier.** Profile data (24h), financials (1h), quotes (10s). Use `HybridCache` with an explicit TTL per endpoint.

## Resilience

The Finnhub HTTP client is wired with `Microsoft.Extensions.Http.Resilience` (Polly v8) — retry, timeout, circuit-breaker. Don't add ad-hoc `try/catch` around HTTP calls; configure the resilience pipeline instead.

For 403 responses (premium-locked endpoints): handle these as a typed exception, do **not** retry through Polly. Mark the endpoint as premium-gated and return a structured error.

## CI & releases

- `.github/workflows/dotnet.yml` runs build + test on every push.
- `.github/workflows/release.yml` uses `release-please` in manifest mode. Versioning is driven by Conventional Commit prefixes — write your commit messages accordingly.
- `CHANGELOG.md` is generated; don't edit it by hand.

## Planning

Roadmap and active design notes live in `.planning/`. Start with `.planning/ROADMAP.md` to see what's planned. Phase-level `SPEC.md` and `PLAN.md` documents describe ratified scope and approach; treat them as authoritative when they exist.

Intermediate working files (discussion notes, research dumps, review scratch) are gitignored — if you need them they live locally only.

## AI agent skills (gstack)

This repo is compatible with [gstack](https://github.com/garrytan/gstack) — an optional Claude Code skill pack. Install is per-developer (`~/.claude/skills/gstack`); no repo dependency, no commits required.

If you have gstack installed, prefer these skills for the relevant workflow:

- `/review` — pre-merge diff review against the base branch
- `/cso` — OWASP + STRIDE security audit (relevant for `FINNHUB_API_KEY` handling and the external HTTP boundary)
- `/investigate` — systematic root-cause debugging
- `/office-hours`, `/plan-ceo-review`, `/plan-eng-review` — feature planning before writing code
- `/ship`, `/land-and-deploy` — PR creation, CI wait, post-merge verification

Browser-driven skills (`/qa`, `/design-*`, `/canary`, `/benchmark`) don't generally apply — this server has no UI. `/qa` can still drive the HTTP transport for endpoint smoke tests.

When any AI agent needs to browse, prefer gstack's `/browse` over `mcp__claude-in-chrome__*` tools if both are available.

## What not to do

- Don't bypass `TreatWarningsAsErrors` by suppressing warnings.
- Don't add reflection-based JSON serialization.
- Don't add a third-party HTTP client — use the typed `HttpClient` registered in `Infrastructure`.
- Don't introduce a different mocking library — stick with NSubstitute.
- Don't commit `.env`, API keys, or any secrets.
- Don't edit `CHANGELOG.md` directly.
