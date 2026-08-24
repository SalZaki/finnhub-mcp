---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E11
title: "User Experience, Performance and Quota Control"
priority: P1
phase: "M3 — Scale & Ecosystem"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 6
  subtasks: 20
  traceability_owned: 6
  traceability_items: 11
story_estimate_days: 43
subtask_estimate_hours: 318
---

<a id="id-e11"></a>
# E11 — User Experience, Performance and Quota Control

This is the self-contained coding-agent backlog for E11. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

## Coding Agent Rules

1. Keep every ID immutable and preserve Feature → Epic, Story → Feature, and Subtask → Story parentage.
2. Create parent items before child items and treat dependency links as blockers.
3. Preserve priorities, estimates, labels, source findings, statuses, owner roles, and deliverables.
4. Complete every acceptance criterion and subtask before closing a Story.
5. Follow cross-Epic dependency links to the sibling Markdown files in this package.
6. Keep every included traceability row covered or replace it with demonstrably equivalent coverage.
7. Apply the Delivery Guide and relevant Roadmap gates before declaring this Epic complete.

## 1. Portfolio Slice

| Epic | Priority | Features | Stories | Subtasks | Story Days | Subtask Hours | Phase | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E11 | P1 | 5 | 6 | 20 | 43 | 318 | M3 — Scale & Ecosystem | Not Started |

## 2. Epic Definition

**Objective:** Give users approachable exploration surfaces and consistently fast, quota-efficient behavior under local and hosted workloads.

**Business value:** Shortens onboarding, makes failures actionable and improves perceived reliability on Finnhub's constrained free tier.

**Exit criteria:**

