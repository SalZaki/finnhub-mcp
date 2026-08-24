---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E04
title: "Resilience, Quota Governance, and Cache Correctness"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 4
  user_stories: 10
  subtasks: 33
  traceability_owned: 14
  traceability_items: 19
story_estimate_days: 36.5
subtask_estimate_hours: 231
---

<a id="id-e04"></a>
# E04 — Resilience, Quota Governance, and Cache Correctness

This is the self-contained coding-agent backlog for E04. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E04 | P0 | 4 | 10 | 33 | 36.5 | 231 | M0 — Hardened Core | Not Started |

> [!WARNING]
> This Epic participates in the pre-existing circular blocker [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01) → [US04.04.02](#id-us04-04-02) → [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01). The backlog owner must decide which edge is a true blocker, reclassify one edge as coordination, or introduce a shared foundation Story before automated sequencing.

## 2. Epic Definition

**Objective:** Make upstream failures predictable, honor Finnhub limits proactively, and improve latency without serving incorrectly scoped or misleading cached data.

**Business value:** Protects availability and the shared API budget, gives agents actionable failures, and improves response time under free-tier constraints.

**Exit criteria:**

- [ ] Every failure maps to a stable error category with retryability and remediation metadata.
- [ ] Retries, timeouts, and circuit breaking operate on an explicit HTTP/status exception matrix and honor Retry-After.
- [ ] Weighted reservations prevent multi-call tools from overspending quota and work across instances when configured.
- [ ] Cache keys, TTLs, stale behavior, and negative caching are documented, observable, and covered by tests.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F04.01](#id-f04-01) | P0 | Stable Failure Contract | 2 | 5.5 | Not Started |
| [F04.02](#id-f04-02) | P0 | Correct Retry, Timeout, and Circuit Policies | 2 | 6 | Not Started |
| [F04.03](#id-f04-03) | P0 | Proactive Finnhub Quota Governance | 3 | 12 | Not Started |
| [F04.04](#id-f04-04) | P1 | Tiered and Semantically Correct Caching | 3 | 13 | Not Started |

<a id="id-f04-01"></a>
### F04.01 — Stable Failure Contract

- **Parent Epic:** [E04](#id-e04)
- **Priority:** P0
- **Status:** Not Started

**Description:** Expand and standardize error categories, status mapping, retryability, and partial-success reporting.

**Expected outcome:** Agents receive machine-readable failures with a concrete safe next step.

**Stories:**

- [US04.01.01](#id-us04-01-01) — Expand and standardize the domain error taxonomy (P0, 2.5d)
- [US04.01.02](#id-us04-01-02) — Represent partial multi-source results explicitly (P1, 3d)

<a id="id-f04-02"></a>
### F04.02 — Correct Retry, Timeout, and Circuit Policies

- **Parent Epic:** [E04](#id-e04)
- **Priority:** P0
- **Status:** Not Started

**Description:** Retry only transient failures, honor server guidance, and count failures at the intended logical-call boundary.

**Expected outcome:** Resilience policies improve availability without multiplying bad requests or prematurely opening circuits.

**Stories:**

- [US04.02.01](#id-us04-02-01) — Retry only classified transient failures (P0, 3d)
- [US04.02.02](#id-us04-02-02) — Align circuit breaking with logical upstream calls (P0, 3d)

<a id="id-f04-03"></a>
### F04.03 — Proactive Finnhub Quota Governance

- **Parent Epic:** [E04](#id-e04)
- **Priority:** P0
- **Status:** Not Started

**Description:** Reserve weighted request budgets, observe every upstream attempt, and coordinate rate limits locally or across instances.

**Expected outcome:** The service stays below configured minute/second limits and preserves emergency capacity for high-value calls.

**Stories:**

- [US04.03.01](#id-us04-03-01) — Observe quota headers and every upstream attempt (P0, 3d)
- [US04.03.02](#id-us04-03-02) — Reserve weighted quota before multi-call tools execute (P0, 4d)
- [US04.03.03](#id-us04-03-03) — Coordinate quotas across instances with bounded waiting (P1, 5d)

<a id="id-f04-04"></a>
### F04.04 — Tiered and Semantically Correct Caching

- **Parent Epic:** [E04](#id-e04)
- **Priority:** P1
- **Status:** Not Started

**Description:** Cache canonical provider data with endpoint-specific TTLs, tenant/key partitioning, optional L2, and safe degraded reads.

**Expected outcome:** Caching reduces latency and quota cost without stale request metadata, view fragmentation, or cross-scope leakage.

**Stories:**

- [US04.04.01](#id-us04-04-01) — Cache canonical provider responses and project views afterward (P1, 4d)
- [US04.04.02](#id-us04-04-02) — Add endpoint-specific TTLs and optional distributed L2 (P1, 5d)
- [US04.04.03](#id-us04-04-03) — Support safe stale fallback, negative caching, and opt-in warming (P1, 4d)

## 4. User Stories and Subtasks

<a id="id-us04-01-01"></a>
### US04.01.01 — Expand and standardize the domain error taxonomy

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.01](#id-f04-01) | P0 | 2.5 | 17 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI agent developer

**User story:** As an AI agent developer, I want stable error codes with retry guidance so that orchestration can recover safely without parsing prose.

**Acceptance criteria:**

- [ ] The taxonomy includes RateLimited, Unauthenticated, PermissionDenied, ConfigurationError, Cancelled, CircuitOpen, and PartialSuccess in addition to existing categories.
- [ ] Each code defines retryable, recommended action, HTTP mapping where applicable, and whether upstream quota was consumed.
- [ ] Errors include a trace id and optional retry_after_seconds but exclude stack traces, secrets, raw provider bodies, and unbounded input echoes.
- [ ] PremiumRequired, BudgetExceeded, NotFound, and InvalidQuery remain distinguishable and have specific remediation text.
- [ ] A contract test snapshots code names and compatibility rules.

**Dependencies:** —

**Labels:** `errors` `api-contract` `agent-ux`

**Source findings:**

- Existing types cover eight cases but omit authentication, authorization, rate limiting, configuration, cancellation, open circuit, and partial success.

**Subtasks:**

<a id="id-st04-01-01-01"></a>
- [ ] **ST04.01.01.01 — Specify error codes, retryability, and remediation matrix**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Versioned error contract
  - Status: Not Started
<a id="id-st04-01-01-02"></a>
- [ ] **ST04.01.01.02 — Implement shared error factory and mappings**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Central error mapping library
  - Status: Not Started
<a id="id-st04-01-01-03"></a>
- [ ] **ST04.01.01.03 — Add compatibility snapshots and sanitization tests**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Error contract tests
  - Status: Not Started

<a id="id-us04-01-02"></a>
### US04.01.02 — Represent partial multi-source results explicitly

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.01](#id-f04-01) | P1 | 3 | 21 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** financial researcher

**User story:** As a financial researcher, I want partial responses to identify missing components so that I do not mistake degraded output for complete analysis.

**Acceptance criteria:**

- [ ] Multi-call tools return completeness status, successful components, failed components, and per-component error codes.
- [ ] A partial response is neither a silent success nor a total failure when useful verified data remains.
- [ ] The response declares upstream_calls, quota_cost, cache status, and as-of time for each component.
- [ ] Retry recommendations target only failed safe-to-retry components.
- [ ] Tests cover one optional failure, one mandatory failure, and multiple mixed failures.

**Dependencies:** [US04.01.01](#id-us04-01-01), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02)

**Labels:** `errors` `partial-success` `provenance`

**Source findings:**

- Multi-source tools can fail or degrade inconsistently and do not provide a general partial-success contract.

**Subtasks:**

<a id="id-st04-01-02-01"></a>
- [ ] **ST04.01.02.01 — Define component completeness/provenance DTOs**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Partial-result schema
  - Status: Not Started
<a id="id-st04-01-02-02"></a>
- [ ] **ST04.01.02.02 — Apply partial-result aggregation to multi-call tools**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Shared multi-source aggregator
  - Status: Not Started
<a id="id-st04-01-02-03"></a>
- [ ] **ST04.01.02.03 — Test mandatory and optional component failures**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Partial-success test matrix
  - Status: Not Started

<a id="id-us04-02-01"></a>
### US04.02.01 — Retry only classified transient failures

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.02](#id-f04-02) | P0 | 3 | 17 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** site reliability engineer

**User story:** As an SRE, I want an explicit retry matrix so that malformed, unauthorized, missing, or premium requests are not multiplied against Finnhub.

**Acceptance criteria:**

- [ ] 400, 401, 403, 404, semantic provider errors, and PremiumRequired are not retried unless an endpoint-specific rule proves they are transient.
- [ ] 408, 429, selected 5xx responses, connection reset, and transient DNS/network errors use bounded retries with decorrelated jitter.
- [ ] 429 and 503 honor a valid Retry-After value subject to a configured maximum deadline.
- [ ] Retries preserve cancellation and never reuse an already-sent HttpRequestMessage.
- [ ] Parameterized tests cover every status/exception class and assert exact attempt counts.

**Dependencies:** [US04.01.01](#id-us04-01-01)

**Labels:** `resilience` `retry` `http` `quota`

**Source findings:**

- The current policy retries nearly all non-success responses except 401/403, including bad requests and not-found responses, and does not honor Retry-After.

**Subtasks:**

<a id="id-st04-02-01-01"></a>
- [ ] **ST04.02.01.01 — Document endpoint status/exception retry matrix**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Retry classification table
  - Status: Not Started
<a id="id-st04-02-01-02"></a>
- [ ] **ST04.02.01.02 — Implement bounded jitter and Retry-After handling**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Classified retry policy
  - Status: Not Started
<a id="id-st04-02-01-03"></a>
- [ ] **ST04.02.01.03 — Create parameterized attempt-count tests**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Retry behavior test suite
  - Status: Not Started

<a id="id-us04-02-02"></a>
### US04.02.02 — Align circuit breaking with logical upstream calls

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.02](#id-f04-02) | P0 | 3 | 18 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** site reliability engineer

**User story:** As an SRE, I want circuit accounting and policy order to reflect logical calls so that one retried operation does not distort service health or keep hammering an open dependency.

**Acceptance criteria:**

- [ ] The policy order and rationale for timeout, quota wait, retry, and circuit breaker are documented and unit-tested.
- [ ] Non-transient 4xx, authentication, PremiumRequired, and caller cancellation do not contribute to circuit failure rate.
- [ ] A logical call's retry attempts are observable, while circuit sampling uses the explicitly chosen attempt- or operation-level semantics.
- [ ] Open-circuit calls fail immediately as CircuitOpen with retry timing and perform no upstream request.
- [ ] Half-open probes are bounded so concurrent callers cannot create a recovery stampede.

**Dependencies:** [US04.02.01](#id-us04-02-01)

**Labels:** `resilience` `circuit-breaker` `polly`

**Source findings:**

- Retry currently wraps the breaker so one logical request can register multiple breaker faults; the breaker also handles the same overly broad non-success set.

**Subtasks:**

<a id="id-st04-02-02-01"></a>
- [ ] **ST04.02.02.01 — Model and document resilience policy order**
  - Type: design
  - Estimate: 4 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Resilience policy ADR
  - Status: Not Started
<a id="id-st04-02-02-02"></a>
- [ ] **ST04.02.02.02 — Reconfigure circuit predicates and half-open probes**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Logical-call circuit policy
  - Status: Not Started
<a id="id-st04-02-02-03"></a>
- [ ] **ST04.02.02.03 — Test retry/circuit interaction and recovery stampedes**
  - Type: reliability-testing
  - Estimate: 7 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Circuit interaction tests
  - Status: Not Started

<a id="id-us04-03-01"></a>
### US04.03.01 — Observe quota headers and every upstream attempt

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.03](#id-f04-03) | P0 | 3 | 18 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want quota accounting around every Finnhub attempt so that retries and intermediate throttles are not invisible.

**Acceptance criteria:**

- [ ] Every physical upstream attempt records endpoint cost, status, latency, and applicable Finnhub rate headers before response disposal or retry.
- [ ] Intermediate 429 responses update the limiter and are visible even when a later retry succeeds.
- [ ] Quota telemetry excludes API keys and uses only opaque key/tenant identifiers.
- [ ] The api-status resource reports observed values, configured limits, confidence/source, and age rather than implying unavailable precision.
- [ ] Tests simulate headers across retries and assert the final ledger includes all attempts.

**Dependencies:** [US04.02.01](#id-us04-02-01)

**Labels:** `quota` `observability` `rate-limit` `retry`

**Source findings:**

- The current outer rate tracker observes only the final response, so intermediate 429s and retry cost are hidden.

**Subtasks:**

<a id="id-st04-03-01-01"></a>
- [ ] **ST04.03.01.01 — Move quota observation inside physical-attempt boundary**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Per-attempt quota observer
  - Status: Not Started
<a id="id-st04-03-01-02"></a>
- [ ] **ST04.03.01.02 — Expose honest quota status and age metadata**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Revised api-status resource
  - Status: Not Started
<a id="id-st04-03-01-03"></a>
- [ ] **ST04.03.01.03 — Test headers and 429 accounting through retries**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Quota observation tests
  - Status: Not Started

<a id="id-us04-03-02"></a>
### US04.03.02 — Reserve weighted quota before multi-call tools execute

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.03](#id-f04-03) | P0 | 4 | 31 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI agent

**User story:** As an AI agent, I want tools to preflight their expected Finnhub cost so that a research workflow does not start work it cannot finish within the remaining budget.

**Acceptance criteria:**

- [ ] Each tool declares minimum, typical, and maximum physical upstream cost, including optional and retry attempts.
- [ ] The limiter atomically reserves expected mandatory cost before execution and releases unused capacity after completion.
- [ ] The default free-tier policy is configurable around a conservative minute budget such as 55 of 60 calls, with an emergency reserve such as 5 calls.
- [ ] A separate configurable burst/second constraint is enforced in addition to the minute window.
- [ ] Insufficient budget returns BudgetExceeded before any upstream work and includes reset timing and lower-cost alternatives.

**Dependencies:** [US04.01.01](#id-us04-01-01), [US04.03.01](#id-us04-03-01)

**Labels:** `quota` `budget` `multi-call` `free-tier`

**Source findings:**

- Budget checks can occur after work and do not account intelligently for multi-call tools; the free tier needs proactive headroom and reserved capacity.

**Subtasks:**

<a id="id-st04-03-02-01"></a>
- [ ] **ST04.03.02.01 — Catalogue physical call costs for every tool**
  - Type: analysis
  - Estimate: 6 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Tool quota-cost catalogue
  - Status: Not Started
<a id="id-st04-03-02-02"></a>
- [ ] **ST04.03.02.02 — Implement atomic reservation, release, and emergency reserve**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Weighted quota budgeter
  - Status: Not Started
<a id="id-st04-03-02-03"></a>
- [ ] **ST04.03.02.03 — Integrate preflight into multi-call tools**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Preflight-enabled tools
  - Status: Not Started
<a id="id-st04-03-02-04"></a>
- [ ] **ST04.03.02.04 — Test minute/burst/reset and zero-work denial cases**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Quota budget test suite
  - Status: Not Started

<a id="id-us04-03-03"></a>
### US04.03.03 — Coordinate quotas across instances with bounded waiting

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.03](#id-f04-03) | P1 | 5 | 28 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** platform engineer

**User story:** As a platform engineer, I want an optional distributed quota backend so that horizontally scaled servers sharing a Finnhub key respect one provider budget.

**Acceptance criteria:**

- [ ] A limiter interface supports in-memory single-instance and Redis-backed atomic reservation implementations.
- [ ] Distributed keys include credential fingerprint and tenant policy scope without including the secret.
- [ ] Queued work has a maximum deadline and cancellation; expired work returns RateLimited or BudgetExceeded without an upstream call.
- [ ] Redis outage behavior is explicitly configurable as fail-closed for shared production keys and safe local fallback for development.
- [ ] Concurrency tests across multiple host instances prove the configured aggregate minute and burst limits are not exceeded.

**Dependencies:** [US04.03.02](#id-us04-03-02), [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01)

**Labels:** `quota` `redis` `distributed` `scaling`

**Source findings:**

- The current tracker is process-local and cannot coordinate multiple instances sharing one Finnhub quota.

**Subtasks:**

<a id="id-st04-03-03-01"></a>
- [ ] **ST04.03.03.01 — Define limiter storage interface and failure modes**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: platform architect
  - Deliverable/evidence: Distributed limiter contract
  - Status: Not Started
<a id="id-st04-03-03-02"></a>
- [ ] **ST04.03.03.02 — Implement Redis atomic reservation backend**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Redis quota backend
  - Status: Not Started
<a id="id-st04-03-03-03"></a>
- [ ] **ST04.03.03.03 — Run multi-instance aggregate-limit tests**
  - Type: performance-testing
  - Estimate: 9 hours
  - Suggested owner role: performance engineer
  - Deliverable/evidence: Distributed limiter verification
  - Status: Not Started

<a id="id-us04-04-01"></a>
### US04.04.01 — Cache canonical provider responses and project views afterward

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.04](#id-f04-04) | P1 | 4 | 25 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** performance engineer

**User story:** As a performance engineer, I want one canonical cache entry per provider query so that summary/standard/full variants reuse upstream data rather than fragmenting cache and quota.

**Acceptance criteria:**

- [ ] Cache factories store normalized provider-domain data without per-request query ids, duration, generated timestamps, view flags, or token estimates.
- [ ] View, fields, limit, output metadata, and request identifiers are applied after a cache hit on each call.
- [ ] Semantically equivalent normalized inputs produce the same provider cache key.
- [ ] Cache responses expose fetched_at, age, hit/miss/stale state, and source endpoint without exposing internal keys.
- [ ] Tests prove multiple views and limits share one upstream fetch but return distinct correct projections and fresh per-call metadata.

**Dependencies:** [US05.01.02](./E05-financial-and-symbol-data-correctness.md#id-us05-01-02)

**Labels:** `cache` `projection` `performance` `metadata`

**Source findings:**

- View flags can fragment caches, and search caches full responses containing per-request QueryId, timestamp, and duration that become stale on a hit.

**Subtasks:**

<a id="id-st04-04-01-01"></a>
- [ ] **ST04.04.01.01 — Split canonical provider data from response metadata**
  - Type: refactoring
  - Estimate: 10 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Canonical cache DTO layer
  - Status: Not Started
<a id="id-st04-04-01-02"></a>
- [ ] **ST04.04.01.02 — Normalize cache keys and post-cache projections**
  - Type: implementation
  - Estimate: 9 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Projection-aware cache pipeline
  - Status: Not Started
<a id="id-st04-04-01-03"></a>
- [ ] **ST04.04.01.03 — Test cross-view reuse and fresh metadata**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Cache projection tests
  - Status: Not Started

<a id="id-us04-04-02"></a>
### US04.04.02 — Add endpoint-specific TTLs and optional distributed L2

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.04](#id-f04-04) | P1 | 5 | 29 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want cache policy matched to data volatility and deployment topology so that history, quotes, profiles, and reference data are neither over-fetched nor served under the wrong freshness promise.

**Acceptance criteria:**

- [ ] TTL policy distinguishes real-time quote, historical candle, company/profile, news, financial, calendar, and reference-data classes.
- [ ] Historical candles do not reuse the 10-second quote TTL unless the requested interval includes a still-open bar with an explicit policy.
- [ ] TTL values include bounded jitter to avoid synchronized expiry stampedes.
- [ ] HybridCache supports an optional Redis/distributed L2 while retaining an in-memory local profile.
- [ ] Cache-key versioning and deployment documentation cover schema changes and tenant/credential partitioning.

**Dependencies:** [US03.02.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-02-01), [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01)

**Labels:** `cache` `hybrid-cache` `redis` `ttl`

**Source findings:**

- HybridCache is configured only locally, and historical candles use the Quote 10-second cache category even though their freshness characteristics differ.

**Subtasks:**

<a id="id-st04-04-02-01"></a>
- [ ] **ST04.04.02.01 — Define per-data-class TTL and jitter policy**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: data platform engineer
  - Deliverable/evidence: Cache freshness matrix
  - Status: Not Started
<a id="id-st04-04-02-02"></a>
- [ ] **ST04.04.02.02 — Implement history TTL and cache-key versioning**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Correct endpoint cache policies
  - Status: Not Started
<a id="id-st04-04-02-03"></a>
- [ ] **ST04.04.02.03 — Configure optional Redis L2 HybridCache storage**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Tiered L1/L2 cache
  - Status: Not Started
<a id="id-st04-04-02-04"></a>
- [ ] **ST04.04.02.04 — Test TTL classes, jitter, and partition keys**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Cache policy tests
  - Status: Not Started

<a id="id-us04-04-03"></a>
### US04.04.03 — Support safe stale fallback, negative caching, and opt-in warming

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F04.04](#id-f04-04) | P1 | 4 | 27 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** financial researcher

**User story:** As a financial researcher, I want bounded degraded cache behavior so that transient outages remain usable without concealing stale or unavailable premium data.

**Acceptance criteria:**

- [ ] Eligible stable datasets such as profiles, financials, and recent news can use stale-if-error within a documented maximum staleness window.
- [ ] Stale responses are marked with fetched_at, age, stale reason, and completeness; quotes are never silently stale under a real-time label.
- [ ] PremiumRequired and stable negative capability results may be cached for a short configurable 5-15 minute range, partitioned by credential version.
- [ ] Cache warming is disabled globally by default and can target only an explicit tenant watchlist within a reserved quota budget.
- [ ] Tests cover stale success, stale expiry, negative-cache entitlement change, and warming budget exhaustion.

**Dependencies:** [US04.04.02](#id-us04-04-02), [US04.03.02](#id-us04-03-02)

**Labels:** `cache` `stale-if-error` `negative-cache` `warming`

**Source findings:**

- Safe stale-if-error and short premium negative caching can improve availability; broad cache warming would waste free-tier quota and should be opt-in by watchlist.

**Subtasks:**

<a id="id-st04-04-03-01"></a>
- [ ] **ST04.04.03.01 — Define eligible stale and negative-cache policies**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: data platform engineer
  - Deliverable/evidence: Degraded cache policy
  - Status: Not Started
<a id="id-st04-04-03-02"></a>
- [ ] **ST04.04.03.02 — Implement stale metadata and premium negative caching**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Safe degraded-read pipeline
  - Status: Not Started
<a id="id-st04-04-03-03"></a>
- [ ] **ST04.04.03.03 — Implement watchlist-only warming with quota reservation**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: platform engineer
  - Deliverable/evidence: Opt-in cache warmer
  - Status: Not Started
<a id="id-st04-04-03-04"></a>
- [ ] **ST04.04.03.04 — Test staleness, entitlement changes, and warming limits**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Degraded cache behavior tests
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST04.01.01.01](#id-st04-01-01-01) | [US04.01.01](#id-us04-01-01) | P0 | Specify error codes, retryability, and remediation matrix | design | 5 | API architect | Versioned error contract | Not Started |
| [ST04.01.01.02](#id-st04-01-01-02) | [US04.01.01](#id-us04-01-01) | P0 | Implement shared error factory and mappings | implementation | 7 | backend engineer | Central error mapping library | Not Started |
| [ST04.01.01.03](#id-st04-01-01-03) | [US04.01.01](#id-us04-01-01) | P0 | Add compatibility snapshots and sanitization tests | testing | 5 | test engineer | Error contract tests | Not Started |
| [ST04.01.02.01](#id-st04-01-02-01) | [US04.01.02](#id-us04-01-02) | P1 | Define component completeness/provenance DTOs | design | 5 | API architect | Partial-result schema | Not Started |
| [ST04.01.02.02](#id-st04-01-02-02) | [US04.01.02](#id-us04-01-02) | P1 | Apply partial-result aggregation to multi-call tools | implementation | 10 | backend engineer | Shared multi-source aggregator | Not Started |
| [ST04.01.02.03](#id-st04-01-02-03) | [US04.01.02](#id-us04-01-02) | P1 | Test mandatory and optional component failures | testing | 6 | test engineer | Partial-success test matrix | Not Started |
| [ST04.02.01.01](#id-st04-02-01-01) | [US04.02.01](#id-us04-02-01) | P0 | Document endpoint status/exception retry matrix | design | 4 | SRE | Retry classification table | Not Started |
| [ST04.02.01.02](#id-st04-02-01-02) | [US04.02.01](#id-us04-02-01) | P0 | Implement bounded jitter and Retry-After handling | implementation | 7 | backend engineer | Classified retry policy | Not Started |
| [ST04.02.01.03](#id-st04-02-01-03) | [US04.02.01](#id-us04-02-01) | P0 | Create parameterized attempt-count tests | testing | 6 | test engineer | Retry behavior test suite | Not Started |
| [ST04.02.02.01](#id-st04-02-02-01) | [US04.02.02](#id-us04-02-02) | P0 | Model and document resilience policy order | design | 4 | SRE | Resilience policy ADR | Not Started |
| [ST04.02.02.02](#id-st04-02-02-02) | [US04.02.02](#id-us04-02-02) | P0 | Reconfigure circuit predicates and half-open probes | implementation | 7 | backend engineer | Logical-call circuit policy | Not Started |
| [ST04.02.02.03](#id-st04-02-02-03) | [US04.02.02](#id-us04-02-02) | P0 | Test retry/circuit interaction and recovery stampedes | reliability-testing | 7 | SRE | Circuit interaction tests | Not Started |
| [ST04.03.01.01](#id-st04-03-01-01) | [US04.03.01](#id-us04-03-01) | P0 | Move quota observation inside physical-attempt boundary | implementation | 7 | backend engineer | Per-attempt quota observer | Not Started |
| [ST04.03.01.02](#id-st04-03-01-02) | [US04.03.01](#id-us04-03-01) | P0 | Expose honest quota status and age metadata | implementation | 5 | MCP engineer | Revised api-status resource | Not Started |
| [ST04.03.01.03](#id-st04-03-01-03) | [US04.03.01](#id-us04-03-01) | P0 | Test headers and 429 accounting through retries | testing | 6 | test engineer | Quota observation tests | Not Started |
| [ST04.03.02.01](#id-st04-03-02-01) | [US04.03.02](#id-us04-03-02) | P0 | Catalogue physical call costs for every tool | analysis | 6 | backend engineer | Tool quota-cost catalogue | Not Started |
| [ST04.03.02.02](#id-st04-03-02-02) | [US04.03.02](#id-us04-03-02) | P0 | Implement atomic reservation, release, and emergency reserve | implementation | 10 | platform engineer | Weighted quota budgeter | Not Started |
| [ST04.03.02.03](#id-st04-03-02-03) | [US04.03.02](#id-us04-03-02) | P0 | Integrate preflight into multi-call tools | implementation | 8 | backend engineer | Preflight-enabled tools | Not Started |
| [ST04.03.02.04](#id-st04-03-02-04) | [US04.03.02](#id-us04-03-02) | P0 | Test minute/burst/reset and zero-work denial cases | testing | 7 | test engineer | Quota budget test suite | Not Started |
| [ST04.03.03.01](#id-st04-03-03-01) | [US04.03.03](#id-us04-03-03) | P1 | Define limiter storage interface and failure modes | design | 5 | platform architect | Distributed limiter contract | Not Started |
| [ST04.03.03.02](#id-st04-03-03-02) | [US04.03.03](#id-us04-03-03) | P1 | Implement Redis atomic reservation backend | implementation | 14 | platform engineer | Redis quota backend | Not Started |
| [ST04.03.03.03](#id-st04-03-03-03) | [US04.03.03](#id-us04-03-03) | P1 | Run multi-instance aggregate-limit tests | performance-testing | 9 | performance engineer | Distributed limiter verification | Not Started |
| [ST04.04.01.01](#id-st04-04-01-01) | [US04.04.01](#id-us04-04-01) | P1 | Split canonical provider data from response metadata | refactoring | 10 | backend engineer | Canonical cache DTO layer | Not Started |
| [ST04.04.01.02](#id-st04-04-01-02) | [US04.04.01](#id-us04-04-01) | P1 | Normalize cache keys and post-cache projections | implementation | 9 | backend engineer | Projection-aware cache pipeline | Not Started |
| [ST04.04.01.03](#id-st04-04-01-03) | [US04.04.01](#id-us04-04-01) | P1 | Test cross-view reuse and fresh metadata | testing | 6 | test engineer | Cache projection tests | Not Started |
| [ST04.04.02.01](#id-st04-04-02-01) | [US04.04.02](#id-us04-04-02) | P1 | Define per-data-class TTL and jitter policy | design | 5 | data platform engineer | Cache freshness matrix | Not Started |
| [ST04.04.02.02](#id-st04-04-02-02) | [US04.04.02](#id-us04-04-02) | P1 | Implement history TTL and cache-key versioning | implementation | 7 | backend engineer | Correct endpoint cache policies | Not Started |
| [ST04.04.02.03](#id-st04-04-02-03) | [US04.04.02](#id-us04-04-02) | P1 | Configure optional Redis L2 HybridCache storage | implementation | 10 | platform engineer | Tiered L1/L2 cache | Not Started |
| [ST04.04.02.04](#id-st04-04-02-04) | [US04.04.02](#id-us04-04-02) | P1 | Test TTL classes, jitter, and partition keys | testing | 7 | test engineer | Cache policy tests | Not Started |
| [ST04.04.03.01](#id-st04-04-03-01) | [US04.04.03](#id-us04-04-03) | P1 | Define eligible stale and negative-cache policies | design | 5 | data platform engineer | Degraded cache policy | Not Started |
| [ST04.04.03.02](#id-st04-04-03-02) | [US04.04.03](#id-us04-04-03) | P1 | Implement stale metadata and premium negative caching | implementation | 8 | backend engineer | Safe degraded-read pipeline | Not Started |
| [ST04.04.03.03](#id-st04-04-03-03) | [US04.04.03](#id-us04-04-03) | P1 | Implement watchlist-only warming with quota reservation | implementation | 7 | platform engineer | Opt-in cache warmer | Not Started |
| [ST04.04.03.04](#id-st04-04-03-04) | [US04.04.03](#id-us04-04-03) | P1 | Test staleness, entitlement changes, and warming limits | testing | 7 | test engineer | Degraded cache behavior tests | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E04 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| C-01 | C. User Experience | Make errors friendly, actionable, protocol-correct, and expand taxonomy for rate limits, authentication, authorization, configuration, cancellation, circuit-open, and partial success. | [US04.01.01](#id-us04-01-01), [US04.01.02](#id-us04-01-02), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02) | [E04](#id-e04) | P0 | Covered | Explicit review question C1. |
| C-03 | C. User Experience | Optimize response time through corrected TTLs, canonical cache keys, optional L2 cache, stale-if-error, negative caching, jitter, and opt-in watchlist warming. | [US04.04.01](#id-us04-04-01), [US04.04.02](#id-us04-04-02), [US04.04.03](#id-us04-04-03), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01), [US11.03.02](./E11-user-experience-performance-and-quota-control.md#id-us11-03-02) | [E04](#id-e04) | P1 | Covered | Explicit review question C3. |
| C-04 | C. User Experience | Implement smarter free-tier quota management with proactive weighted admission, reservations, Retry-After handling, queue deadlines, and distributed coordination. | [US04.03.01](#id-us04-03-01), [US04.03.02](#id-us04-03-02), [US04.03.03](#id-us04-03-03), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01) | [E04](#id-e04) | P0 | Covered | Explicit review question C4. |
| R-17 | Repository finding | Correct candle/history cache TTLs, cache canonical provider responses rather than view projections, and add optional distributed L2 caching. | [US04.04.01](#id-us04-04-01), [US04.04.02](#id-us04-04-02), [US04.04.03](#id-us04-04-03), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01), [US11.03.02](./E11-user-experience-performance-and-quota-control.md#id-us11-03-02) | [E04](#id-e04) | P1 | Covered | Code-level performance finding. |
| R-18 | Repository finding | Retry only transient failures, honor Retry-After, ensure intermediate 429s are observed, and prevent one logical call from over-faulting the breaker. | [US04.02.01](#id-us04-02-01), [US04.02.02](#id-us04-02-02), [US04.03.01](#id-us04-03-01) | [E04](#id-e04) | P0 | Covered | Code-level resilience finding. |
| R-19 | Repository finding | Partition quota tracking by key/tenant and coordinate multi-instance limits rather than using one process-global tracker. | [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01), [US04.03.03](#id-us04-03-03), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01) | [E02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-e02) | P0 | Covered | Code-level hosted-deployment finding. |
| RF-074 | Code-review detail | Domain failure envelopes such as NotFound, PremiumRequired, and Timeout can appear as MCP successes. | [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US04.01.01](#id-us04-01-01) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-081 | Code-review detail | Cache and rate state are shared globally and need tenant and credential partitioning. | [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01), [US04.04.02](#id-us04-04-02), [US04.03.03](#id-us04-03-03) | [E02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-e02) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-092 | Code-review detail | Error taxonomy lacks RateLimited, Unauthenticated, PermissionDenied, ConfigurationError, Cancelled, CircuitOpen, and PartialSuccess. | [US04.01.01](#id-us04-01-01), [US04.01.02](#id-us04-01-02) | [E04](#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-093 | Code-review detail | Multi-source tool degradation is inconsistent and lacks component completeness/provenance. | [US04.01.02](#id-us04-01-02), [US05.05.02](./E05-financial-and-symbol-data-correctness.md#id-us05-05-02) | [E04](#id-e04) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-094 | Code-review detail | Retry and circuit policies classify nearly every non-success response as transient and ignore Retry-After. | [US04.02.01](#id-us04-02-01), [US04.02.02](#id-us04-02-02) | [E04](#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-095 | Code-review detail | Rate tracking only observes the final response, hiding intermediate 429s and retry cost. | [US04.03.01](#id-us04-03-01) | [E04](#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-096 | Code-review detail | Free-tier quota needs weighted preflight, minute headroom, emergency reserve, burst control, and lower-cost denial guidance. | [US04.03.02](#id-us04-03-02) | [E04](#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-097 | Code-review detail | Multi-instance deployments need a distributed limiter keyed without exposing credentials. | [US04.03.03](#id-us04-03-03) | [E04](#id-e04) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-098 | Code-review detail | Cache entries should contain canonical provider data, not per-call metadata or view projection. | [US04.04.01](#id-us04-04-01), [US05.01.02](./E05-financial-and-symbol-data-correctness.md#id-us05-01-02) | [E04](#id-e04) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-099 | Code-review detail | HybridCache is local-only and historical candles incorrectly share the quote 10-second TTL category. | [US04.04.02](#id-us04-04-02) | [E04](#id-e04) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-100 | Code-review detail | Eligible data can use explicit stale-if-error and short premium negative caching; broad warming should be avoided in favor of opt-in watchlists. | [US04.04.03](#id-us04-04-03) | [E04](#id-e04) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-102 | Code-review detail | Search cache hits can return a previous request's QueryId, timestamp, and duration. | [US05.01.02](./E05-financial-and-symbol-data-correctness.md#id-us05-01-02), [US04.04.01](#id-us04-04-01) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-111 | Code-review detail | News sentiment degrades only for PremiumRequired; other recoverable failures and sequential independent calls need a quota-aware partial-result policy. | [US05.05.02](./E05-financial-and-symbol-data-correctness.md#id-us05-05-02), [US04.01.02](#id-us04-01-02), [US04.03.02](#id-us04-03-02) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 1. M0A — Critical Stabilization

- **Priority:** P0
- **Indicative window:** Weeks 1–3
- **Primary epics:** E01, E03, E04
- **Goal:** Patch the highest-risk protocol, secret-containment, and failure-signaling defects.
- **Entry criteria:** Current CI green; deployment mode and emergency scope agreed.
- **Exit criteria:** Real clients reach the SDK endpoint; simulated routes are gone; redirect leakage is blocked; domain failures set protocol errors; regression tests cover both transports.
- **Delivery note:** This is the immediate hotfix milestone, not the full P0 backlog.

### 2. M0B — Hardened Service

- **Priority:** P0
- **Indicative window:** Weeks 3–10
- **Primary epics:** E02, E04, E05, E07, E15
- **Goal:** Complete hosted security, quota controls, core data correctness, response bounds, and release gates.
- **Entry criteria:** M0A released; authentication/tenant model approved; financial semantics reviewed.
- **Exit criteria:** Hosted trust boundary is enforced; quota/retry/cache behavior is safe; search and calculations are correct; P0 release suite and supply-chain gates pass.
- **Delivery note:** Use feature flags and staged deployment; local-only scope can finish sooner.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E04 | Epic | — | P0 | Resilience, Quota Governance, and Cache Correctness | See [E04](#id-e04) | See [E04](#id-e04) | — | finnhub-mcp; epic | — | Not Started |
| F04.01 | Feature | [E04](#id-e04) | P0 | Stable Failure Contract | See [F04.01](#id-f04-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e04 | — | Not Started |
| F04.02 | Feature | [E04](#id-e04) | P0 | Correct Retry, Timeout, and Circuit Policies | See [F04.02](#id-f04-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e04 | — | Not Started |
| F04.03 | Feature | [E04](#id-e04) | P0 | Proactive Finnhub Quota Governance | See [F04.03](#id-f04-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e04 | — | Not Started |
| F04.04 | Feature | [E04](#id-e04) | P1 | Tiered and Semantically Correct Caching | See [F04.04](#id-f04-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e04 | — | Not Started |
| US04.01.01 | Story | [F04.01](#id-f04-01) | P0 | Expand and standardize the domain error taxonomy | See [US04.01.01](#id-us04-01-01) | See [US04.01.01](#id-us04-01-01) | 2.5d | errors; api-contract; agent-ux | — | Not Started |
| US04.01.02 | Story | [F04.01](#id-f04-01) | P1 | Represent partial multi-source results explicitly | See [US04.01.02](#id-us04-01-02) | See [US04.01.02](#id-us04-01-02) | 3d | errors; partial-success; provenance | [US04.01.01](#id-us04-01-01), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02) | Not Started |
| US04.02.01 | Story | [F04.02](#id-f04-02) | P0 | Retry only classified transient failures | See [US04.02.01](#id-us04-02-01) | See [US04.02.01](#id-us04-02-01) | 3d | resilience; retry; http; quota | [US04.01.01](#id-us04-01-01) | Not Started |
| US04.02.02 | Story | [F04.02](#id-f04-02) | P0 | Align circuit breaking with logical upstream calls | See [US04.02.02](#id-us04-02-02) | See [US04.02.02](#id-us04-02-02) | 3d | resilience; circuit-breaker; polly | [US04.02.01](#id-us04-02-01) | Not Started |
| US04.03.01 | Story | [F04.03](#id-f04-03) | P0 | Observe quota headers and every upstream attempt | See [US04.03.01](#id-us04-03-01) | See [US04.03.01](#id-us04-03-01) | 3d | quota; observability; rate-limit; retry | [US04.02.01](#id-us04-02-01) | Not Started |
| US04.03.02 | Story | [F04.03](#id-f04-03) | P0 | Reserve weighted quota before multi-call tools execute | See [US04.03.02](#id-us04-03-02) | See [US04.03.02](#id-us04-03-02) | 4d | quota; budget; multi-call; free-tier | [US04.01.01](#id-us04-01-01), [US04.03.01](#id-us04-03-01) | Not Started |
| US04.03.03 | Story | [F04.03](#id-f04-03) | P1 | Coordinate quotas across instances with bounded waiting | See [US04.03.03](#id-us04-03-03) | See [US04.03.03](#id-us04-03-03) | 5d | quota; redis; distributed; scaling | [US04.03.02](#id-us04-03-02), [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01) | Not Started |
| US04.04.01 | Story | [F04.04](#id-f04-04) | P1 | Cache canonical provider responses and project views afterward | See [US04.04.01](#id-us04-04-01) | See [US04.04.01](#id-us04-04-01) | 4d | cache; projection; performance; metadata | [US05.01.02](./E05-financial-and-symbol-data-correctness.md#id-us05-01-02) | Not Started |
| US04.04.02 | Story | [F04.04](#id-f04-04) | P1 | Add endpoint-specific TTLs and optional distributed L2 | See [US04.04.02](#id-us04-04-02) | See [US04.04.02](#id-us04-04-02) | 5d | cache; hybrid-cache; redis; ttl | [US03.02.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-02-01), [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01) | Not Started |
| US04.04.03 | Story | [F04.04](#id-f04-04) | P1 | Support safe stale fallback, negative caching, and opt-in warming | See [US04.04.03](#id-us04-04-03) | See [US04.04.03](#id-us04-04-03) | 4d | cache; stale-if-error; negative-cache; warming | [US04.04.02](#id-us04-04-02), [US04.03.02](#id-us04-03-02) | Not Started |
| ST04.01.01.01 | Sub-task | [US04.01.01](#id-us04-01-01) | P0 | Specify error codes, retryability, and remediation matrix | See [ST04.01.01.01](#id-st04-01-01-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST04.01.01.02 | Sub-task | [US04.01.01](#id-us04-01-01) | P0 | Implement shared error factory and mappings | See [ST04.01.01.02](#id-st04-01-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.01.01.03 | Sub-task | [US04.01.01](#id-us04-01-01) | P0 | Add compatibility snapshots and sanitization tests | See [ST04.01.01.03](#id-st04-01-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST04.01.02.01 | Sub-task | [US04.01.02](#id-us04-01-02) | P1 | Define component completeness/provenance DTOs | See [ST04.01.02.01](#id-st04-01-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST04.01.02.02 | Sub-task | [US04.01.02](#id-us04-01-02) | P1 | Apply partial-result aggregation to multi-call tools | See [ST04.01.02.02](#id-st04-01-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST04.01.02.03 | Sub-task | [US04.01.02](#id-us04-01-02) | P1 | Test mandatory and optional component failures | See [ST04.01.02.03](#id-st04-01-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST04.02.01.01 | Sub-task | [US04.02.01](#id-us04-02-01) | P0 | Document endpoint status/exception retry matrix | See [ST04.02.01.01](#id-st04-02-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST04.02.01.02 | Sub-task | [US04.02.01](#id-us04-02-01) | P0 | Implement bounded jitter and Retry-After handling | See [ST04.02.01.02](#id-st04-02-01-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.02.01.03 | Sub-task | [US04.02.01](#id-us04-02-01) | P0 | Create parameterized attempt-count tests | See [ST04.02.01.03](#id-st04-02-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST04.02.02.01 | Sub-task | [US04.02.02](#id-us04-02-02) | P0 | Model and document resilience policy order | See [ST04.02.02.01](#id-st04-02-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; design | — | Not Started |
| ST04.02.02.02 | Sub-task | [US04.02.02](#id-us04-02-02) | P0 | Reconfigure circuit predicates and half-open probes | See [ST04.02.02.02](#id-st04-02-02-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.02.02.03 | Sub-task | [US04.02.02](#id-us04-02-02) | P0 | Test retry/circuit interaction and recovery stampedes | See [ST04.02.02.03](#id-st04-02-02-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; reliability-testing | — | Not Started |
| ST04.03.01.01 | Sub-task | [US04.03.01](#id-us04-03-01) | P0 | Move quota observation inside physical-attempt boundary | See [ST04.03.01.01](#id-st04-03-01-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.03.01.02 | Sub-task | [US04.03.01](#id-us04-03-01) | P0 | Expose honest quota status and age metadata | See [ST04.03.01.02](#id-st04-03-01-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST04.03.01.03 | Sub-task | [US04.03.01](#id-us04-03-01) | P0 | Test headers and 429 accounting through retries | See [ST04.03.01.03](#id-st04-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST04.03.02.01 | Sub-task | [US04.03.02](#id-us04-03-02) | P0 | Catalogue physical call costs for every tool | See [ST04.03.02.01](#id-st04-03-02-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; analysis | — | Not Started |
| ST04.03.02.02 | Sub-task | [US04.03.02](#id-us04-03-02) | P0 | Implement atomic reservation, release, and emergency reserve | See [ST04.03.02.02](#id-st04-03-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST04.03.02.03 | Sub-task | [US04.03.02](#id-us04-03-02) | P0 | Integrate preflight into multi-call tools | See [ST04.03.02.03](#id-st04-03-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST04.03.02.04 | Sub-task | [US04.03.02](#id-us04-03-02) | P0 | Test minute/burst/reset and zero-work denial cases | See [ST04.03.02.04](#id-st04-03-02-04) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST04.03.03.01 | Sub-task | [US04.03.03](#id-us04-03-03) | P1 | Define limiter storage interface and failure modes | See [ST04.03.03.01](#id-st04-03-03-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST04.03.03.02 | Sub-task | [US04.03.03](#id-us04-03-03) | P1 | Implement Redis atomic reservation backend | See [ST04.03.03.02](#id-st04-03-03-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST04.03.03.03 | Sub-task | [US04.03.03](#id-us04-03-03) | P1 | Run multi-instance aggregate-limit tests | See [ST04.03.03.03](#id-st04-03-03-03) | Not applicable; see detail or parent section | 9h | finnhub-mcp; performance-testing | — | Not Started |
| ST04.04.01.01 | Sub-task | [US04.04.01](#id-us04-04-01) | P1 | Split canonical provider data from response metadata | See [ST04.04.01.01](#id-st04-04-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; refactoring | — | Not Started |
| ST04.04.01.02 | Sub-task | [US04.04.01](#id-us04-04-01) | P1 | Normalize cache keys and post-cache projections | See [ST04.04.01.02](#id-st04-04-01-02) | Not applicable; see detail or parent section | 9h | finnhub-mcp; implementation | — | Not Started |
| ST04.04.01.03 | Sub-task | [US04.04.01](#id-us04-04-01) | P1 | Test cross-view reuse and fresh metadata | See [ST04.04.01.03](#id-st04-04-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST04.04.02.01 | Sub-task | [US04.04.02](#id-us04-04-02) | P1 | Define per-data-class TTL and jitter policy | See [ST04.04.02.01](#id-st04-04-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST04.04.02.02 | Sub-task | [US04.04.02](#id-us04-04-02) | P1 | Implement history TTL and cache-key versioning | See [ST04.04.02.02](#id-st04-04-02-02) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.04.02.03 | Sub-task | [US04.04.02](#id-us04-04-02) | P1 | Configure optional Redis L2 HybridCache storage | See [ST04.04.02.03](#id-st04-04-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST04.04.02.04 | Sub-task | [US04.04.02](#id-us04-04-02) | P1 | Test TTL classes, jitter, and partition keys | See [ST04.04.02.04](#id-st04-04-02-04) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST04.04.03.01 | Sub-task | [US04.04.03](#id-us04-04-03) | P1 | Define eligible stale and negative-cache policies | See [ST04.04.03.01](#id-st04-04-03-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST04.04.03.02 | Sub-task | [US04.04.03](#id-us04-04-03) | P1 | Implement stale metadata and premium negative caching | See [ST04.04.03.02](#id-st04-04-03-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST04.04.03.03 | Sub-task | [US04.04.03](#id-us04-04-03) | P1 | Implement watchlist-only warming with quota reservation | See [ST04.04.03.03](#id-st04-04-03-03) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST04.04.03.04 | Sub-task | [US04.04.03](#id-us04-04-03) | P1 | Test staleness, entitlement changes, and warming limits | See [ST04.04.03.04](#id-st04-04-03-04) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |

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

- [ ] Create E04 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 4 Features under E04.
- [ ] Create all 10 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 33 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 19 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 48 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

