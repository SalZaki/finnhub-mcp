---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E10
title: "Service Operations, Resources and Extensibility"
priority: P1
phase: "M2 — Operability & Adoption"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 6
  subtasks: 18
  traceability_owned: 13
  traceability_items: 14
story_estimate_days: 37
subtask_estimate_hours: 266
---

<a id="id-e10"></a>
# E10 — Service Operations, Resources and Extensibility

This is the self-contained coding-agent backlog for E10. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E10 | P1 | 5 | 6 | 18 | 37 | 266 | M2 — Operability & Adoption | Not Started |

## 2. Epic Definition

**Objective:** Make runtime health, exchange reference data, tool availability, third-party modules and telemetry operationally reliable.

**Business value:** Enables safe deployment, proactive diagnosis and controlled ecosystem growth without forking core registration logic.

**Exit criteria:**

- [ ] Liveness, readiness and redacted service status are available without consuming Finnhub quota.
- [ ] Exchange data is reproducibly refreshed and carries source/version metadata.
- [ ] Tool registration, enablement and extension modules use one authoritative profile and emit correct list-change notifications.
- [ ] Low-cardinality OpenTelemetry and Prometheus signals cover tool, cache, quota and upstream behavior.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F10.01](#id-f10-01) | P1 | Health and service-status surfaces | 1 | 5 | Not Started |
| [F10.02](#id-f10-02) | P1 | Exchange reference-data refresh | 1 | 4 | Not Started |
| [F10.03](#id-f10-03) | P1 | Authoritative tool profiles and enablement | 2 | 13 | Not Started |
| [F10.04](#id-f10-04) | P2 | Controlled extension modules | 1 | 8 | Not Started |
| [F10.05](#id-f10-05) | P1 | OpenTelemetry and Prometheus observability | 1 | 7 | Not Started |

<a id="id-f10-01"></a>
### F10.01 — Health and service-status surfaces

- **Parent Epic:** [E10](#id-e10)
- **Priority:** P1
- **Status:** Not Started

**Description:** Separate liveness, readiness and AI-readable status while avoiding quota-burning dependency probes.

**Expected outcome:** Operators and agents can distinguish process, configuration and upstream degradation.

**Stories:**

- [US10.01.01](#id-us10-01-01) — Publish liveness, readiness and AI-readable status (P1, 5d)

<a id="id-f10-02"></a>
### F10.02 — Exchange reference-data refresh

- **Parent Epic:** [E10](#id-e10)
- **Priority:** P1
- **Status:** Not Started

**Description:** Refresh the static exchange catalogue through a validated, reviewable pipeline with provenance.

**Expected outcome:** The low-latency static resource stays current without adding runtime Finnhub calls.

**Stories:**

- [US10.02.01](#id-us10-02-01) — Refresh exchange catalogue through reviewed automation (P1, 4d)

<a id="id-f10-03"></a>
### F10.03 — Authoritative tool profiles and enablement

- **Parent Epic:** [E10](#id-e10)
- **Priority:** P1
- **Status:** Not Started

**Description:** Separate search indexing from registration and make one profile drive visibility, capabilities, authorization and search.

**Expected outcome:** Tools can be enabled or disabled without catalogue drift or misleading lazy-schema claims.

**Stories:**

- [US10.03.01](#id-us10-03-01) — Create one authoritative tool profile (P1, 7d)
- [US10.03.02](#id-us10-03-02) — Enable and disable tools safely (P1, 6d)

<a id="id-f10-04"></a>
### F10.04 — Controlled extension modules

- **Parent Epic:** [E10](#id-e10)
- **Priority:** P2
- **Status:** Not Started

**Description:** Define a compile-time first module contract and a signed allowlist path for optional packages.

**Expected outcome:** Third parties can contribute tools without arbitrary runtime code discovery.

**Stories:**

- [US10.04.01](#id-us10-04-01) — Load controlled third-party tool modules (P2, 8d)

<a id="id-f10-05"></a>
### F10.05 — OpenTelemetry and Prometheus observability

- **Parent Epic:** [E10](#id-e10)
- **Priority:** P1
- **Status:** Not Started

**Description:** Instrument low-cardinality tool, upstream, cache, quota, circuit and session signals.

**Expected outcome:** Usage and reliability regressions are visible without leaking queries, symbols or credentials.

**Stories:**

- [US10.05.01](#id-us10-05-01) — Instrument low-cardinality service metrics (P1, 7d)

## 4. User Stories and Subtasks

<a id="id-us10-01-01"></a>
### US10.01.01 — Publish liveness, readiness and AI-readable status

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.01](#id-f10-01) | P1 | 5 | 34 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Operator and AI agent

**User story:** As an operator or agent, I want the appropriate health surface so that I can distinguish process health, readiness and upstream degradation safely.

**Acceptance criteria:**

- [ ] /health/live checks only process/event-loop viability and does not require Finnhub or consume quota.
- [ ] /health/ready verifies configuration, cache and internal dependencies using passive state or bounded local checks, not quota-burning live market probes.
- [ ] finnhub://resources/api-status or service-status reports redacted quota, circuit, cache and last-upstream state with timestamps; an optional health tool returns the same bounded contract for AI clients.
- [ ] Hosted status surfaces require the configured access policy and never expose API keys, tenant identifiers or raw failure bodies.
- [ ] Tests cover healthy, missing-key, circuit-open, stale-upstream and dependency-unavailable states.

**Dependencies:** [US10.05.01](#id-us10-05-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02)

**Labels:** `health` `resource` `operations` `P1`

**Source findings:**

- The existing health endpoint is unconditional.
- Add liveness/readiness and an AI-readable service-status resource/tool without spending Finnhub quota.

**Subtasks:**

<a id="id-st10-01-01-01"></a>
- [ ] **ST10.01.01.01 — Define live/ready/status state model**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Health dependency and redaction specification.
  - Status: Not Started
<a id="id-st10-01-01-02"></a>
- [ ] **ST10.01.01.02 — Implement endpoints and status resource/tool**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Quota-free live/ready and redacted AI status surfaces.
  - Status: Not Started
<a id="id-st10-01-01-03"></a>
- [ ] **ST10.01.01.03 — Test degraded and protected states**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Health matrix and access-control tests.
  - Status: Not Started

<a id="id-us10-02-01"></a>
### US10.02.01 — Refresh exchange catalogue through reviewed automation

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.02](#id-f10-02) | P1 | 4 | 30 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Maintainer

**User story:** As a maintainer, I want a scheduled validation pipeline so that the static 79-exchange resource stays current and reproducible.

**Acceptance criteria:**

- [ ] A monthly or quarterly CI job fetches the authoritative exchange source, validates schema, codes, uniqueness and expected change bounds, and generates deterministic output.
- [ ] The job opens a reviewable pull request rather than mutating production data at runtime.
- [ ] The resource includes source_url, retrieved_at, data_as_of, checksum and schema_version and preserves the last known-good catalogue on failure.
- [ ] Diff tests flag removals, code changes and suspicious count shifts for human approval.

**Dependencies:** —

**Labels:** `resource` `exchanges` `automation` `P1`

**Source findings:**

- Static exchange data is appropriate at runtime but should be refreshed periodically through CI and expose source/retrieved/checksum/schema/as-of metadata.

**Subtasks:**

<a id="id-st10-02-01-01"></a>
- [ ] **ST10.02.01.01 — Create deterministic exchange generator**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Validated source-to-resource generation script.
  - Status: Not Started
<a id="id-st10-02-01-02"></a>
- [ ] **ST10.02.01.02 — Add scheduled PR workflow and diff guards**
  - Type: devops
  - Estimate: 12 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Monthly/quarterly reviewable refresh automation.
  - Status: Not Started
<a id="id-st10-02-01-03"></a>
- [ ] **ST10.02.01.03 — Expose exchange provenance metadata**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Source/as-of/checksum/schema resource fields.
  - Status: Not Started

<a id="id-us10-03-01"></a>
### US10.03.01 — Create one authoritative tool profile

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.03](#id-f10-03) | P1 | 7 | 50 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Tool maintainer

**User story:** As a tool maintainer, I want one profile to drive registration and discovery so that tools/list, capabilities and search never disagree.

**Acceptance criteria:**

- [ ] The current IToolRegistry search-only responsibility is renamed IToolSearchIndex or equivalent.
- [ ] A typed ToolProfile is the single source for id, version, description, schemas, tags, examples, cost, entitlement, authorization policy, enabled state and registration factory.
- [ ] Startup builds registration, tools/list, capabilities resource, authorization map and BM25 snapshot from the same validated immutable profiles.
- [ ] Contract tests fail on duplicate ids, missing schemas, stale examples or catalogue/search/list divergence.
- [ ] Documentation clarifies that search-tools ranks tools but cannot hide already advertised schemas by itself.

**Dependencies:** —

**Labels:** `registry` `architecture` `tool-discovery` `P1`

**Source findings:**

- IToolRegistry is currently a search index and does not dynamically register tools.
- search-tools does not actually keep normal tools/list schemas off the wire; real lazy listing requires actual catalogue changes.

**Subtasks:**

<a id="id-st10-03-01-01"></a>
- [ ] **ST10.03.01.01 — Define ToolProfile and rename search index**
  - Type: refactor
  - Estimate: 16 hours
  - Suggested owner role: Software architect
  - Deliverable/evidence: Separated registration/profile/search abstractions.
  - Status: Not Started
<a id="id-st10-03-01-02"></a>
- [ ] **ST10.03.01.02 — Generate catalogue consumers from profiles**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Unified list, capabilities, auth, registration and BM25 build.
  - Status: Not Started
<a id="id-st10-03-01-03"></a>
- [ ] **ST10.03.01.03 — Add catalogue divergence tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Profile integrity and consumer-consistency suite.
  - Status: Not Started

<a id="id-us10-03-02"></a>
### US10.03.02 — Enable and disable tools safely

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.03](#id-f10-03) | P1 | 6 | 44 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Service administrator

**User story:** As an administrator, I want tool profiles enabled by configuration and policy so that deployments can expose only licensed and approved capabilities.

**Acceptance criteria:**

- [ ] Startup configuration can enable/disable tools by profile, environment, entitlement and authorization policy before they are advertised.
- [ ] If runtime changes are supported, a validated immutable catalogue is swapped atomically and emits MCP tools/list_changed only to capable clients.
- [ ] Disabled tools disappear consistently from tools/list, capabilities, search results, next actions and workflow planning.
- [ ] Invalid reloads retain the prior catalogue and expose a redacted operational error.
- [ ] Concurrency tests verify calls against the old/new snapshots complete deterministically.

**Dependencies:** [US10.03.01](#id-us10-03-01)

**Labels:** `registry` `feature-flags` `mcp` `P1`

**Source findings:**

- Dynamic enabling/disabling is not supported by the current registry.
- True dynamic listing needs atomic tool-set changes and list_changed notifications.

**Subtasks:**

<a id="id-st10-03-02-01"></a>
- [ ] **ST10.03.02.01 — Implement startup enablement policies**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Configuration/entitlement/auth-driven tool filtering.
  - Status: Not Started
<a id="id-st10-03-02-02"></a>
- [ ] **ST10.03.02.02 — Implement atomic optional runtime reload**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Validated snapshot swap and list_changed notification.
  - Status: Not Started
<a id="id-st10-03-02-03"></a>
- [ ] **ST10.03.02.03 — Test disabled/reload consistency**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Concurrency and all-catalogue-surface tests.
  - Status: Not Started

<a id="id-us10-04-01"></a>
### US10.04.01 — Load controlled third-party tool modules

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.04](#id-f10-04) | P2 | 8 | 58 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Extension developer

**User story:** As an extension developer, I want a documented module contract so that I can add a tool without editing core startup code.

**Acceptance criteria:**

- [ ] IFinnhubToolModule or equivalent registers profiles and services through a versioned compatibility contract and is supported first as compile-time/NuGet composition.
- [ ] Optional runtime packages require an explicit signed/hash allowlist, compatible API version and administrator restart or reviewed reload.
- [ ] Modules cannot read credentials directly; they receive scoped provider/config abstractions and are subject to common validation, quota, cache, auth and telemetry middleware.
- [ ] A sample module, conformance test kit and failure-isolation tests document the contributor path.
- [ ] Arbitrary directory scanning or unsigned plugin execution is explicitly unsupported.

**Dependencies:** [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02)

**Labels:** `extensions` `plugins` `architecture` `P2`

**Source findings:**

- Third-party tool registration should use a controlled compile-time/NuGet module contract, with signed allowlisting if runtime packages are ever accepted.

**Subtasks:**

<a id="id-st10-04-01-01"></a>
- [ ] **ST10.04.01.01 — Specify extension compatibility and trust model**
  - Type: design
  - Estimate: 14 hours
  - Suggested owner role: Security architect
  - Deliverable/evidence: Module API, permission boundaries and signing policy.
  - Status: Not Started
<a id="id-st10-04-01-02"></a>
- [ ] **ST10.04.01.02 — Implement module loader and sample**
  - Type: implementation
  - Estimate: 28 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Compile-time package composition and example extension.
  - Status: Not Started
<a id="id-st10-04-01-03"></a>
- [ ] **ST10.04.01.03 — Build conformance and isolation kit**
  - Type: test
  - Estimate: 16 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: Middleware, compatibility and failure-isolation tests.
  - Status: Not Started

<a id="id-us10-05-01"></a>
### US10.05.01 — Instrument low-cardinality service metrics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F10.05](#id-f10-05) | P1 | 7 | 50 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Service operator

**User story:** As an operator, I want Prometheus/OpenTelemetry metrics so that I can detect slow, failing or quota-heavy tools without leaking user data.

**Acceptance criteria:**

- [ ] Counters and histograms cover tool calls/outcomes/view/error, duration, upstream requests/latency/status, cache hits/misses/stale, token estimates, limiter wait/reject, quota headroom, circuit state and active sessions.
- [ ] Metric labels use bounded values such as tool, outcome and error code and prohibit symbol, query, user id, tenant id and API key labels.
- [ ] Tracing propagates a correlation id through MCP call, cache and provider request with secrets and raw payloads redacted.
- [ ] The metrics endpoint is disabled or access-controlled by default for hosted mode and includes scrape/load tests.
- [ ] Dashboards and alert examples cover elevated error rate, p95 latency, quota exhaustion and circuit opening.

**Dependencies:** —

**Labels:** `observability` `prometheus` `opentelemetry` `P1`

**Source findings:**

- Prometheus metrics should cover tool frequency and errors plus cache, tokens, limiter, quota, circuit and sessions.
- Labels must remain low cardinality and must not include symbols, queries, users or keys.

**Subtasks:**

<a id="id-st10-05-01-01"></a>
- [ ] **ST10.05.01.01 — Define metric and trace semantic conventions**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Low-cardinality observability specification.
  - Status: Not Started
<a id="id-st10-05-01-02"></a>
- [ ] **ST10.05.01.02 — Instrument MCP, cache, provider and resilience paths**
  - Type: implementation
  - Estimate: 26 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: OpenTelemetry metrics/traces and protected Prometheus exporter.
  - Status: Not Started
<a id="id-st10-05-01-03"></a>
- [ ] **ST10.05.01.03 — Add dashboards, alerts and label tests**
  - Type: operations
  - Estimate: 14 hours
  - Suggested owner role: SRE
  - Deliverable/evidence: Operational dashboard pack and cardinality/redaction tests.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST10.01.01.01](#id-st10-01-01-01) | [US10.01.01](#id-us10-01-01) | P1 | Define live/ready/status state model | design | 8 | SRE | Health dependency and redaction specification. | Not Started |
| [ST10.01.01.02](#id-st10-01-01-02) | [US10.01.01](#id-us10-01-01) | P1 | Implement endpoints and status resource/tool | implementation | 18 | Backend engineer | Quota-free live/ready and redacted AI status surfaces. | Not Started |
| [ST10.01.01.03](#id-st10-01-01-03) | [US10.01.01](#id-us10-01-01) | P1 | Test degraded and protected states | test | 8 | QA automation engineer | Health matrix and access-control tests. | Not Started |
| [ST10.02.01.01](#id-st10-02-01-01) | [US10.02.01](#id-us10-02-01) | P1 | Create deterministic exchange generator | implementation | 12 | Backend engineer | Validated source-to-resource generation script. | Not Started |
| [ST10.02.01.02](#id-st10-02-01-02) | [US10.02.01](#id-us10-02-01) | P1 | Add scheduled PR workflow and diff guards | devops | 12 | DevOps engineer | Monthly/quarterly reviewable refresh automation. | Not Started |
| [ST10.02.01.03](#id-st10-02-01-03) | [US10.02.01](#id-us10-02-01) | P1 | Expose exchange provenance metadata | implementation | 6 | Backend engineer | Source/as-of/checksum/schema resource fields. | Not Started |
| [ST10.03.01.01](#id-st10-03-01-01) | [US10.03.01](#id-us10-03-01) | P1 | Define ToolProfile and rename search index | refactor | 16 | Software architect | Separated registration/profile/search abstractions. | Not Started |
| [ST10.03.01.02](#id-st10-03-01-02) | [US10.03.01](#id-us10-03-01) | P1 | Generate catalogue consumers from profiles | implementation | 24 | Backend engineer | Unified list, capabilities, auth, registration and BM25 build. | Not Started |
| [ST10.03.01.03](#id-st10-03-01-03) | [US10.03.01](#id-us10-03-01) | P1 | Add catalogue divergence tests | test | 10 | QA automation engineer | Profile integrity and consumer-consistency suite. | Not Started |
| [ST10.03.02.01](#id-st10-03-02-01) | [US10.03.02](#id-us10-03-02) | P1 | Implement startup enablement policies | implementation | 16 | Backend engineer | Configuration/entitlement/auth-driven tool filtering. | Not Started |
| [ST10.03.02.02](#id-st10-03-02-02) | [US10.03.02](#id-us10-03-02) | P1 | Implement atomic optional runtime reload | implementation | 18 | MCP engineer | Validated snapshot swap and list_changed notification. | Not Started |
| [ST10.03.02.03](#id-st10-03-02-03) | [US10.03.02](#id-us10-03-02) | P1 | Test disabled/reload consistency | test | 10 | QA automation engineer | Concurrency and all-catalogue-surface tests. | Not Started |
| [ST10.04.01.01](#id-st10-04-01-01) | [US10.04.01](#id-us10-04-01) | P2 | Specify extension compatibility and trust model | design | 14 | Security architect | Module API, permission boundaries and signing policy. | Not Started |
| [ST10.04.01.02](#id-st10-04-01-02) | [US10.04.01](#id-us10-04-01) | P2 | Implement module loader and sample | implementation | 28 | Backend engineer | Compile-time package composition and example extension. | Not Started |
| [ST10.04.01.03](#id-st10-04-01-03) | [US10.04.01](#id-us10-04-01) | P2 | Build conformance and isolation kit | test | 16 | Security engineer | Middleware, compatibility and failure-isolation tests. | Not Started |
| [ST10.05.01.01](#id-st10-05-01-01) | [US10.05.01](#id-us10-05-01) | P1 | Define metric and trace semantic conventions | design | 10 | SRE | Low-cardinality observability specification. | Not Started |
| [ST10.05.01.02](#id-st10-05-01-02) | [US10.05.01](#id-us10-05-01) | P1 | Instrument MCP, cache, provider and resilience paths | implementation | 26 | Backend engineer | OpenTelemetry metrics/traces and protected Prometheus exporter. | Not Started |
| [ST10.05.01.03](#id-st10-05-01-03) | [US10.05.01](#id-us10-05-01) | P1 | Add dashboards, alerts and label tests | operations | 14 | SRE | Operational dashboard pack and cardinality/redaction tests. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E10 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| D-01 | D. Tools & Resources | Expose health/liveness/readiness safely for operators and an AI-readable service-status resource without quota-burning probes. | [US10.01.01](#id-us10-01-01), [US13.04.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-04-02) | [E10](#id-e10) | P1 | Covered | Explicit review question D1. |
| D-02 | D. Tools & Resources | Keep exchange reference data generated and periodically refreshed with source, retrieved date, checksum, schema validation, and reviewable pull requests. | [US10.02.01](#id-us10-02-01) | [E10](#id-e10) | P1 | Covered | Explicit review question D2. |
| D-03 | D. Tools & Resources | Add a versioned, allowlisted custom tool/module extension mechanism suitable for third-party contributions. | [US10.04.01](#id-us10-04-01) | [E10](#id-e10) | P2 | Covered | Explicit review question D3. |
| D-04 | D. Tools & Resources | Replace or rename the search-only IToolRegistry and add one source of truth for startup tool profiles, dynamic enablement, capabilities, indexing, and list_changed behavior. | [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02) | [E10](#id-e10) | P1 | Covered | Explicit review question D4. |
| D-05 | D. Tools & Resources | Add OpenTelemetry/Prometheus metrics for tool calls, outcomes, latency, upstream usage, cache, quota, tokens, and circuit state with low-cardinality labels. | [US10.05.01](#id-us10-05-01) | [E10](#id-e10) | P1 | Covered | Explicit review question D5. |
| R-16 | Repository finding | Correct claims that search-tools keeps other schemas off the MCP tools/list wire unless actual dynamic tool-list behavior is implemented. | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Documentation/context finding. |
| R-21 | Repository finding | Add exchange reference source/retrieval/checksum/as-of metadata and an automated refresh validation workflow. | [US10.02.01](#id-us10-02-01) | [E10](#id-e10) | P1 | Covered | Reference-data governance finding. |
| R-22 | Repository finding | Rename IToolRegistry to reflect its search-index role unless it gains actual registration, enablement, authorization, and list-change control. | [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02) | [E10](#id-e10) | P1 | Covered | Architecture finding. |
| R-23 | Repository finding | Prevent high-cardinality or secret-bearing metric labels such as symbol, raw query, user ID, or API key. | [US10.05.01](#id-us10-05-01) | [E10](#id-e10) | P1 | Covered | Observability/privacy finding. |
| RF-123 | Code-review detail | D1 - proactive health tool and meaningful service status | [US10.01.01](#id-us10-01-01) | [E10](#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-124 | Code-review detail | D2 - exchange resource freshness without runtime latency | [US10.02.01](#id-us10-02-01) | [E10](#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-125 | Code-review detail | D3/D4 - custom registration, dynamic enable/disable and IToolRegistry limitations | [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02), [US10.04.01](#id-us10-04-01) | [E10](#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-126 | Code-review detail | D5 - Prometheus metrics for tool frequency, errors, cache, quota and resilience | [US10.05.01](#id-us10-05-01), [US11.05.01](./E11-user-experience-performance-and-quota-control.md#id-us11-05-01) | [E10](#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-138 | Code-review detail | Observed issue - search-tools cannot itself provide true lazy schema listing | [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02) | [E10](#id-e10) | P1 | Covered | Detailed finding retained from the repository review. |

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
| E10 | Epic | — | P1 | Service Operations, Resources and Extensibility | See [E10](#id-e10) | See [E10](#id-e10) | — | finnhub-mcp; epic | — | Not Started |
| F10.01 | Feature | [E10](#id-e10) | P1 | Health and service-status surfaces | See [F10.01](#id-f10-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e10 | — | Not Started |
| F10.02 | Feature | [E10](#id-e10) | P1 | Exchange reference-data refresh | See [F10.02](#id-f10-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e10 | — | Not Started |
| F10.03 | Feature | [E10](#id-e10) | P1 | Authoritative tool profiles and enablement | See [F10.03](#id-f10-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e10 | — | Not Started |
| F10.04 | Feature | [E10](#id-e10) | P2 | Controlled extension modules | See [F10.04](#id-f10-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e10 | — | Not Started |
| F10.05 | Feature | [E10](#id-e10) | P1 | OpenTelemetry and Prometheus observability | See [F10.05](#id-f10-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e10 | — | Not Started |
| US10.01.01 | Story | [F10.01](#id-f10-01) | P1 | Publish liveness, readiness and AI-readable status | See [US10.01.01](#id-us10-01-01) | See [US10.01.01](#id-us10-01-01) | 5d | health; resource; operations; P1 | [US10.05.01](#id-us10-05-01), [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02) | Not Started |
| US10.02.01 | Story | [F10.02](#id-f10-02) | P1 | Refresh exchange catalogue through reviewed automation | See [US10.02.01](#id-us10-02-01) | See [US10.02.01](#id-us10-02-01) | 4d | resource; exchanges; automation; P1 | — | Not Started |
| US10.03.01 | Story | [F10.03](#id-f10-03) | P1 | Create one authoritative tool profile | See [US10.03.01](#id-us10-03-01) | See [US10.03.01](#id-us10-03-01) | 7d | registry; architecture; tool-discovery; P1 | — | Not Started |
| US10.03.02 | Story | [F10.03](#id-f10-03) | P1 | Enable and disable tools safely | See [US10.03.02](#id-us10-03-02) | See [US10.03.02](#id-us10-03-02) | 6d | registry; feature-flags; mcp; P1 | [US10.03.01](#id-us10-03-01) | Not Started |
| US10.04.01 | Story | [F10.04](#id-f10-04) | P2 | Load controlled third-party tool modules | See [US10.04.01](#id-us10-04-01) | See [US10.04.01](#id-us10-04-01) | 8d | extensions; plugins; architecture; P2 | [US10.03.01](#id-us10-03-01), [US10.03.02](#id-us10-03-02) | Not Started |
| US10.05.01 | Story | [F10.05](#id-f10-05) | P1 | Instrument low-cardinality service metrics | See [US10.05.01](#id-us10-05-01) | See [US10.05.01](#id-us10-05-01) | 7d | observability; prometheus; opentelemetry; P1 | — | Not Started |
| ST10.01.01.01 | Sub-task | [US10.01.01](#id-us10-01-01) | P1 | Define live/ready/status state model | See [ST10.01.01.01](#id-st10-01-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST10.01.01.02 | Sub-task | [US10.01.01](#id-us10-01-01) | P1 | Implement endpoints and status resource/tool | See [ST10.01.01.02](#id-st10-01-01-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST10.01.01.03 | Sub-task | [US10.01.01](#id-us10-01-01) | P1 | Test degraded and protected states | See [ST10.01.01.03](#id-st10-01-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST10.02.01.01 | Sub-task | [US10.02.01](#id-us10-02-01) | P1 | Create deterministic exchange generator | See [ST10.02.01.01](#id-st10-02-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST10.02.01.02 | Sub-task | [US10.02.01](#id-us10-02-01) | P1 | Add scheduled PR workflow and diff guards | See [ST10.02.01.02](#id-st10-02-01-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; devops | — | Not Started |
| ST10.02.01.03 | Sub-task | [US10.02.01](#id-us10-02-01) | P1 | Expose exchange provenance metadata | See [ST10.02.01.03](#id-st10-02-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST10.03.01.01 | Sub-task | [US10.03.01](#id-us10-03-01) | P1 | Define ToolProfile and rename search index | See [ST10.03.01.01](#id-st10-03-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; refactor | — | Not Started |
| ST10.03.01.02 | Sub-task | [US10.03.01](#id-us10-03-01) | P1 | Generate catalogue consumers from profiles | See [ST10.03.01.02](#id-st10-03-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST10.03.01.03 | Sub-task | [US10.03.01](#id-us10-03-01) | P1 | Add catalogue divergence tests | See [ST10.03.01.03](#id-st10-03-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST10.03.02.01 | Sub-task | [US10.03.02](#id-us10-03-02) | P1 | Implement startup enablement policies | See [ST10.03.02.01](#id-st10-03-02-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST10.03.02.02 | Sub-task | [US10.03.02](#id-us10-03-02) | P1 | Implement atomic optional runtime reload | See [ST10.03.02.02](#id-st10-03-02-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST10.03.02.03 | Sub-task | [US10.03.02](#id-us10-03-02) | P1 | Test disabled/reload consistency | See [ST10.03.02.03](#id-st10-03-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST10.04.01.01 | Sub-task | [US10.04.01](#id-us10-04-01) | P2 | Specify extension compatibility and trust model | See [ST10.04.01.01](#id-st10-04-01-01) | Not applicable; see detail or parent section | 14h | finnhub-mcp; design | — | Not Started |
| ST10.04.01.02 | Sub-task | [US10.04.01](#id-us10-04-01) | P2 | Implement module loader and sample | See [ST10.04.01.02](#id-st10-04-01-02) | Not applicable; see detail or parent section | 28h | finnhub-mcp; implementation | — | Not Started |
| ST10.04.01.03 | Sub-task | [US10.04.01](#id-us10-04-01) | P2 | Build conformance and isolation kit | See [ST10.04.01.03](#id-st10-04-01-03) | Not applicable; see detail or parent section | 16h | finnhub-mcp; test | — | Not Started |
| ST10.05.01.01 | Sub-task | [US10.05.01](#id-us10-05-01) | P1 | Define metric and trace semantic conventions | See [ST10.05.01.01](#id-st10-05-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST10.05.01.02 | Sub-task | [US10.05.01](#id-us10-05-01) | P1 | Instrument MCP, cache, provider and resilience paths | See [ST10.05.01.02](#id-st10-05-01-02) | Not applicable; see detail or parent section | 26h | finnhub-mcp; implementation | — | Not Started |
| ST10.05.01.03 | Sub-task | [US10.05.01](#id-us10-05-01) | P1 | Add dashboards, alerts and label tests | See [ST10.05.01.03](#id-st10-05-01-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; operations | — | Not Started |

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

- [ ] Create E10 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E10.
- [ ] Create all 6 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 18 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 14 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 30 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

