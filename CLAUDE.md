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
  - **`release:` is NOT an allowed type.** Promotion PRs from `main → release` must use `chore(release): promote main to release` (PR #171 was rejected for using `release:`).
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

The Finnhub HTTP client is wired with `Microsoft.Extensions.Http.Resilience` (Polly v8) — retry, timeout, circuit-breaker. Don't add ad-hoc `try/catch` around HTTP calls; configure the resilience pipeline instead.

For 403 responses (premium-locked endpoints): handle these as a typed exception, do **not** retry through Polly. Mark the endpoint as premium-gated and return a structured error.

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

## CI & releases — gated through the `release` branch

**Branch model:**
- `main` — every feature/fix/ci PR lands here as normal (squash or merge, your call)
- `release` — the ship gate; releases only happen from here

**Workflow files:**
- `.github/workflows/dotnet.yml` runs format + build + tests on every push to `main`/`develop`/`release` and every PR targeting `main`/`develop`
- `.github/workflows/release.yml` triggers **only** on push to `release` — runs `validate` (format + tests) → `release-please` (uses `target-branch: release` — required, see PR #162) → 6-platform build matrix on success
- `.github/workflows/pr-title.yml` validates PR titles against Conventional Commits

**Validate runs BEFORE release-please.** If tests fail, no tag is created. (PR #169 corrected this — previously failed validate left an orphan tag and GitHub release with no artifacts.)

**Shipping a release:**
1. Open a `main → release` PR titled `chore(release): promote main to release`
2. **Merge it with `--merge` (NOT `--squash`)** → validate runs on `release`
3. release-please opens its own PR against `release` with the version bump + CHANGELOG entry (`release-please--branches--release`)
4. **Merge that with `--merge` (NOT `--squash`)** → tag + GitHub release + 6-platform build matrix triggers

> **Why `--merge` and not `--squash`?** release-please walks the commit graph since the last release tag and bumps the version off the conventional-commit titles it finds (`fix:` → patch, `feat:` → minor). A squash replaces every inner commit with a single `chore(release):` commit. release-please then sees only a hidden `chore:` type, finds nothing user-facing, and skips releasing — leaving you with an empty cycle. A regular merge preserves the inner titles in release's history. PR #178 was squash-merged and produced no release; PR #180 was the recovery merge that brought the inner `fix(ci):` commits back into release's history so release-please could cut v1.20.1.

**Things to know:**
- `CHANGELOG.md` is generated; don't edit it by hand.
- After a promotion lands on `release`, GitHub's UI will offer a "compare and pull request" prompt suggesting `release → main`. **It's noise** — that direction is circular (release-please adds CHANGELOG + manifest commits to `release` that main doesn't have; merging them back is meaningless). Dismiss the banner.
- Branch protection on `release` is enabled (require PR + `Validate Release Branch` status check + no force push + no deletions). Direct pushes are blocked; the admin-merge escape valve remains for the Codecov-on-introducing-PR class of chicken-and-egg cases.

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
- **Don't use `release:` as a Conventional Commit type.** Promotion PRs are `chore(release): …`. The PR-title validator will block `release:` (PR #171).
- **Don't `git commit --no-verify`** to skip the commit-msg hook. The server-side PR-title check will still reject the merge, so you'd be deferring pain rather than avoiding it.
- **Don't strip the trailing-slash normalization in `ConfigureFinnHubClient`.** It looks like dead code but it prevents the URL-resolution bug class from PR #169. The XML comment on the method explains why.
- **Don't merge GitHub's "compare and pull request" prompt for `release → main`.** It's a circular merge (see "Gated release model" above).
- **Don't ship synthetic-payload-only client tests.** Real Finnhub fixtures live under `tests/Fixtures/finnhub/`; use them. Mocks bypass URL resolution AND don't catch upstream shape drift.
- **Don't guess the fix from a code-review reading when a live error is reported** — read the server log first (see "Debugging discipline" above). The financials misdiagnosis cost a release cycle for this exact reason.
- **Don't `--squash` promotion PRs from `main → release`.** The squash collapses every inner commit into a single `chore(release): promote main to release` commit, hiding the `fix:` / `feat:` titles that drive release-please's version bump. release-please walks the squashed history, finds only `chore(release):` (hidden type), and skips releasing — leaving you with an empty release cycle that has to be repaired by a follow-up `--merge`. Always use `gh pr merge --merge` for promotion PRs. PR #178 was the canary; PR #180 was the recovery merge that brought the inner `fix(ci):` commits back into release's history.
- **Don't file development work without a GitHub Issue.** Every PR that ships scope should reference one (`Closes #N` in the body). One-off bug fixes filed reactively are the only exception, and even those should get an issue retroactively if the fix is non-trivial.
- **Don't bring back the markdown issue templates.** `epic.md`, `feature.md`, `user_story.md`, `task.md` were replaced by the YAML forms in PR #188 because the YAML versions enforce required fields and labels. The legacy files are gone; recreating them silently shadows the YAML and breaks the gate.
- **Don't put a User Story under an Epic without an intermediate Feature.** The hierarchy is `Epic → Feature → User Story → Sub-task`. Skipping a level breaks the sub-issue parent navigation and the GitHub Project rollups. If the Feature is genuinely a placeholder, file it anyway with `TBD` content rather than pointing the Story at the Epic.
- **Don't push directly to `release`.** Branch protection blocks it. Use a promotion PR with `--merge` (see above). The admin-merge escape valve exists for the Codecov-on-introducing-PR class only, not for routine pushes.
