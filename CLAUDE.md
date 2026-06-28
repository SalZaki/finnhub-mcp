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

The Finnhub API key is read from the `FINNHUB_API_KEY` environment variable, or from `dotnet user-secrets` (`FinnHub:ApiKey`) in `Development`. A legacy `.env` (loaded by `DotNetEnv` in `Development`) is still honoured as a fallback, but it is git-ignored and a `pre-commit` hook blocks it from being staged. Never hardcode the key or commit `.env`.

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

- **Conventional Commits** — release-please depends on them. Enforced by a Husky.Net `commit-msg` hook locally **and** the `PR Title` GitHub Action server-side. Allowed types (the full list, mirrors `.release-please-config.json`): `feat`, `fix`, `perf`, `refactor`, `revert`, `style`, `build`, `ci`, `chore`, `docs`, `test`. Breaking changes via `!` (`feat!:` …) or a `BREAKING CHANGE:` footer.
  - **`release:` is NOT an allowed type.** release-please's own release PR uses `chore(release): release X.Y.Z` (PR #171 was rejected for using `release:`).
  - After a fresh clone: `dotnet tool restore && dotnet husky install` — otherwise the local hook isn't wired and commits go straight to the server-side check.
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
7. **MANDATORY: capture a real Finnhub response** at `tests/Fixtures/finnhub/<endpoint>-<ticker>.json` via `tests/Fixtures/finnhub/capture.sh`, and write at least one client test that loads that fixture. Synthetic payloads have shipped bugs (PR #166, PR #169) — real captures are non-negotiable.
8. **MANDATORY: assert the on-wire URL** in the client test using `_handler.LastRequest!.RequestUri!.AbsoluteUri`. Pin the full expected URL with `Assert.Equal`. Catches the URL-resolution bug class from PR #169 before it ships. See `ConfigureFinnHubClientTests` and any of the `HitsApiV1*Endpoint` tests for the pattern.
9. **MANDATORY: live-smoke against real Finnhub** with the running server before marking the PR ready. `dotnet run` locally, hit each new tool via curl or Claude Code, confirm the response shape. Mocks alone don't catch URL-resolution, follow-redirect, header, or auth issues.
10. **Update README.md** "Currently Available" section.

### Debugging discipline (added 2026-05-23)

When a live error is reported, the **first** step is to read the actual server log / stack trace — not to guess the most plausible code-review explanation. The financials misdiagnosis (PR #166 fixed a real bug but not the user-reported one; PR #169 was the actual fix) cost a full release cycle because the investigation skipped this step. Stack traces from the running process are authoritative; "most plausible code path" guesses are not.

## Adding a new MCP resource

Same shape as a tool, but lives in `Server/Resources/<Group>/` and is exposed via `finnhub://resources/<name>` rather than as a callable tool. Resources are read-only and should be cacheable.

## Performance & token discipline

This server is consumed by LLMs — tokens are the constraint, not latency.

- **Default to slim projections.** Tools should return a curated summary by default; opt into `view: "full"` for the raw payload.
- **Aggregate server-side.** Don't return 250 candle rows when a summary stat is what the LLM wants.
- **Source-generated JSON only.** Reflection-based serialization defeats AOT and inflates startup.
- **Cache by mutability tier.** Profile data (24h), financials (1h), quotes (10s). Use `HybridCache` with an explicit TTL per endpoint.

## Resilience

The Finnhub HTTP clients are wired with hand-rolled Polly v8 policies (`Microsoft.Extensions.Http.Polly` + `Polly`) in `ServiceCollectionExtension.cs` — a retry policy and a circuit breaker registered per client via `AddPolicyHandler`. Don't add ad-hoc `try/catch` around HTTP calls; configure these policies instead. We deliberately do **not** use `Microsoft.Extensions.Http.Resilience.AddStandardResilienceHandler` — keeping the policies explicit makes the 403/401 premium-gating and the rate-limit-header observation handler obvious; that package is intentionally unreferenced.

The retry policy (3 attempts) retries transient/5xx responses, `HttpRequestException`, and **only timeout-caused** `TaskCanceledException` (`ex.InnerException is TimeoutException`) — a caller cancellation propagates immediately instead of burning ~14s of backoff on a request nobody is waiting for. Backoff is `2^n` seconds plus up to 1s of jitter, so concurrent clients don't retry in lockstep during a 5xx storm.

For 403/401 responses (premium-locked endpoints, premium-gated `/stock/symbol` exchanges): excluded from both the retry and the circuit breaker — they are permanent per-key failures. The clients surface them as a typed `ApiClientPremiumRequiredException`; do **not** retry through Polly.

## HTTP clients and URI resolution

Two URL-construction patterns exist in the codebase. Both work; **don't mix them by accident**:

- **Absolute** — `FinnHubSearchApiClient.BuildRequestUri` concatenates `BaseUrl + endpoint` manually. Slash-tolerant in either direction.
- **Relative-with-slashed-BaseAddress** — all six Wave A/B clients use `client.SendAsync(new HttpRequestMessage(Get, "stock/profile2?…"))`. This relies on `HttpClient.BaseAddress` having a **trailing slash**. Per RFC 3986, without the slash the last path segment is *replaced* (`/api/v1` + `stock/profile2` → `/api/stock/profile2`, dropping `/v1` silently — surfaces as `InvalidResponse` from HTML deserialization on the Finnhub landing page).

`ConfigureFinnHubClient` in `ServiceCollectionExtension.cs` defensively appends the trailing slash if the configured `BaseUrl` lacks one. **Do not strip that normalization in a cleanup PR** — it's load-bearing (PR #169). The XML comment on the method explains why.

Every client must have a regression test that pins the on-wire URL:
```csharp
Assert.Equal("https://finnhub.io/api/v1/stock/profile2?symbol=AAPL",
             _handler.LastRequest!.RequestUri!.AbsoluteUri);
```
See the `Hits<...>Endpoint` tests in each `Clients/<Group>/*Tests.cs` for the pattern.

## Test fixtures (real Finnhub captures)

`tests/Fixtures/finnhub/*.json` holds frozen real responses captured via `tests/Fixtures/finnhub/capture.sh` (needs `FINNHUB_API_KEY`). Client tests load them through `Fixture.LoadFinnHub("name")`.

**Why we don't use synthetic test payloads:** synthetic data masks shape drift — twice now. PR #166 fixed mixed-type values in `/stock/metric` (date strings interleaved with numeric KPIs); PR #167 fixed null `change`/`percent_change` in `/quote` for unknown symbols. Both passed the original synthetic tests and broke live.

Each Infrastructure test csproj copies fixtures into the test bin directory:
```xml
<Content Include="..\Fixtures\finnhub\*.json"
         LinkBase="Fixtures\finnhub"
         CopyToOutputDirectory="PreserveNewest" />
```

Refresh fixtures when Finnhub changes a shape or you add an endpoint.

## CI & releases — single-branch, release-please on `main`

**Branch model:** `main` is the only long-lived branch. Feature/fix/ci PRs land on `main`; releases are cut from `main` by release-please. There is **no `release` branch** — the gated `main → release` promotion model was retired on 2026-06-27 (it caused a squash-promotion empty-release and an accidental branch-deletion while cutting v1.21.0). This matches the simpler single-branch flow used in the sibling `doodleworks-mcp` repo.

**Workflow files:**
- `.github/workflows/dotnet.yml` — format + build + tests on every push to `main` and every PR targeting `main`.
- `.github/workflows/release.yml` — triggers on push to `main` (+ `workflow_dispatch`): `release-please` opens/updates a release PR; merging that PR creates the tag + GitHub Release, which gates the 6-platform build matrix + `publish-npm`. No separate `validate` job — `dotnet.yml` already validates the same pushes/PRs.
- `.github/workflows/pr-title.yml` — validates PR titles against Conventional Commits.

**Shipping a release:**
1. Land feature/fix PRs on `main` as normal (squash or merge — the squash footgun is gone now that there's no promotion step to collapse).
2. release-please maintains a `chore(release): release X.Y.Z` PR on `main` (branch `release-please--branches--main`) with the version bump + CHANGELOG.
3. **Merge that release PR** → tag `vX.Y.Z` + GitHub Release + 6-platform binaries + npm publish (all 7 packages via OIDC trusted publishing).

> release-please bumps off the conventional-commit titles since the last release (`fix:` → patch, `feat:` → minor). Because feature PRs land directly on `main`, it always sees the real titles — no special merge discipline for the release PR.

**Things to know:**
- `CHANGELOG.md` and `.release-please-manifest.json` are release-please-managed; don't edit by hand (the one exception was the 2026-06-27 single-branch migration, which seeded `main` with the v1.21.0 state + a `last-release-sha` in `.release-please-config.json`).
- npm publishing is **tokenless via OIDC trusted publishing** (configured per package on npmjs.com) — no `NPM_TOKEN`. The `publish-npm` job has `id-token: write` + `--provenance`, and a smoke-pack guard runs before publish.
- `main` is protected by ruleset 5687245 (require signatures, no force-push, no deletion; owner bypass). Requiring the CI status checks on `main` is recommended but not yet enabled.

## Coverage

`dotnet test --settings coverlet.runsettings` produces real per-project coverage XML. Codecov ingests the results on every PR and enforces:

- **Project floor: 85% line.** Aggregate coverage across the codebase. PRs that drop below this fail the Codecov check.
- **Patch floor: 80% line.** New / changed code in a PR must be ≥80% covered.

**Excluded from measurement** — both because the only meaningful test for them is end-to-end host bootstrap, which the live-smoke workflow covers:

- `Program.cs` — via `ExcludeByFile` in `coverlet.runsettings`
- `ServiceCollectionExtension.cs` — via `[ExcludeFromCodeCoverage]` on the class with an XML doc explaining why

Baseline at time of writing (PR #179 + #180 follow-up): **98% line / 90% branch / 100% method**. 437 tests across `FinnHub.MCP.Server.Tests.Unit`, `Application.Tests.Unit`, `Infrastructure.Tests.Unit`, and `LiveSmoke`.

If coverage drops below the floor, your options are:
1. Add tests covering the gap (preferred).
2. Decorate the new code with `[ExcludeFromCodeCoverage]` and document why in the XML doc / commit message (rare — only for genuinely untestable code like DI wiring or host entry points).
3. Admin-merge with explicit justification (escape valve, same as for the CI-flake cases).

## Live smoke

`.github/workflows/live-smoke.yml` runs daily at 07:00 UTC and on `workflow_dispatch`. It boots the server in-process via `WebApplicationFactory<Program>`, walks every MCP tool with `AAPL` (plus the unknown-symbol edge case for `get-quote`), and asserts `IsSuccess=true` or a tolerated typed failure (`PremiumRequired` / `NotFound`). On failure it opens an `upstream-drift`-labeled issue with the failing-run URL and a runbook pointer; de-dupes against any existing open `upstream-drift` issue from the last 24 hours.

The live-smoke project is `tests/FinnHub.MCP.Server.Tests.LiveSmoke/`, tagged `[Trait("Category", "LiveSmoke")]` so `dotnet test` in CI (`dotnet.yml` and `release.yml`) excludes it via `--filter "Category!=LiveSmoke"`. Only the live-smoke workflow opts in. This protects the Finnhub quota on PR builds.

Full runbook (failure causes, key rotation, local-run instructions) lives in `CONTRIBUTING.md → "Live-smoke runbook"`.

## Planning

Roadmap and active design notes live in `.planning/`:

- `.planning/specs/01-product-surface.md` — Milestone 01, token-conscious tool fabric (envelope, middleware, HybridCache, P6 aggregation tools, P7 search-tools, P8 prompts)
- `.planning/specs/02-distribution.md` — Milestone 02, hosted BYOK + AOT + Docker + dotnet tool + MCP registry

Treat these as authoritative when they exist. Intermediate working files (discussion notes, research dumps, review scratch) are gitignored — if you need them they live locally only.

Day-to-day tracking happens through GitHub Issues — see the next section. Issue bodies cite the relevant `.planning/specs/*.md` section in their `## Roadmap reference` field so the trace from issue → spec is always one click away.

## Issue workflow

All planned development is tracked through GitHub Issues with a strict hierarchy:

```
Epic            — milestone-sized initiative
 └─ Feature     — discrete capability inside an Epic
     └─ User Story — one end-user-facing increment, sized to a single PR
         └─ Sub-task   — atomic engineering work item inside a Story
```

Authoritative doc: [`docs/ISSUE_WORKFLOW.md`](docs/ISSUE_WORKFLOW.md).

- **Templates** — YAML forms in `.github/ISSUE_TEMPLATE/{epic,feature,user-story,sub-task}.yml` with required fields, field validation, and auto-applied type labels. Legacy markdown templates were removed (they shadowed the YAML); only `bug_report.md` and `feature_request.md` remain for community contributions. New work files into the YAML forms.
- **GitHub Milestones** — `M1 — Token-Conscious Tool Fabric`, `M2 — Distribution & Hosting`, `M3 — Production Hardening`. Every Epic is assigned to one. Due dates currently unset; revisit per cycle.
- **Labels** — type (`type:epic|feature|story|subtask`, applied by template), priority (`priority:critical|high|medium|low`), status (`status:backlog|ready|in-progress|blocked|in-review`), area (`area:tool|resource|prompt|meta-tool|transport|auth|cache|observability|hosting|distribution|testing|ci`), milestone (`milestone:m1-fabric|m2-distribution|m3-hardening`). Standard GitHub labels (`bug`, `documentation`, etc.) preserved for community use.
- **Parent linkage** — use **both** GitHub's native sub-issues (via `gh api graphql -F query='mutation { addSubIssue(input: { issueId, subIssueId }) ... }'` — full snippet in `docs/ISSUE_WORKFLOW.md`) AND a textual `Parent <Level>: #N` line on the first line of the body. The first gives the GitHub UI parent-child navigation + progress bar; the second survives in markdown views and exports.
- **Naming** — `[Epic <N>]`, `[Feature <N.M>]`, `[User Story <N.M.P>] As a <role>, I want <capability>`, `[Task] <verb-led short title>`. User Story bodies use the strict format `As a <role>, I want <capability>, so that <outcome>.` — no deviations.
- **Branch naming** — `<type>/<short-description>` matching the Conventional Commits type the PR will land as (e.g. `feat/get-calendar-earnings-dispatch`, `chore/coverage-tightening`, `fix/live-smoke-unknown-symbol-assertion`).
- **Quality bar** — no filler (`robust`, `seamless`, `leverage`, `comprehensive`, etc.), testable Given/When/Then acceptance criteria, self-contained bodies that an engineer can pick up cold, no fabricated requirements (mark `TBD` if the spec doesn't say).

The 187-issue migration (Phase B, 2026-05-25) established this structure. The legacy issues `#42`, `#95`, `#96`, `#97` were closed with explanatory comments pointing to the new hierarchy. New work files into the existing hierarchy; new Epics get filed only for genuinely new milestone-sized initiatives.

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
- **Don't use `release:` as a Conventional Commit type.** release-please's release PR is `chore(release): …`. The PR-title validator will block `release:` (PR #171).
- **Don't `git commit --no-verify`** to skip the commit-msg hook. The server-side PR-title check will still reject the merge, so you'd be deferring pain rather than avoiding it.
- **Don't strip the trailing-slash normalization in `ConfigureFinnHubClient`.** It looks like dead code but it prevents the URL-resolution bug class from PR #169. The XML comment on the method explains why.
- **Don't recreate the `release` branch or re-add a `main → release` promotion step.** Releases are single-branch on `main` via release-please as of 2026-06-27; the gated-promotion model was retired after it caused a squash empty-release and an accidental branch deletion (see "CI & releases" above).
- **Don't ship synthetic-payload-only client tests.** Real Finnhub fixtures live under `tests/Fixtures/finnhub/`; use them. Mocks bypass URL resolution AND don't catch upstream shape drift.
- **Don't guess the fix from a code-review reading when a live error is reported** — read the server log first (see "Debugging discipline" above). The financials misdiagnosis cost a release cycle for this exact reason.
- **Don't file development work without a GitHub Issue.** Every PR that ships scope should reference one (`Closes #N` in the body). One-off bug fixes filed reactively are the only exception, and even those should get an issue retroactively if the fix is non-trivial.
- **Don't bring back the markdown issue templates.** `epic.md`, `feature.md`, `user_story.md`, `task.md` were replaced by the YAML forms in PR #188 because the YAML versions enforce required fields and labels. The legacy files are gone; recreating them silently shadows the YAML and breaks the gate.
- **Don't put a User Story under an Epic without an intermediate Feature.** The hierarchy is `Epic → Feature → User Story → Sub-task`. Skipping a level breaks the sub-issue parent navigation and the GitHub Project rollups. If the Feature is genuinely a placeholder, file it anyway with `TBD` content rather than pointing the Story at the Epic.
