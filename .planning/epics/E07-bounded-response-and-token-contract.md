---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E07
title: "Bounded Response and Token Contract"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 4
  user_stories: 7
  subtasks: 21
  traceability_owned: 8
  traceability_items: 13
story_estimate_days: 32
subtask_estimate_hours: 246
---

<a id="id-e07"></a>
# E07 — Bounded Response and Token Contract

This is the self-contained coding-agent backlog for E07. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E07 | P0 | 4 | 7 | 21 | 32 | 246 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Make every tool response predictable, projectable, pageable, token-budget-aware and protocol-correct.

**Business value:** Prevents context-window overruns, misleading truncation and silent application errors while giving clients a stable integration contract.

**Exit criteria:**

- [ ] View presets have documented, non-overlapping semantics and all collection tools support deterministic bounds.
- [ ] Field projection, cursor pagination and token preflight are enforced before expensive upstream work.
- [ ] Success, partial success and failure are returned as structured MCP results with stable machine-readable metadata.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F07.01](#id-f07-01) | P1 | View presets and field projection | 2 | 9 | Not Started |
| [F07.02](#id-f07-02) | P0 | Pagination and deterministic bounds | 1 | 6 | Not Started |
| [F07.03](#id-f07-03) | P1 | Token budget and usage reporting | 2 | 9 | Not Started |
| [F07.04](#id-f07-04) | P0 | Structured result and error contract | 2 | 8 | Not Started |

<a id="id-f07-01"></a>
### F07.01 — View presets and field projection

- **Parent Epic:** [E07](#id-e07)
- **Priority:** P1
- **Status:** Not Started

**Description:** Redefine summary, standard and full as documented presets and add genuine output field projection.

**Expected outcome:** Callers request only the detail they need and identical or misleading view tiers are eliminated.

**Stories:**

- [US07.01.01](#id-us07-01-01) — Define meaningful view presets (P1, 5d)
- [US07.01.02](#id-us07-01-02) — Implement real field projection (P1, 4d)

<a id="id-f07-02"></a>
### F07.02 — Pagination and deterministic bounds

- **Parent Epic:** [E07](#id-e07)
- **Priority:** P0
- **Status:** Not Started

**Description:** Apply max_items, cursor and hard caps to all variable-size output collections.

**Expected outcome:** Responses are predictable and expose truthful truncation and continuation state.

**Stories:**

- [US07.02.01](#id-us07-02-01) — Bound and paginate every collection (P0, 6d)

<a id="id-f07-03"></a>
### F07.03 — Token budget and usage reporting

- **Parent Epic:** [E07](#id-e07)
- **Priority:** P1
- **Status:** Not Started

**Description:** Estimate and enforce output budgets before upstream work, then report measured and estimated usage transparently.

**Expected outcome:** Clients can plan context consumption and avoid BudgetExceeded failures after spending quota.

**Stories:**

- [US07.03.01](#id-us07-03-01) — Preflight output token budgets (P1, 5d)
- [US07.03.02](#id-us07-03-02) — Report token usage with method and uncertainty (P1, 4d)

<a id="id-f07-04"></a>
### F07.04 — Structured result and error contract

- **Parent Epic:** [E07](#id-e07)
- **Priority:** P0
- **Status:** Not Started

**Description:** Return protocol-level structured success, partial and error results with a complete actionable error taxonomy.

**Expected outcome:** Clients no longer parse JSON text or mistake domain failures for successful MCP calls.

**Stories:**

- [US07.04.01](#id-us07-04-01) — Return structured MCP results (P0, 4d)
- [US07.04.02](#id-us07-04-02) — Complete actionable error taxonomy (P0, 4d)

## 4. User Stories and Subtasks

<a id="id-us07-01-01"></a>
### US07.01.01 — Define meaningful view presets

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.01](#id-f07-01) | P1 | 5 | 40 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As a client developer, I want stable view presets so that summary, standard and full produce predictable levels of detail.

**Acceptance criteria:**

- [ ] A response-contract ADR defines required fields and collection limits for each preset by tool family.
- [ ] No two view values produce identical payloads unless the response explicitly reports that the source has no additional detail.
- [ ] full remains subject to documented hard caps and include_raw is a separate privileged flag rather than an unlimited synonym.
- [ ] Snapshot tests cover effective_view, omitted_fields and deterministic payload shape for all twelve existing tools.

**Dependencies:** [US07.02.01](#id-us07-02-01)

**Labels:** `api-contract` `view` `P1`

**Source findings:**

- View semantics are inconsistent; some tiers are identical, full is sometimes uncapped and exchange full is still capped at 100.

**Subtasks:**

<a id="id-st07-01-01-01"></a>
- [ ] **ST07.01.01.01 — Write view-semantics ADR and matrix**
  - Type: design
  - Estimate: 12 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Approved per-tool-family view matrix and migration notes.
  - Status: Not Started
<a id="id-st07-01-01-02"></a>
- [ ] **ST07.01.01.02 — Refactor view projection and metadata**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Shared view preset implementation with effective_view reporting.
  - Status: Not Started
<a id="id-st07-01-01-03"></a>
- [ ] **ST07.01.01.03 — Add view snapshot matrix**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: All-tool snapshot tests for three presets.
  - Status: Not Started

<a id="id-us07-01-02"></a>
### US07.01.02 — Implement real field projection

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.01](#id-f07-01) | P1 | 4 | 28 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As a client developer, I want an allowlisted fields parameter so that only requested output fields consume context.

**Acceptance criteria:**

- [ ] fields is validated against the selected tool/view output schema and unknown or unavailable fields return InvalidQuery with valid alternatives.
- [ ] Projection is applied to the final structured payload and is not merely included in the cache key.
- [ ] Required identity, pagination, error and provenance metadata cannot be projected away.
- [ ] Search-symbol removes stale field names such as count/result/is_exact_match unless implemented and adds tests proving projection.

**Dependencies:** [US07.01.01](#id-us07-01-01)

**Labels:** `api-contract` `projection` `P1`

**Source findings:**

- search-symbol validates fields but discards them; its allowlist contains stale/nonexistent names and misses actual fields.

**Subtasks:**

<a id="id-st07-01-02-01"></a>
- [ ] **ST07.01.02.01 — Generate projection allowlists from schemas**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Reusable field-selection validator/projector.
  - Status: Not Started
<a id="id-st07-01-02-02"></a>
- [ ] **ST07.01.02.02 — Repair search-symbol projection contract**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Accurate search fields and applied projection.
  - Status: Not Started
<a id="id-st07-01-02-03"></a>
- [ ] **ST07.01.02.03 — Test required-field preservation**
  - Type: test
  - Estimate: 6 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Projection allow/deny and payload tests.
  - Status: Not Started

<a id="id-us07-02-01"></a>
### US07.02.01 — Bound and paginate every collection

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.02](#id-f07-02) | P0 | 6 | 48 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As a client developer, I want deterministic limits and cursors so that large peers, calendar, symbols, news and history results never overflow a context window.

**Acceptance criteria:**

- [ ] All collection tools accept max_items within per-tool soft and hard limits and apply the limit to returned data, not only the cache key.
- [ ] Responses include available_count when known, returned_count, truncated, omitted_count when known and next_cursor.
- [ ] Calendar and peers preserve the provider total rather than replacing it with the post-truncation count.
- [ ] Cursors are opaque, versioned, query-bound and deterministic; invalid/expired cursors return InvalidQuery.
- [ ] Boundary tests cover zero results, exact limit, one-over-limit and full-view hard caps.

**Dependencies:** —

**Labels:** `api-contract` `pagination` `bounds` `P0`

**Source findings:**

- search-symbol accepts but never applies limit.
- Calendar and peers can report truncated count as total and expose no cursor/truncated indicator.
- Full responses need deterministic hard caps.

**Subtasks:**

<a id="id-st07-02-01-01"></a>
- [ ] **ST07.02.01.01 — Define shared cursor and bounds contract**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Versioned cursor, limit and truncation specification.
  - Status: Not Started
<a id="id-st07-02-01-02"></a>
- [ ] **ST07.02.01.02 — Apply pagination middleware/helpers to collections**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Consistent pagination across existing tools.
  - Status: Not Started
<a id="id-st07-02-01-03"></a>
- [ ] **ST07.02.01.03 — Add cross-tool boundary tests**
  - Type: test
  - Estimate: 14 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Limit/cursor/count/truncation contract suite.
  - Status: Not Started

<a id="id-us07-03-01"></a>
### US07.03.01 — Preflight output token budgets

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.03](#id-f07-03) | P1 | 5 | 36 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** LLM orchestrator

**User story:** As an orchestrator, I want max_output_tokens enforced before provider work so that a call cannot spend quota and then fail solely on response size.

**Acceptance criteria:**

- [ ] Every variable-output tool accepts max_output_tokens within service policy and computes a conservative preflight plan from view, fields and max_items.
- [ ] If the requested shape cannot fit, the server either deterministically reduces optional items/fields or returns BudgetExceeded before upstream calls, according to explicit policy.
- [ ] BudgetExceeded recommends smaller view, fewer fields/items or pagination and never recommends a larger view.
- [ ] Tests assert zero provider calls on preflight rejection and stable reduction order when auto-fit is enabled.

**Dependencies:** [US07.01.01](#id-us07-01-01), [US07.01.02](#id-us07-01-02), [US07.02.01](#id-us07-02-01)

**Labels:** `tokens` `budget` `api-contract` `P1`

**Source findings:**

- BudgetExceeded is currently evaluated after upstream work and can suggest a larger view or fields.

**Subtasks:**

<a id="id-st07-03-01-01"></a>
- [ ] **ST07.03.01.01 — Design token preflight planner**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Budget policy and deterministic auto-fit order.
  - Status: Not Started
<a id="id-st07-03-01-02"></a>
- [ ] **ST07.03.01.02 — Enforce preflight in wrapped tools**
  - Type: implementation
  - Estimate: 18 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Shared max_output_tokens admission middleware.
  - Status: Not Started
<a id="id-st07-03-01-03"></a>
- [ ] **ST07.03.01.03 — Assert no-call rejection and fit behavior**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Provider-call-count and budget boundary tests.
  - Status: Not Started

<a id="id-us07-03-02"></a>
### US07.03.02 — Report token usage with method and uncertainty

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.03](#id-f07-03) | P1 | 4 | 32 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** LLM orchestrator

**User story:** As an orchestrator, I want transparent usage estimates so that I can budget model context without treating chars/4 as exact.

**Acceptance criteria:**

- [ ] The final response reports estimate_method, estimated_tokens, uncertainty_percent, budget_tokens and measured character/byte counts after all metadata is serialized.
- [ ] Reporting separates payload tokens, MCP wire/envelope estimate and tools/list schema estimate.
- [ ] An optional model/tokenizer hint uses a supported tokenizer when available and falls back to a named conservative heuristic.
- [ ] A representative corpus measures estimation error by payload type and fails CI if the documented error envelope is exceeded.

**Dependencies:** [US07.03.01](#id-us07-03-01)

**Labels:** `tokens` `reporting` `P1`

**Source findings:**

- approx_tokens uses chars/4, can be materially inaccurate and is calculated before final metadata.
- Token reporting should distinguish payload, wire and schema costs and expose its method/uncertainty.

**Subtasks:**

<a id="id-st07-03-02-01"></a>
- [ ] **ST07.03.02.01 — Implement final-payload usage estimator**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Method-aware token/character/byte reporting component.
  - Status: Not Started
<a id="id-st07-03-02-02"></a>
- [ ] **ST07.03.02.02 — Calibrate estimates on response corpus**
  - Type: evaluation
  - Estimate: 12 hours
  - Suggested owner role: Context engineer
  - Deliverable/evidence: Error-distribution report and CI thresholds.
  - Status: Not Started
<a id="id-st07-03-02-03"></a>
- [ ] **ST07.03.02.03 — Separate payload/wire/schema reporting**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Expanded usage metadata schema.
  - Status: Not Started

<a id="id-us07-04-01"></a>
### US07.04.01 — Return structured MCP results

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.04](#id-f07-04) | P0 | 4 | 32 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As a client developer, I want native structured content and output schemas so that I do not parse JSON embedded in text.

**Acceptance criteria:**

- [ ] Wrapped tools enable structured content and publish output schemas for common envelope and tool-specific data.
- [ ] Successful calls populate structuredContent; an optional compact text summary is secondary and semantically equivalent.
- [ ] Domain envelopes with is_success false set protocol isError true except explicitly modeled PartialSuccess.
- [ ] Protocol integration tests exercise initialize, tools/list and tools/call and assert both schema and isError behavior.

**Dependencies:** —

**Labels:** `mcp` `structured-output` `P0`

**Source findings:**

- Wrapped tools leave UseStructuredContent false, so object results become JSON text.
- NotFound, PremiumRequired and Timeout envelopes can appear as protocol successes; only BudgetExceeded is marked as an MCP error.

**Subtasks:**

<a id="id-st07-04-01-01"></a>
- [ ] **ST07.04.01.01 — Enable structured tool outputs and schemas**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: SDK configuration and generated output schemas.
  - Status: Not Started
<a id="id-st07-04-01-02"></a>
- [ ] **ST07.04.01.02 — Map domain failure to protocol isError**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Wrapper error-state mapping.
  - Status: Not Started
<a id="id-st07-04-01-03"></a>
- [ ] **ST07.04.01.03 — Run protocol-client integration tests**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Initialize/list/call structured-result test suite.
  - Status: Not Started

<a id="id-us07-04-02"></a>
### US07.04.02 — Complete actionable error taxonomy

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F07.04](#id-f07-04) | P0 | 4 | 30 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** End user and client developer

**User story:** As a caller, I want stable error codes and recovery guidance so that I know whether to retry, authenticate, change inputs or reduce scope.

**Acceptance criteria:**

- [ ] The taxonomy includes NotFound, InvalidQuery, RateLimited, Unauthenticated, PermissionDenied, PremiumRequired, ConfigurationError, Timeout, CircuitOpen, ServiceUnavailable, InvalidResponse, Cancelled, BudgetExceeded, PartialSuccess and Unknown.
- [ ] Each error returns code, safe message, retryable, retry_after when known, correlation_id, docs_url and bounded suggested_actions.
- [ ] Messages never expose API keys, raw provider bodies or sensitive configuration and distinguish missing data from entitlement denial.
- [ ] Provider/status mappings and protocol isError behavior have table-driven tests for every code.

**Dependencies:** [US07.04.01](#id-us07-04-01)

**Labels:** `errors` `ux` `mcp` `P0`

**Source findings:**

- Current error types omit RateLimited, Unauthenticated, PermissionDenied, ConfigurationError, Cancelled, CircuitOpen and PartialSuccess.
- Existing failures are not consistently friendly, actionable or protocol-visible.

**Subtasks:**

<a id="id-st07-04-02-01"></a>
- [ ] **ST07.04.02.01 — Define error catalogue and provider mappings**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Versioned error registry with recovery metadata.
  - Status: Not Started
<a id="id-st07-04-02-02"></a>
- [ ] **ST07.04.02.02 — Implement safe error factory**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Central redacted actionable error builder.
  - Status: Not Started
<a id="id-st07-04-02-03"></a>
- [ ] **ST07.04.02.03 — Add exhaustive status/error tests**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Table-driven taxonomy and protocol suite.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST07.01.01.01](#id-st07-01-01-01) | [US07.01.01](#id-us07-01-01) | P1 | Write view-semantics ADR and matrix | design | 12 | API architect | Approved per-tool-family view matrix and migration notes. | Not Started |
| [ST07.01.01.02](#id-st07-01-01-02) | [US07.01.01](#id-us07-01-01) | P1 | Refactor view projection and metadata | implementation | 18 | Backend engineer | Shared view preset implementation with effective_view reporting. | Not Started |
| [ST07.01.01.03](#id-st07-01-01-03) | [US07.01.01](#id-us07-01-01) | P1 | Add view snapshot matrix | test | 10 | QA automation engineer | All-tool snapshot tests for three presets. | Not Started |
| [ST07.01.02.01](#id-st07-01-02-01) | [US07.01.02](#id-us07-01-02) | P1 | Generate projection allowlists from schemas | implementation | 12 | Backend engineer | Reusable field-selection validator/projector. | Not Started |
| [ST07.01.02.02](#id-st07-01-02-02) | [US07.01.02](#id-us07-01-02) | P1 | Repair search-symbol projection contract | implementation | 10 | Backend engineer | Accurate search fields and applied projection. | Not Started |
| [ST07.01.02.03](#id-st07-01-02-03) | [US07.01.02](#id-us07-01-02) | P1 | Test required-field preservation | test | 6 | QA automation engineer | Projection allow/deny and payload tests. | Not Started |
| [ST07.02.01.01](#id-st07-02-01-01) | [US07.02.01](#id-us07-02-01) | P0 | Define shared cursor and bounds contract | design | 10 | API architect | Versioned cursor, limit and truncation specification. | Not Started |
| [ST07.02.01.02](#id-st07-02-01-02) | [US07.02.01](#id-us07-02-01) | P0 | Apply pagination middleware/helpers to collections | implementation | 24 | Backend engineer | Consistent pagination across existing tools. | Not Started |
| [ST07.02.01.03](#id-st07-02-01-03) | [US07.02.01](#id-us07-02-01) | P0 | Add cross-tool boundary tests | test | 14 | QA automation engineer | Limit/cursor/count/truncation contract suite. | Not Started |
| [ST07.03.01.01](#id-st07-03-01-01) | [US07.03.01](#id-us07-03-01) | P1 | Design token preflight planner | design | 10 | Context engineer | Budget policy and deterministic auto-fit order. | Not Started |
| [ST07.03.01.02](#id-st07-03-01-02) | [US07.03.01](#id-us07-03-01) | P1 | Enforce preflight in wrapped tools | implementation | 18 | Backend engineer | Shared max_output_tokens admission middleware. | Not Started |
| [ST07.03.01.03](#id-st07-03-01-03) | [US07.03.01](#id-us07-03-01) | P1 | Assert no-call rejection and fit behavior | test | 8 | QA automation engineer | Provider-call-count and budget boundary tests. | Not Started |
| [ST07.03.02.01](#id-st07-03-02-01) | [US07.03.02](#id-us07-03-02) | P1 | Implement final-payload usage estimator | implementation | 14 | Context engineer | Method-aware token/character/byte reporting component. | Not Started |
| [ST07.03.02.02](#id-st07-03-02-02) | [US07.03.02](#id-us07-03-02) | P1 | Calibrate estimates on response corpus | evaluation | 12 | Context engineer | Error-distribution report and CI thresholds. | Not Started |
| [ST07.03.02.03](#id-st07-03-02-03) | [US07.03.02](#id-us07-03-02) | P1 | Separate payload/wire/schema reporting | implementation | 6 | Backend engineer | Expanded usage metadata schema. | Not Started |
| [ST07.04.01.01](#id-st07-04-01-01) | [US07.04.01](#id-us07-04-01) | P0 | Enable structured tool outputs and schemas | implementation | 16 | MCP engineer | SDK configuration and generated output schemas. | Not Started |
| [ST07.04.01.02](#id-st07-04-01-02) | [US07.04.01](#id-us07-04-01) | P0 | Map domain failure to protocol isError | implementation | 8 | MCP engineer | Wrapper error-state mapping. | Not Started |
| [ST07.04.01.03](#id-st07-04-01-03) | [US07.04.01](#id-us07-04-01) | P0 | Run protocol-client integration tests | test | 8 | QA automation engineer | Initialize/list/call structured-result test suite. | Not Started |
| [ST07.04.02.01](#id-st07-04-02-01) | [US07.04.02](#id-us07-04-02) | P0 | Define error catalogue and provider mappings | design | 8 | API architect | Versioned error registry with recovery metadata. | Not Started |
| [ST07.04.02.02](#id-st07-04-02-02) | [US07.04.02](#id-us07-04-02) | P0 | Implement safe error factory | implementation | 14 | Backend engineer | Central redacted actionable error builder. | Not Started |
| [ST07.04.02.03](#id-st07-04-02-03) | [US07.04.02](#id-us07-04-02) | P0 | Add exhaustive status/error tests | test | 8 | QA automation engineer | Table-driven taxonomy and protocol suite. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E07 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A-02 | A. Feature Enhancements | Improve the summary/standard/full view design with fields, limits, pagination, truncation metadata, provenance, and token budgets. | [US07.01.01](#id-us07-01-01), [US07.01.02](#id-us07-01-02), [US07.02.01](#id-us07-02-01), [US07.03.01](#id-us07-03-01), [US07.03.02](#id-us07-03-02), [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01) | [E07](#id-e07) | P0 | Covered | Explicit review question A2. |
| C-01 | C. User Experience | Make errors friendly, actionable, protocol-correct, and expand taxonomy for rate limits, authentication, authorization, configuration, cancellation, circuit-open, and partial success. | [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01), [US04.01.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-02), [US07.04.01](#id-us07-04-01), [US07.04.02](#id-us07-04-02) | [E04](./E04-resilience-quota-governance-and-cache-correctness.md#id-e04) | P0 | Covered | Explicit review question C1. |
| C-05 | C. User Experience | Replace char/4-only approx_tokens with method-labelled, uncertainty-aware, granular payload/wire/schema token reporting and output budgets. | [US07.03.01](#id-us07-03-01), [US07.03.02](#id-us07-03-02) | [E07](#id-e07) | P1 | Covered | Explicit review question C5. |
| G-03 | G. Context Engineering | Make next_actions respect context-window and output-token budgets, visited tools, quota cost, cache state, and likely information gain. | [US08.01.01](./E08-intelligent-discovery-and-context-engineering.md#id-us08-01-01), [US07.03.01](#id-us07-03-01) | [E08](./E08-intelligent-discovery-and-context-engineering.md#id-e08) | P1 | Covered | Explicit review question G3. |
| R-04 | Repository finding | Implement or remove search-symbol fields projection and enforce the accepted limit; correct the stale field allowlist. | [US05.01.01](./E05-financial-and-symbol-data-correctness.md#id-us05-01-01), [US07.01.02](#id-us07-01-02) | [E05](./E05-financial-and-symbol-data-correctness.md#id-e05) | P0 | Covered | Code-level core-functionality finding. |
| R-08 | Repository finding | Translate unsuccessful inner envelopes into MCP protocol errors with structured content and stable output/error schemas. | [US01.02.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-01), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US07.04.01](#id-us07-04-01) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P0 | Covered | Code-level P0 interoperability finding. |
| R-13 | Repository finding | Stop reporting truncated peer/calendar collections as total counts and expose returned/available/truncated/cursor metadata. | [US07.02.01](#id-us07-02-01) | [E07](#id-e07) | P0 | Covered | Code-level response-contract finding. |
| RF-113 | Code-review detail | A2 - view summary/standard/full semantics are inconsistent and need projection, caps and raw-data separation | [US07.01.01](#id-us07-01-01), [US07.01.02](#id-us07-01-02), [US07.02.01](#id-us07-02-01) | [E07](#id-e07) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-118 | Code-review detail | C1 - friendly actionable error taxonomy and protocol-level failures | [US07.04.01](#id-us07-04-01), [US07.04.02](#id-us07-04-02) | [E07](#id-e07) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-122 | Code-review detail | C5 - accurate granular approx_tokens and output budget reporting | [US07.03.01](#id-us07-03-01), [US07.03.02](#id-us07-03-02) | [E07](#id-e07) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-130 | Code-review detail | Cross-cutting - every financial payload needs provenance, units, freshness, completeness, cache and quota metadata | [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01), [US07.03.02](#id-us07-03-02) | [E06](./E06-financial-data-coverage-and-semantics.md#id-e06) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-131 | Code-review detail | Observed bug - search-symbol fields and limit are accepted/cache-keyed but not applied | [US07.01.02](#id-us07-01-02), [US07.02.01](#id-us07-02-01) | [E07](#id-e07) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-137 | Code-review detail | Observed bug - calendar/peers truncation can misstate totals and lacks cursor metadata | [US07.02.01](#id-us07-02-01) | [E07](#id-e07) | P0 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 2. M0B — Hardened Service

- **Priority:** P0
- **Indicative window:** Weeks 3–10
- **Primary epics:** E02, E04, E05, E07, E15
- **Goal:** Complete hosted security, quota controls, core data correctness, response bounds, and release gates.
- **Entry criteria:** M0A released; authentication/tenant model approved; financial semantics reviewed.
- **Exit criteria:** Hosted trust boundary is enforced; quota/retry/cache behavior is safe; search and calculations are correct; P0 release suite and supply-chain gates pass.
- **Delivery note:** Use feature flags and staged deployment; local-only scope can finish sooner.

### 3. M1 — Contract & Core Data

- **Priority:** P1
- **Indicative window:** Weeks 8–18
- **Primary epics:** E06–E09
- **Goal:** Stabilize output contracts and add the highest-value financial workflows.
- **Entry criteria:** M0B exit gates met; output budget and provider-entitlement policies approved.
- **Exit criteria:** View/fields/pagination/provenance shipped; OHLCV, indicators, corporate actions, statements, filings, and bounded batches available.
- **Delivery note:** Sequence premium-dependent endpoints behind capability flags.

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E07 | Epic | — | P0 | Bounded Response and Token Contract | See [E07](#id-e07) | See [E07](#id-e07) | — | finnhub-mcp; epic | — | Not Started |
| F07.01 | Feature | [E07](#id-e07) | P1 | View presets and field projection | See [F07.01](#id-f07-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e07 | — | Not Started |
| F07.02 | Feature | [E07](#id-e07) | P0 | Pagination and deterministic bounds | See [F07.02](#id-f07-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e07 | — | Not Started |
| F07.03 | Feature | [E07](#id-e07) | P1 | Token budget and usage reporting | See [F07.03](#id-f07-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e07 | — | Not Started |
| F07.04 | Feature | [E07](#id-e07) | P0 | Structured result and error contract | See [F07.04](#id-f07-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e07 | — | Not Started |
| US07.01.01 | Story | [F07.01](#id-f07-01) | P1 | Define meaningful view presets | See [US07.01.01](#id-us07-01-01) | See [US07.01.01](#id-us07-01-01) | 5d | api-contract; view; P1 | [US07.02.01](#id-us07-02-01) | Not Started |
| US07.01.02 | Story | [F07.01](#id-f07-01) | P1 | Implement real field projection | See [US07.01.02](#id-us07-01-02) | See [US07.01.02](#id-us07-01-02) | 4d | api-contract; projection; P1 | [US07.01.01](#id-us07-01-01) | Not Started |
| US07.02.01 | Story | [F07.02](#id-f07-02) | P0 | Bound and paginate every collection | See [US07.02.01](#id-us07-02-01) | See [US07.02.01](#id-us07-02-01) | 6d | api-contract; pagination; bounds; P0 | — | Not Started |
| US07.03.01 | Story | [F07.03](#id-f07-03) | P1 | Preflight output token budgets | See [US07.03.01](#id-us07-03-01) | See [US07.03.01](#id-us07-03-01) | 5d | tokens; budget; api-contract; P1 | [US07.01.01](#id-us07-01-01), [US07.01.02](#id-us07-01-02), [US07.02.01](#id-us07-02-01) | Not Started |
| US07.03.02 | Story | [F07.03](#id-f07-03) | P1 | Report token usage with method and uncertainty | See [US07.03.02](#id-us07-03-02) | See [US07.03.02](#id-us07-03-02) | 4d | tokens; reporting; P1 | [US07.03.01](#id-us07-03-01) | Not Started |
| US07.04.01 | Story | [F07.04](#id-f07-04) | P0 | Return structured MCP results | See [US07.04.01](#id-us07-04-01) | See [US07.04.01](#id-us07-04-01) | 4d | mcp; structured-output; P0 | — | Not Started |
| US07.04.02 | Story | [F07.04](#id-f07-04) | P0 | Complete actionable error taxonomy | See [US07.04.02](#id-us07-04-02) | See [US07.04.02](#id-us07-04-02) | 4d | errors; ux; mcp; P0 | [US07.04.01](#id-us07-04-01) | Not Started |
| ST07.01.01.01 | Sub-task | [US07.01.01](#id-us07-01-01) | P1 | Write view-semantics ADR and matrix | See [ST07.01.01.01](#id-st07-01-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; design | — | Not Started |
| ST07.01.01.02 | Sub-task | [US07.01.01](#id-us07-01-01) | P1 | Refactor view projection and metadata | See [ST07.01.01.02](#id-st07-01-01-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST07.01.01.03 | Sub-task | [US07.01.01](#id-us07-01-01) | P1 | Add view snapshot matrix | See [ST07.01.01.03](#id-st07-01-01-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |
| ST07.01.02.01 | Sub-task | [US07.01.02](#id-us07-01-02) | P1 | Generate projection allowlists from schemas | See [ST07.01.02.01](#id-st07-01-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST07.01.02.02 | Sub-task | [US07.01.02](#id-us07-01-02) | P1 | Repair search-symbol projection contract | See [ST07.01.02.02](#id-st07-01-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST07.01.02.03 | Sub-task | [US07.01.02](#id-us07-01-02) | P1 | Test required-field preservation | See [ST07.01.02.03](#id-st07-01-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; test | — | Not Started |
| ST07.02.01.01 | Sub-task | [US07.02.01](#id-us07-02-01) | P0 | Define shared cursor and bounds contract | See [ST07.02.01.01](#id-st07-02-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST07.02.01.02 | Sub-task | [US07.02.01](#id-us07-02-01) | P0 | Apply pagination middleware/helpers to collections | See [ST07.02.01.02](#id-st07-02-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST07.02.01.03 | Sub-task | [US07.02.01](#id-us07-02-01) | P0 | Add cross-tool boundary tests | See [ST07.02.01.03](#id-st07-02-01-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; test | — | Not Started |
| ST07.03.01.01 | Sub-task | [US07.03.01](#id-us07-03-01) | P1 | Design token preflight planner | See [ST07.03.01.01](#id-st07-03-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST07.03.01.02 | Sub-task | [US07.03.01](#id-us07-03-01) | P1 | Enforce preflight in wrapped tools | See [ST07.03.01.02](#id-st07-03-01-02) | Not applicable; see detail or parent section | 18h | finnhub-mcp; implementation | — | Not Started |
| ST07.03.01.03 | Sub-task | [US07.03.01](#id-us07-03-01) | P1 | Assert no-call rejection and fit behavior | See [ST07.03.01.03](#id-st07-03-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST07.03.02.01 | Sub-task | [US07.03.02](#id-us07-03-02) | P1 | Implement final-payload usage estimator | See [ST07.03.02.01](#id-st07-03-02-01) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST07.03.02.02 | Sub-task | [US07.03.02](#id-us07-03-02) | P1 | Calibrate estimates on response corpus | See [ST07.03.02.02](#id-st07-03-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; evaluation | — | Not Started |
| ST07.03.02.03 | Sub-task | [US07.03.02](#id-us07-03-02) | P1 | Separate payload/wire/schema reporting | See [ST07.03.02.03](#id-st07-03-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST07.04.01.01 | Sub-task | [US07.04.01](#id-us07-04-01) | P0 | Enable structured tool outputs and schemas | See [ST07.04.01.01](#id-st07-04-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST07.04.01.02 | Sub-task | [US07.04.01](#id-us07-04-01) | P0 | Map domain failure to protocol isError | See [ST07.04.01.02](#id-st07-04-01-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST07.04.01.03 | Sub-task | [US07.04.01](#id-us07-04-01) | P0 | Run protocol-client integration tests | See [ST07.04.01.03](#id-st07-04-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST07.04.02.01 | Sub-task | [US07.04.02](#id-us07-04-02) | P0 | Define error catalogue and provider mappings | See [ST07.04.02.01](#id-st07-04-02-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST07.04.02.02 | Sub-task | [US07.04.02](#id-us07-04-02) | P0 | Implement safe error factory | See [ST07.04.02.02](#id-st07-04-02-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST07.04.02.03 | Sub-task | [US07.04.02](#id-us07-04-02) | P0 | Add exhaustive status/error tests | See [ST07.04.02.03](#id-st07-04-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |

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

- [ ] Create E07 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 4 Features under E07.
- [ ] Create all 7 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 21 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 13 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 33 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