- [ ] A safe local web explorer and interactive CLI can discover, invoke and inspect every supported capability.
- [ ] Caching, stale fallback and opt-in warming meet documented latency targets without broad free-tier consumption.
- [ ] Quota admission is proactive, weighted and understandable to clients, including multi-instance deployments.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F11.01](#id-f11-01) | P2 | Safe local web explorer | 1 | 8 | Not Started |
| [F11.02](#id-f11-02) | P1 | Interactive CLI | 1 | 7 | Not Started |
| [F11.03](#id-f11-03) | P1 | Cache and latency optimization | 2 | 14 | Not Started |
| [F11.04](#id-f11-04) | P0 | Proactive quota management | 1 | 8 | Not Started |
| [F11.05](#id-f11-05) | P1 | Performance SLOs and regression tests | 1 | 6 | Not Started |

<a id="id-f11-01"></a>
### F11.01 — Safe local web explorer

- **Parent Epic:** [E11](#id-e11)
- **Priority:** P2
- **Status:** Not Started

**Description:** Provide a local-first UI for tool discovery, fixture demos, invocation and response inspection.

**Expected outcome:** Non-developers can evaluate the server without exposing a shared Finnhub key or generic public proxy.

**Stories:**

- [US11.01.01](#id-us11-01-01) — Explore tools in a safe local web UI (P2, 8d)

<a id="id-f11-02"></a>
### F11.02 — Interactive CLI

- **Parent Epic:** [E11](#id-e11)
- **Priority:** P1
- **Status:** Not Started

**Description:** Add guided discovery, validation, streaming output and diagnostics for terminal users.

**Expected outcome:** Developers can verify configuration and exercise workflows faster than hand-writing MCP payloads.

**Stories:**

- [US11.02.01](#id-us11-02-01) — Provide an interactive diagnostic CLI (P1, 7d)

<a id="id-f11-03"></a>
### F11.03 — Cache and latency optimization

- **Parent Epic:** [E11](#id-e11)
- **Priority:** P1
- **Status:** Not Started

**Description:** Canonicalize cached provider responses, add optional L2, tune TTLs and use safe stale/negative caching.

**Expected outcome:** Repeated research is faster and consumes fewer requests without serving misleading freshness.

**Stories:**

- [US11.03.01](#id-us11-03-01) — Canonicalize and tier cache storage (P1, 8d)
- [US11.03.02](#id-us11-03-02) — Tune TTL, stale fallback and opt-in warming (P1, 6d)

<a id="id-f11-04"></a>
### F11.04 — Proactive quota management

- **Parent Epic:** [E11](#id-e11)
- **Priority:** P0
- **Status:** Not Started

**Description:** Reserve and schedule weighted work against minute and second limits with clear client feedback.

**Expected outcome:** Free-tier users avoid preventable 429s and multi-call tools fail before partial quota waste.

**Stories:**

- [US11.04.01](#id-us11-04-01) — Reserve and schedule Finnhub quota proactively (P0, 8d)

<a id="id-f11-05"></a>
### F11.05 — Performance SLOs and regression tests

- **Parent Epic:** [E11](#id-e11)
- **Priority:** P1
- **Status:** Not Started

**Description:** Define latency and quota-efficiency targets and continuously benchmark representative cold and warm calls.

**Expected outcome:** Performance changes are evidence-based and regressions block release.

**Stories:**

- [US11.05.01](#id-us11-05-01) — Establish latency and quota-efficiency SLOs (P1, 6d)

## 4. User Stories and Subtasks

<a id="id-us11-01-01"></a>
### US11.01.01 — Explore tools in a safe local web UI

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.01](#id-f11-01) | P2 | 8 | 56 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Prospective user

**User story:** As a prospective user, I want a simple explorer so that I can discover tools, inspect schemas and run examples without learning raw JSON-RPC first.

**Acceptance criteria:**

- [ ] The local UI lists enabled tools/resources/prompts, renders validated parameter forms and displays structured data, metadata, errors and next actions.
- [ ] Fixture mode demonstrates all twelve current tools and does not require or transmit a Finnhub key.
- [ ] Live mode sends requests only to the user's local server or a hardened BFF; no shared API key is present in browser code or storage.
- [ ] A search-ranking lab shows tokenized intent, ranked tools, relative score and matched concepts and clearly states that score is not probability.
- [ ] The UI is keyboard accessible and includes copyable CLI/MCP request examples with secrets redacted.

**Dependencies:** [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01), [US08.02.02](./E08-intelligent-discovery-and-context-engineering.md#id-us08-02-02)

**Labels:** `web-ui` `explorer` `accessibility` `P2`

**Source findings:**

- A simple Web UI would help users explore the API.
- An intent playground should show search-tools ranking and matched concepts.
- A public generic MCP Inspector proxy or browser-shared Finnhub key is unsafe; start with fixture/local mode.

**Subtasks:**

<a id="id-st11-01-01-01"></a>
- [ ] **ST11.01.01.01 — Design local explorer and fixture flows**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: UX engineer
  - Deliverable/evidence: Accessible flows for catalogue, calls and ranking lab.
  - Status: Not Started
<a id="id-st11-01-01-02"></a>
- [ ] **ST11.01.01.02 — Implement fixture/local tool explorer**
  - Type: implementation
  - Estimate: 32 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Local-first web UI for twelve tools/resources/prompts.
  - Status: Not Started
<a id="id-st11-01-01-03"></a>
- [ ] **ST11.01.01.03 — Threat-model live mode and browser storage**
  - Type: security-test
  - Estimate: 12 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: No-shared-key design and UI security tests.
  - Status: Not Started

<a id="id-us11-02-01"></a>
### US11.02.01 — Provide an interactive diagnostic CLI

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.02](#id-f11-02) | P1 | 7 | 48 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Developer

**User story:** As a developer, I want an interactive CLI so that I can configure, discover, invoke and troubleshoot the MCP server quickly.

**Acceptance criteria:**

- [ ] The CLI supports configure/status, tools search/list/describe/call, resources read, prompts render and a guided research workflow.
- [ ] Inputs are schema-validated with completion/help and secrets are accepted through standard configuration or secure prompt, never echoed or stored in shell history by default.
- [ ] Output supports human table, JSON and streaming/event views and shows cache, quota, token and correlation metadata.
- [ ] A doctor command validates SDK/server version, transport reachability, key presence and configuration without a quota-consuming quote call.
- [ ] Cross-platform integration tests cover STDIO and Streamable HTTP targets.

**Dependencies:** [US10.01.01](./E10-service-operations-resources-and-extensibility.md#id-us10-01-01), [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01)

**Labels:** `cli` `developer-experience` `P1`

**Source findings:**

- An interactive CLI would reduce onboarding and make API exploration and diagnostics easier.

**Subtasks:**

<a id="id-st11-02-01-01"></a>
- [ ] **ST11.02.01.01 — Design CLI commands and secure configuration flow**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Developer experience engineer
  - Deliverable/evidence: Command tree, output formats and secret-handling spec.
  - Status: Not Started
<a id="id-st11-02-01-02"></a>
- [ ] **ST11.02.01.02 — Implement interactive CLI and doctor**
  - Type: implementation
  - Estimate: 28 hours
  - Suggested owner role: Developer experience engineer
  - Deliverable/evidence: Cross-platform CLI for discovery, calls and diagnostics.
  - Status: Not Started
<a id="id-st11-02-01-03"></a>
- [ ] **ST11.02.01.03 — Add STDIO and HTTP CLI integration tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Transport and redaction test suite.
  - Status: Not Started

<a id="id-us11-03-01"></a>
### US11.03.01 — Canonicalize and tier cache storage

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.03](#id-f11-03) | P1 | 8 | 56 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Service operator

**User story:** As an operator, I want canonical provider-response caching with optional L2 so that views and callers share safe cached data across instances.

**Acceptance criteria:**

- [ ] Cache keys represent normalized provider inputs and credential/tenant entitlement scope, not output view flags that can be projected after retrieval.
- [ ] Per-call metadata such as query_id, request timestamp and duration is created after cache lookup and is never stored in a shared response object.
- [ ] An optional distributed L2 provider such as Redis is supported with serialization/versioning and local L1 remains available for single-instance deployments.
- [ ] Per-symbol batch calls reuse the same cache entries as single-symbol calls and tests cover tenant isolation and mixed L1/L2 hits.
- [ ] Cache responses expose hit tier, age and source fetched_at without leaking cache keys.

**Dependencies:** [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01)

**Labels:** `cache` `hybrid-cache` `performance` `P1`

**Source findings:**

- HybridCache currently has only local storage and view flags fragment cache entries.
- SearchService caches request-specific query id/timestamp/duration and can return stale metadata.
- Canonical provider responses should be cached and projected per call.

**Subtasks:**

<a id="id-st11-03-01-01"></a>
- [ ] **ST11.03.01.01 — Refactor to canonical provider cache objects**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: View-independent, request-metadata-free cache services.
  - Status: Not Started
<a id="id-st11-03-01-02"></a>
- [ ] **ST11.03.01.02 — Add optional distributed L2**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Distributed systems engineer
  - Deliverable/evidence: Versioned Redis-backed HybridCache configuration.
  - Status: Not Started
<a id="id-st11-03-01-03"></a>
- [ ] **ST11.03.01.03 — Test isolation, tiers and batch reuse**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: L1/L2, tenant and per-symbol cache suite.
  - Status: Not Started

<a id="id-us11-03-02"></a>
### US11.03.02 — Tune TTL, stale fallback and opt-in warming

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.03](#id-f11-03) | P1 | 6 | 48 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Free-tier operator

**User story:** As a free-tier operator, I want data-class-specific caching so that common workflows are fast without consuming quota broadly or disguising stale data.

**Acceptance criteria:**

- [ ] A documented TTL matrix separates quotes, intraday/history candles, profiles, statements, recommendations, news, exchanges and entitlement/negative results and adds bounded jitter.
- [ ] Historical candles no longer reuse the 10-second quote TTL; immutable closed periods receive longer safe TTLs.
- [ ] PremiumRequired and stable no-data results may be negative-cached for a short documented period, while profiles/financials/news support stale-if-error with explicit stale age/warnings.
- [ ] Cache warming is opt-in and watchlist-driven with admission against reserved quota; broad market warming is disabled on the free tier.
- [ ] Tests cover expiry boundaries, stale fallback, premium cache invalidation and warming budget exhaustion.

**Dependencies:** [US11.03.01](#id-us11-03-01), [US11.04.01](#id-us11-04-01)

**Labels:** `cache` `ttl` `warming` `P1`

**Source findings:**

- Candles currently use the quote 10-second TTL and need a history TTL.
- Add premium negative caching, stale-if-error, jitter and cache metadata.
- Do not broadly warm caches on the free tier; allow opt-in watchlist warming.

**Subtasks:**

<a id="id-st11-03-02-01"></a>
- [ ] **ST11.03.02.01 — Define data-class TTL and stale matrix**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: TTL, jitter, negative and stale-if-error policy.
  - Status: Not Started
<a id="id-st11-03-02-02"></a>
- [ ] **ST11.03.02.02 — Implement history, negative and stale caching**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Policy-driven cache behavior with freshness metadata.
  - Status: Not Started
<a id="id-st11-03-02-03"></a>
- [ ] **ST11.03.02.03 — Implement watchlist-only warming**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Quota-admitted opt-in warming scheduler.
  - Status: Not Started
<a id="id-st11-03-02-04"></a>
- [ ] **ST11.03.02.04 — Test expiry and warming exhaustion**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Time-controlled cache and quota tests.
  - Status: Not Started

<a id="id-us11-04-01"></a>
### US11.04.01 — Reserve and schedule Finnhub quota proactively

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.04](#id-f11-04) | P0 | 8 | 66 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Free-tier operator

**User story:** As a free-tier operator, I want admission control before work begins so that retries, composites and batches do not unexpectedly exhaust the provider budget.

**Acceptance criteria:**

- [ ] A proactive limiter tracks both minute and applicable second limits, defaults below the stated 60/minute ceiling such as 55 with a configurable reserve, and honors provider headers/Retry-After.
- [ ] Every tool declares estimated weighted provider-call cost; multi-call tools, workflows and batches reserve cost before execution and reconcile actual cost afterward.
- [ ] Queued work has a deadline/cancellation path and returns RateLimited with retry_after, remaining estimate and lower-cost suggestions rather than waiting indefinitely.
- [ ] Retries and all intermediate provider responses update quota state, while non-retryable 4xx responses do not consume retry loops or circuit-failure counts incorrectly.
- [ ] Multi-instance hosted mode supports an atomic distributed limiter such as Redis and load tests verify no material overshoot.

**Dependencies:** [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01)

**Labels:** `quota` `rate-limit` `resilience` `P0`

**Source findings:**

- The free tier is stated as 60 requests/minute and needs reserve-aware proactive admission, weighted tools and preflight.
- The current tracker sees only final responses; retry/breaker policy retries many non-retryable statuses and ignores Retry-After.
- A distributed limiter is needed for multiple instances; Finnhub's extra per-second constraint must also be respected.

**Subtasks:**

<a id="id-st11-04-01-01"></a>
- [ ] **ST11.04.01.01 — Design weighted dual-window quota model**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: Distributed systems engineer
  - Deliverable/evidence: Minute/second admission, reserve and reconciliation spec.
  - Status: Not Started
<a id="id-st11-04-01-02"></a>
- [ ] **ST11.04.01.02 — Implement proactive local and Redis limiters**
  - Type: implementation
  - Estimate: 30 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Atomic quota reservation and deadline-aware queue.
  - Status: Not Started
<a id="id-st11-04-01-03"></a>
- [ ] **ST11.04.01.03 — Correct retry, breaker and Retry-After handling**
  - Type: bugfix
  - Estimate: 14 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Status-aware resilience pipeline and intermediate quota tracking.
  - Status: Not Started
<a id="id-st11-04-01-04"></a>
- [ ] **ST11.04.01.04 — Run overshoot and recovery load tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: Performance engineer
  - Deliverable/evidence: Single/multi-instance quota correctness report.
  - Status: Not Started

<a id="id-us11-05-01"></a>
### US11.05.01 — Establish latency and quota-efficiency SLOs

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F11.05](#id-f11-05) | P1 | 6 | 44 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Maintainer

**User story:** As a maintainer, I want representative performance baselines so that caching and feature changes have measurable release criteria.

**Acceptance criteria:**

- [ ] The project defines p50/p95 targets for warm cache, cold single-call, multi-call and batch operations plus maximum calls per representative workflow.
- [ ] Benchmarks record serialization, cache, provider, middleware and token-estimation time separately using deterministic fixtures and an optional gated live run.
- [ ] CI compares results to a versioned baseline with explicit regression thresholds and publishes artifacts without requiring a production Finnhub key.
- [ ] A load profile verifies bounded memory, concurrency and limiter behavior; live smoke tests remain opt-in and report provider cost.
- [ ] Optimization work is prioritized by measured bottlenecks rather than indiscriminate cache warming.

**Dependencies:** [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01), [US11.03.01](#id-us11-03-01), [US11.04.01](#id-us11-04-01)

**Labels:** `performance` `slo` `benchmark` `P1`

**Source findings:**

- Response time should be measured rather than assumed; cache optimization and warming need explicit latency/quota evidence.

**Subtasks:**

<a id="id-st11-05-01-01"></a>
- [ ] **ST11.05.01.01 — Define workload SLOs and benchmark fixtures**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Performance engineer
  - Deliverable/evidence: Cold/warm/composite/batch SLO matrix.
  - Status: Not Started
<a id="id-st11-05-01-02"></a>
- [ ] **ST11.05.01.02 — Implement benchmark and load harness**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Performance engineer
  - Deliverable/evidence: Deterministic component timings and bounded-memory load profile.
  - Status: Not Started
<a id="id-st11-05-01-03"></a>
- [ ] **ST11.05.01.03 — Add CI regression reporting**
  - Type: devops
  - Estimate: 10 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Versioned thresholds and release-blocking artifacts.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST11.01.01.01](#id-st11-01-01-01) | [US11.01.01](#id-us11-01-01) | P2 | Design local explorer and fixture flows | design | 12 | UX engineer | Accessible flows for catalogue, calls and ranking lab. | Not Started |
| [ST11.01.01.02](#id-st11-01-01-02) | [US11.01.01](#id-us11-01-01) | P2 | Implement fixture/local tool explorer | implementation | 32 | Frontend engineer | Local-first web UI for twelve tools/resources/prompts. | Not Started |
| [ST11.01.01.03](#id-st11-01-01-03) | [US11.01.01](#id-us11-01-01) | P2 | Threat-model live mode and browser storage | security-test | 12 | Security engineer | No-shared-key design and UI security tests. | Not Started |
| [ST11.02.01.01](#id-st11-02-01-01) | [US11.02.01](#id-us11-02-01) | P1 | Design CLI commands and secure configuration flow | design | 10 | Developer experience engineer | Command tree, output formats and secret-handling spec. | Not Started |
| [ST11.02.01.02](#id-st11-02-01-02) | [US11.02.01](#id-us11-02-01) | P1 | Implement interactive CLI and doctor | implementation | 28 | Developer experience engineer | Cross-platform CLI for discovery, calls and diagnostics. | Not Started |
| [ST11.02.01.03](#id-st11-02-01-03) | [US11.02.01](#id-us11-02-01) | P1 | Add STDIO and HTTP CLI integration tests | test | 10 | QA automation engineer | Transport and redaction test suite. | Not Started |
| [ST11.03.01.01](#id-st11-03-01-01) | [US11.03.01](#id-us11-03-01) | P1 | Refactor to canonical provider cache objects | implementation | 24 | Backend engineer | View-independent, request-metadata-free cache services. | Not Started |
| [ST11.03.01.02](#id-st11-03-01-02) | [US11.03.01](#id-us11-03-01) | P1 | Add optional distributed L2 | implementation | 20 | Distributed systems engineer | Versioned Redis-backed HybridCache configuration. | Not Started |
| [ST11.03.01.03](#id-st11-03-01-03) | [US11.03.01](#id-us11-03-01) | P1 | Test isolation, tiers and batch reuse | test | 12 | QA automation engineer | L1/L2, tenant and per-symbol cache suite. | Not Started |
| [ST11.03.02.01](#id-st11-03-02-01) | [US11.03.02](#id-us11-03-02) | P1 | Define data-class TTL and stale matrix | design | 10 | Financial data engineer | TTL, jitter, negative and stale-if-error policy. | Not Started |
| [ST11.03.02.02](#id-st11-03-02-02) | [US11.03.02](#id-us11-03-02) | P1 | Implement history, negative and stale caching | implementation | 18 | Backend engineer | Policy-driven cache behavior with freshness metadata. | Not Started |
| [ST11.03.02.03](#id-st11-03-02-03) | [US11.03.02](#id-us11-03-02) | P1 | Implement watchlist-only warming | implementation | 12 | Backend engineer | Quota-admitted opt-in warming scheduler. | Not Started |
| [ST11.03.02.04](#id-st11-03-02-04) | [US11.03.02](#id-us11-03-02) | P1 | Test expiry and warming exhaustion | test | 8 | QA automation engineer | Time-controlled cache and quota tests. | Not Started |
| [ST11.04.01.01](#id-st11-04-01-01) | [US11.04.01](#id-us11-04-01) | P0 | Design weighted dual-window quota model | design | 12 | Distributed systems engineer | Minute/second admission, reserve and reconciliation spec. | Not Started |
| [ST11.04.01.02](#id-st11-04-01-02) | [US11.04.01](#id-us11-04-01) | P0 | Implement proactive local and Redis limiters | implementation | 30 | Backend engineer | Atomic quota reservation and deadline-aware queue. | Not Started |
| [ST11.04.01.03](#id-st11-04-01-03) | [US11.04.01](#id-us11-04-01) | P0 | Correct retry, breaker and Retry-After handling | bugfix | 14 | Backend engineer | Status-aware resilience pipeline and intermediate quota tracking. | Not Started |
| [ST11.04.01.04](#id-st11-04-01-04) | [US11.04.01](#id-us11-04-01) | P0 | Run overshoot and recovery load tests | test | 10 | Performance engineer | Single/multi-instance quota correctness report. | Not Started |
| [ST11.05.01.01](#id-st11-05-01-01) | [US11.05.01](#id-us11-05-01) | P1 | Define workload SLOs and benchmark fixtures | design | 10 | Performance engineer | Cold/warm/composite/batch SLO matrix. | Not Started |
| [ST11.05.01.02](#id-st11-05-01-02) | [US11.05.01](#id-us11-05-01) | P1 | Implement benchmark and load harness | implementation | 24 | Performance engineer | Deterministic component timings and bounded-memory load profile. | Not Started |
| [ST11.05.01.03](#id-st11-05-01-03) | [US11.05.01](#id-us11-05-01) | P1 | Add CI regression reporting | devops | 10 | DevOps engineer | Versioned thresholds and release-blocking artifacts. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E11 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| C-02 | C. User Experience | Provide a safe CLI and/or simple web explorer for tool discovery and end-user experimentation. | [US11.01.01](#id-us11-01-01), [US11.02.01](#id-us11-02-01), [US13.02.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-01) | [E11](#id-e11) | P1 | Covered | Explicit review question C2. |
| C-03 | C. User Experience | Optimize response time through corrected TTLs, canonical cache keys, optional L2 cache, stale-if-error, negative caching, jitter, and opt-in watchlist warming. | [US04.04.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-01), [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02), [US04.04.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-03), [US11.03.01](#id-us11-03-01), [US11.03.02](#id-us11-03-02) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P1 | Covered | Explicit review question C3. |
| C-04 | C. User Experience | Implement smarter free-tier quota management with proactive weighted admission, reservations, Retry-After handling, queue deadlines, and distributed coordination. | [US04.03.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-01), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02), [US04.03.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-03), [US11.04.01](#id-us11-04-01) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P0 | Covered | Explicit review question C4. |
| R-17 | Repository finding | Correct candle/history cache TTLs, cache canonical provider responses rather than view projections, and add optional distributed L2 caching. | [US04.04.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-01), [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02), [US04.04.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-03), [US11.03.01](#id-us11-03-01), [US11.03.02](#id-us11-03-02) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P1 | Covered | Code-level performance finding. |
| R-19 | Repository finding | Partition quota tracking by key/tenant and coordinate multi-instance limits rather than using one process-global tracker. | [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01), [US04.03.03](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-03), [US11.04.01](#id-us11-04-01) | [E02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-e02) | P0 | Covered | Code-level hosted-deployment finding. |
| RF-119 | Code-review detail | C2 - simple Web UI and interactive CLI for exploration | [US11.01.01](#id-us11-01-01), [US11.02.01](#id-us11-02-01) | [E11](#id-e11) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-120 | Code-review detail | C3 - response-time, HybridCache, L2, TTL, stale fallback and cache warming improvements | [US11.03.01](#id-us11-03-01), [US11.03.02](#id-us11-03-02), [US11.05.01](#id-us11-05-01) | [E11](#id-e11) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-121 | Code-review detail | C4 - smarter free-tier quota management, retry behavior and distributed limiting | [US11.04.01](#id-us11-04-01), [US09.03.01](./E09-real-time-streaming-and-batch-efficiency.md#id-us09-03-01) | [E11](#id-e11) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-126 | Code-review detail | D5 - Prometheus metrics for tool frequency, errors, cache, quota and resilience | [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01), [US11.05.01](#id-us11-05-01) | [E10](./E10-service-operations-resources-and-extensibility.md#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-132 | Code-review detail | Observed bug - request-specific search metadata is cached and duplicate symbol-resolution paths waste calls | [US11.03.01](#id-us11-03-01), [US08.05.01](./E08-intelligent-discovery-and-context-engineering.md#id-us08-05-01) | [E11](#id-e11) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-140 | Code-review detail | Website/playground safety - no browser shared key or public generic Inspector proxy; ranking lab is suitable | [US11.01.01](#id-us11-01-01), [US09.02.01](./E09-real-time-streaming-and-batch-efficiency.md#id-us09-02-01) | [E11](#id-e11) | P2 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 4. M2 — Operability & Context

- **Priority:** P1
- **Indicative window:** Weeks 12–22
- **Primary epics:** E08, E10–E12, E14, E15
- **Goal:** Improve discovery quality, observability, documentation, and delivery controls.
- **Entry criteria:** Stable tool schemas and telemetry taxonomy.
- **Exit criteria:** Golden intent evals run in CI; metrics/health are live; API docs are generated; release and supply-chain controls are enforced.
- **Delivery note:** Can proceed in parallel with later M1 stories.

### 5. M3 — Showcase & Ecosystem

- **Priority:** P1/P2
- **Indicative window:** Weeks 18–28
- **Primary epics:** E11, E13, E14
- **Goal:** Create a safe product showcase and evidence-based differentiation story.
- **Entry criteria:** Hosted demo threat model and licensing review complete.
- **Exit criteria:** Fixture playground is live; hardened demo is bounded; comparison/positioning is published; contributor path is measurable.
- **Delivery note:** Keep generic Inspector access disabled unless explicitly locked down.

### 6. M4 — Scale & Streaming

- **Priority:** P2
- **Indicative window:** Weeks 24–36
- **Primary epics:** E09–E11
- **Goal:** Add upstream WebSocket ingestion, distributed controls, and extension mechanisms.
- **Entry criteria:** Demand demonstrated; remote deployment and quota model proven.
- **Exit criteria:** Reconnect/backpressure/gap handling verified; distributed limiter/cache available; extension contract versioned.
- **Delivery note:** Do not invent a non-standard MCP WebSocket transport.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E11 | Epic | — | P1 | User Experience, Performance and Quota Control | See [E11](#id-e11) | See [E11](#id-e11) | — | finnhub-mcp; epic | — | Not Started |
| F11.01 | Feature | [E11](#id-e11) | P2 | Safe local web explorer | See [F11.01](#id-f11-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e11 | — | Not Started |
| F11.02 | Feature | [E11](#id-e11) | P1 | Interactive CLI | See [F11.02](#id-f11-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e11 | — | Not Started |
| F11.03 | Feature | [E11](#id-e11) | P1 | Cache and latency optimization | See [F11.03](#id-f11-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e11 | — | Not Started |
| F11.04 | Feature | [E11](#id-e11) | P0 | Proactive quota management | See [F11.04](#id-f11-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e11 | — | Not Started |
| F11.05 | Feature | [E11](#id-e11) | P1 | Performance SLOs and regression tests | See [F11.05](#id-f11-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e11 | — | Not Started |
| US11.01.01 | Story | [F11.01](#id-f11-01) | P2 | Explore tools in a safe local web UI | See [US11.01.01](#id-us11-01-01) | See [US11.01.01](#id-us11-01-01) | 8d | web-ui; explorer; accessibility; P2 | [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01), [US08.02.02](./E08-intelligent-discovery-and-context-engineering.md#id-us08-02-02) | Not Started |
| US11.02.01 | Story | [F11.02](#id-f11-02) | P1 | Provide an interactive diagnostic CLI | See [US11.02.01](#id-us11-02-01) | See [US11.02.01](#id-us11-02-01) | 7d | cli; developer-experience; P1 | [US10.01.01](./E10-service-operations-resources-and-extensibility.md#id-us10-01-01), [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01) | Not Started |
| US11.03.01 | Story | [F11.03](#id-f11-03) | P1 | Canonicalize and tier cache storage | See [US11.03.01](#id-us11-03-01) | See [US11.03.01](#id-us11-03-01) | 8d | cache; hybrid-cache; performance; P1 | [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01) | Not Started |
| US11.03.02 | Story | [F11.03](#id-f11-03) | P1 | Tune TTL, stale fallback and opt-in warming | See [US11.03.02](#id-us11-03-02) | See [US11.03.02](#id-us11-03-02) | 6d | cache; ttl; warming; P1 | [US11.03.01](#id-us11-03-01), [US11.04.01](#id-us11-04-01) | Not Started |
| US11.04.01 | Story | [F11.04](#id-f11-04) | P0 | Reserve and schedule Finnhub quota proactively | See [US11.04.01](#id-us11-04-01) | See [US11.04.01](#id-us11-04-01) | 8d | quota; rate-limit; resilience; P0 | [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01) | Not Started |
| US11.05.01 | Story | [F11.05](#id-f11-05) | P1 | Establish latency and quota-efficiency SLOs | See [US11.05.01](#id-us11-05-01) | See [US11.05.01](#id-us11-05-01) | 6d | performance; slo; benchmark; P1 | [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01), [US11.03.01](#id-us11-03-01), [US11.04.01](#id-us11-04-01) | Not Started |
| ST11.01.01.01 | Sub-task | [US11.01.01](#id-us11-01-01) | P2 | Design local explorer and fixture flows | See [ST11.01.01.01](#id-st11-01-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST11.01.01.02 | Sub-task | [US11.01.01](#id-us11-01-01) | P2 | Implement fixture/local tool explorer | See [ST11.01.01.02](#id-st11-01-01-02) | Not applicable; see detail or parent section | 32h | finnhub-mcp; implementation | — | Not Started |
| ST11.01.01.03 | Sub-task | [US11.01.01](#id-us11-01-01) | P2 | Threat-model live mode and browser storage | See [ST11.01.01.03](#id-st11-01-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; security-test | — | Not Started |
| ST11.02.01.01 | Sub-task | [US11.02.01](#id-us11-02-01) | P1 | Design CLI commands and secure configuration flow | See [ST11.02.01.01](#id-st11-02-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST11.02.01.02 | Sub-task | [US11.02.01](#id-us11-02-01) | P1 | Implement interactive CLI and doctor | See [ST11.02.01.02](#id-st11-02-01-02) | Not applicable; see detail or parent section | 28h | finnhub-mcp; implementation | — | Not Started |
| ST11.02.01.03 | Sub-task | [US11.02.01](#id-us11-02-01) | P1 | Add STDIO and HTTP CLI integration tests | See [ST11.02.01.03](#id-st11-02-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST11.03.01.01 | Sub-task | [US11.03.01](#id-us11-03-01) | P1 | Refactor to canonical provider cache objects | See [ST11.03.01.01](#id-st11-03-01-01) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST11.03.01.02 | Sub-task | [US11.03.01](#id-us11-03-01) | P1 | Add optional distributed L2 | See [ST11.03.01.02](#id-st11-03-01-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST11.03.01.03 | Sub-task | [US11.03.01](#id-us11-03-01) | P1 | Test isolation, tiers and batch reuse | See [ST11.03.01.03](#id-st11-03-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST11.03.02.01 | Sub-task | [US11.03.02](#id-us11-03-02) | P1 | Define data-class TTL and stale matrix | See [ST11.03.02.01](#id-st11-03-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST11.03.02.02 | Sub-task | [US11.03.02](#id-us11-03-02) | P1 | Implement history, negative and stale caching | See [ST11.03.02.02](#id-st11-03-02-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST11.03.02.03 | Sub-task | [US11.03.02](#id-us11-03-02) | P1 | Implement watchlist-only warming | See [ST11.03.02.03](#id-st11-03-02-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST11.03.02.04 | Sub-task | [US11.03.02](#id-us11-03-02) | P1 | Test expiry and warming exhaustion | See [ST11.03.02.04](#id-st11-03-02-04) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST11.04.01.01 | Sub-task | [US11.04.01](#id-us11-04-01) | P0 | Design weighted dual-window quota model | See [ST11.04.01.01](#id-st11-04-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST11.04.01.02 | Sub-task | [US11.04.01](#id-us11-04-01) | P0 | Implement proactive local and Redis limiters | See [ST11.04.01.02](#id-st11-04-01-02) | Not applicable; see detail or parent section | 30h | finnhub-mcp; implementation | — | Not Started |
| ST11.04.01.03 | Sub-task | [US11.04.01](#id-us11-04-01) | P0 | Correct retry, breaker and Retry-After handling | See [ST11.04.01.03](#id-st11-04-01-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; bugfix | — | Not Started |
| ST11.04.01.04 | Sub-task | [US11.04.01](#id-us11-04-01) | P0 | Run overshoot and recovery load tests | See [ST11.04.01.04](#id-st11-04-01-04) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST11.05.01.01 | Sub-task | [US11.05.01](#id-us11-05-01) | P1 | Define workload SLOs and benchmark fixtures | See [ST11.05.01.01](#id-st11-05-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST11.05.01.02 | Sub-task | [US11.05.01](#id-us11-05-01) | P1 | Implement benchmark and load harness | See [ST11.05.01.02](#id-st11-05-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST11.05.01.03 | Sub-task | [US11.05.01](#id-us11-05-01) | P1 | Add CI regression reporting | See [ST11.05.01.03](#id-st11-05-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; devops | — | Not Started |

## 9. Delivery Guide

Definitions, governance rules, and quality gates for turning the backlog into releases.

| Area | Rule | Definition / Gate |
| --- | --- | --- |
| Hierarchy | Epic | A measurable product or platform outcome with exit criteria. |
| Hierarchy | Feature | A coherent capability slice that can be released or feature-flagged. |
| Hierarchy | User Story | A user- or operator-valued behavior with testable acceptance criteria. |
| Hierarchy | Subtask | An implementation action that produces code, tests, docs, design, or operational evidence. |
| Priority | P0 | Required for availability, security, protocol correctness, or trustworthy core financial output. |
| Priority | P1 | Material improvement to usefulness, developer productivity, or operating quality. |
| Priority | P2 | Longer-term optimization, ecosystem scale, localization, or optional breadth. |
| Estimation | Story days | Ideal engineering working days, including implementation and unit tests; excludes queue time and external approvals. |
| Estimation | Subtask hours | Implementation-level estimate used for sprint planning; re-estimate after technical design. |
| Definition of Ready | Scope | Persona, desired outcome, acceptance criteria, dependencies, quota/licensing impact, and rollout mode are understood. |
| Definition of Ready | Security | Trust boundary, sensitive inputs, tenant/cache partitioning, and logging requirements are identified. |
| Definition of Done | Quality | Code reviewed; unit, integration, and protocol tests pass; failure paths and cancellation are covered. |
| Definition of Done | Contract | Tool/resource/prompt schema, examples, error semantics, token/output bounds, and compatibility notes are updated. |
| Definition of Done | Operations | Metrics, structured logs, runbook, feature flag/rollback, cache/quota behavior, and alert thresholds are defined where relevant. |
| Definition of Done | Security | No secrets or raw sensitive payloads are logged; abuse limits and authorization tests pass for hosted surfaces. |
| Release gate | P0 | No open P0 defects; real client E2E green for supported transports; security regression suite green; docs match runtime routes. |
| Release gate | Financial accuracy | Metric definitions, units, periods, provenance, edge cases, and reference fixtures are independently reviewed. |
| Release gate | Context quality | Golden intent corpus meets agreed tool@1, Recall@3, invalid-argument, quota, latency, and token thresholds. |
| Operating rule | Hosted demo | Never expose a shared Finnhub key to browsers. Use an authenticated bounded BFF, strict allowlists, and per-user quotas. |
| Operating rule | Streaming | Use standard MCP Streamable HTTP. WebSocket is an upstream Finnhub ingestion mechanism or a separately secured UI channel. |
| Operating rule | Feedback | Collect opt-in, privacy-safe ranking feedback for offline evaluation; do not self-train the live BM25 ranker online. |

## 10. Epic Completion Checklist

- [ ] Create E11 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E11.
- [ ] Create all 6 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 20 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 11 relevant traceability rows covered.
- [ ] Satisfy all 3 relevant roadmap milestone gates.
- [ ] Reconcile all 32 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

