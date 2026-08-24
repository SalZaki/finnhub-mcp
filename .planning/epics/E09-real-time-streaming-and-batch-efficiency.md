---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E09
title: "Real-Time Streaming and Batch Efficiency"
priority: P1
phase: "M1 — Product Expansion"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 3
  user_stories: 4
  subtasks: 12
  traceability_owned: 4
  traceability_items: 6
story_estimate_days: 30
subtask_estimate_hours: 216
---

<a id="id-e09"></a>
# E09 — Real-Time Streaming and Batch Efficiency

This is the self-contained coding-agent backlog for E09. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E09 | P1 | 3 | 4 | 12 | 30 | 216 | M1 — Product Expansion | Not Started |

## 2. Epic Definition

**Objective:** Add resilient upstream real-time ingestion and safe multi-symbol operations without inventing a non-standard MCP transport.

**Business value:** Supports monitoring and comparison workflows with lower client round trips while protecting upstream quota and server memory.

**Exit criteria:**

- [ ] Finnhub WebSocket ingestion reconnects safely, applies backpressure and exposes gap/completeness state.
- [ ] Streaming is surfaced through standard MCP capabilities or a bounded snapshot resource, with a separately secured browser channel.
- [ ] Batch calls are ordered, bounded, partially fault tolerant and quota-reserved.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F09.01](#id-f09-01) | P2 | Resilient Finnhub WebSocket ingestion | 1 | 10 | Not Started |
| [F09.02](#id-f09-02) | P2 | Standards-based real-time exposure | 1 | 7 | Not Started |
| [F09.03](#id-f09-03) | P1 | Bounded batch and comparison calls | 2 | 13 | Not Started |

<a id="id-f09-01"></a>
### F09.01 — Resilient Finnhub WebSocket ingestion

- **Parent Epic:** [E09](#id-e09)
- **Priority:** P2
- **Status:** Not Started

**Description:** Share upstream connections, manage subscriptions and handle reconnect, gaps, duplicates and backpressure.

**Expected outcome:** The service maintains current trade state without one upstream socket per downstream caller.

**Stories:**

- [US09.01.01](#id-us09-01-01) — Operate shared upstream trade streams (P2, 10d)

<a id="id-f09-02"></a>
### F09.02 — Standards-based real-time exposure

- **Parent Epic:** [E09](#id-e09)
- **Priority:** P2
- **Status:** Not Started

**Description:** Expose current and streaming data through supported MCP mechanisms and a separately secured UI stream.

**Expected outcome:** Real-time capability remains interoperable with MCP clients and safe for browsers.

**Stories:**

- [US09.02.01](#id-us09-02-01) — Expose real-time data without a custom MCP transport (P2, 7d)

<a id="id-f09-03"></a>
### F09.03 — Bounded batch and comparison calls

- **Parent Epic:** [E09](#id-e09)
- **Priority:** P1
- **Status:** Not Started

**Description:** Support multiple symbols with deterministic order, reservation, concurrency limits and partial results.

**Expected outcome:** Clients reduce MCP round trips without assuming batch calls reduce Finnhub quota.

**Stories:**

- [US09.03.01](#id-us09-03-01) — Execute safe multi-symbol batches (P1, 7d)
- [US09.03.02](#id-us09-03-02) — Add focused quote and comparison batches (P1, 6d)

## 4. User Stories and Subtasks

<a id="id-us09-01-01"></a>
### US09.01.01 — Operate shared upstream trade streams

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F09.01](#id-f09-01) | P2 | 10 | 76 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Real-time service operator

**User story:** As an operator, I want one managed Finnhub WebSocket per credential scope so that real-time subscriptions remain quota- and resource-efficient.

**Acceptance criteria:**

- [ ] A connection manager reference-counts normalized symbol subscriptions and does not open one upstream socket per downstream client.
- [ ] Reconnect uses bounded exponential backoff with jitter, resubscribes idempotently and maintains heartbeat/last-message state.
- [ ] The pipeline deduplicates trades, detects sequence/time gaps where possible and publishes completeness state.
- [ ] Bounded channels define drop/coalesce/backpressure policy and memory remains within a tested limit under a slow-consumer load test.
- [ ] Credential rotation and shutdown close sockets without leaking keys or abandoning background tasks.

**Dependencies:** [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01)

**Labels:** `websocket` `streaming` `upstream` `P2`

**Source findings:**

- Truly real-time data requires Finnhub WebSocket ingestion with one connection per key, reference-counted subscriptions, backpressure, reconnect, heartbeat, gap and dedup handling.

**Subtasks:**

<a id="id-st09-01-01-01"></a>
- [ ] **ST09.01.01.01 — Design streaming lifecycle and backpressure policy**
  - Type: design
  - Estimate: 16 hours
  - Suggested owner role: Distributed systems engineer
  - Deliverable/evidence: Connection/subscription/gap/drop state machine.
  - Status: Not Started
<a id="id-st09-01-01-02"></a>
- [ ] **ST09.01.01.02 — Implement shared Finnhub socket manager**
  - Type: implementation
  - Estimate: 40 hours
  - Suggested owner role: Distributed systems engineer
  - Deliverable/evidence: Credential-scoped connection and reference-counted subscriptions.
  - Status: Not Started
<a id="id-st09-01-01-03"></a>
- [ ] **ST09.01.01.03 — Load-test reconnect and slow consumers**
  - Type: test
  - Estimate: 20 hours
  - Suggested owner role: Performance engineer
  - Deliverable/evidence: Chaos/load report for gaps, memory and reconnection.
  - Status: Not Started

<a id="id-us09-02-01"></a>
### US09.02.01 — Expose real-time data without a custom MCP transport

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F09.02](#id-f09-02) | P2 | 7 | 48 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As an MCP client developer, I want interoperable access to recent trades and updates so that I do not depend on a proprietary WebSocket MCP transport.

**Acceptance criteria:**

- [ ] The MCP server continues to use standard Streamable HTTP/STDIO and does not advertise a non-standard WebSocket transport.
- [ ] At minimum, a bounded recent-trades/latest-state resource or tool exposes timestamped data with gap, delay and dropped-event metadata.
- [ ] If supported by the negotiated SDK/spec version, subscriptions/notifications use standard MCP resource capabilities and list/subscribe semantics.
- [ ] Any browser WebSocket or SSE channel is a separate authenticated, origin-restricted endpoint with symbol and connection bounds.
- [ ] Compatibility tests cover clients that do and do not support resource subscriptions.

**Dependencies:** [US09.01.01](#id-us09-01-01), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01)

**Labels:** `mcp` `streaming` `interoperability` `P2`

**Source findings:**

- Do not invent WebSocket as a new MCP transport; use standard Streamable HTTP and expose upstream streaming through resources/notifications or bounded snapshots.
- A web UI stream should be separate from MCP transport.

**Subtasks:**

<a id="id-st09-02-01-01"></a>
- [ ] **ST09.02.01.01 — Select supported MCP exposure mechanism**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Spec/SDK compatibility decision and fallback contract.
  - Status: Not Started
<a id="id-st09-02-01-02"></a>
- [ ] **ST09.02.01.02 — Implement bounded latest/recent-trades surface**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Resource/tool and optional standard notifications.
  - Status: Not Started
<a id="id-st09-02-01-03"></a>
- [ ] **ST09.02.01.03 — Secure and test browser stream adapter**
  - Type: security-test
  - Estimate: 14 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: Origin/auth/bounds controls and compatibility tests.
  - Status: Not Started

<a id="id-us09-03-01"></a>
### US09.03.01 — Execute safe multi-symbol batches

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F09.03](#id-f09-03) | P1 | 7 | 48 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Portfolio analyst

**User story:** As a portfolio analyst, I want a bounded multi-symbol request so that I can reduce MCP round trips while receiving per-symbol results and errors.

**Acceptance criteria:**

- [ ] The batch contract accepts 5-10 symbols by default with an absolute maximum of 20, normalizes/deduplicates inputs and preserves caller order.
- [ ] Admission reserves the estimated weighted upstream calls before execution and rejects unaffordable work without partial quota consumption.
- [ ] Execution uses configurable bounded concurrency initially set near three and reuses per-symbol canonical cache entries.
- [ ] The response contains per-symbol success/error, aggregate partial_success, returned/failed counts and bounded output metadata.
- [ ] Documentation states that batching reduces MCP latency but not Finnhub request quota.

**Dependencies:** [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01)

**Labels:** `batch` `portfolio` `quota` `P1`

**Source findings:**

- Batch queries should be bounded, ordered, deduplicated, concurrency-limited, quota-reserved and partially fault tolerant.
- Batching reduces MCP round trips, not Finnhub call count.

**Subtasks:**

<a id="id-st09-03-01-01"></a>
- [ ] **ST09.03.01.01 — Define batch admission and partial-result schema**
  - Type: design
  - Estimate: 10 hours
  - Suggested owner role: API architect
  - Deliverable/evidence: Batch bounds, order, cost reservation and error contract.
  - Status: Not Started
<a id="id-st09-03-01-02"></a>
- [ ] **ST09.03.01.02 — Implement bounded batch executor**
  - Type: implementation
  - Estimate: 26 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Reusable ordered concurrency-limited execution service.
  - Status: Not Started
<a id="id-st09-03-01-03"></a>
- [ ] **ST09.03.01.03 — Test mixed success, cache and quota states**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Batch behavior and call-count regression suite.
  - Status: Not Started

<a id="id-us09-03-02"></a>
### US09.03.02 — Add focused quote and comparison batches

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F09.03](#id-f09-03) | P1 | 6 | 44 | M1 — Product Expansion | Not Started | Unassigned |

**Persona:** Equity researcher

**User story:** As an equity researcher, I want focused batch quote and peer-comparison operations so that common portfolio tasks avoid a generic unbounded batch executor.

**Acceptance criteria:**

- [ ] Initial batch support is limited to approved read-only operations such as quote/profile/selected metrics and a compare-symbols composite.
- [ ] compare-symbols requires an allowlisted metric set, reports per-field currency/unit/as_of and never compares incompatible units silently.
- [ ] Composite calls publish estimated/actual provider-call counts and reuse existing single-symbol services rather than duplicating endpoint logic.
- [ ] Tests cover mixed cache hits, one-symbol failures, duplicate symbols, incompatible metrics and output-token truncation.

**Dependencies:** [US09.03.01](#id-us09-03-01), [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01)

**Labels:** `batch` `comparison` `tool` `P1`

**Source findings:**

- A narrow batch/compare design is preferable to a generic arbitrary-operation batch and should preserve financial comparability.

**Subtasks:**

<a id="id-st09-03-02-01"></a>
- [ ] **ST09.03.02.01 — Implement approved quote/profile/metrics batches**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Focused batch tools reusing single-symbol services.
  - Status: Not Started
<a id="id-st09-03-02-02"></a>
- [ ] **ST09.03.02.02 — Implement comparison normalization guardrails**
  - Type: implementation
  - Estimate: 14 hours
  - Suggested owner role: Financial data engineer
  - Deliverable/evidence: Unit/currency/as-of compatibility validation.
  - Status: Not Started
<a id="id-st09-03-02-03"></a>
- [ ] **ST09.03.02.03 — Add composite integration tests**
  - Type: test
  - Estimate: 10 hours
  - Suggested owner role: QA automation engineer
  - Deliverable/evidence: Comparison and batch output/token tests.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST09.01.01.01](#id-st09-01-01-01) | [US09.01.01](#id-us09-01-01) | P2 | Design streaming lifecycle and backpressure policy | design | 16 | Distributed systems engineer | Connection/subscription/gap/drop state machine. | Not Started |
| [ST09.01.01.02](#id-st09-01-01-02) | [US09.01.01](#id-us09-01-01) | P2 | Implement shared Finnhub socket manager | implementation | 40 | Distributed systems engineer | Credential-scoped connection and reference-counted subscriptions. | Not Started |
| [ST09.01.01.03](#id-st09-01-01-03) | [US09.01.01](#id-us09-01-01) | P2 | Load-test reconnect and slow consumers | test | 20 | Performance engineer | Chaos/load report for gaps, memory and reconnection. | Not Started |
| [ST09.02.01.01](#id-st09-02-01-01) | [US09.02.01](#id-us09-02-01) | P2 | Select supported MCP exposure mechanism | design | 10 | MCP engineer | Spec/SDK compatibility decision and fallback contract. | Not Started |
| [ST09.02.01.02](#id-st09-02-01-02) | [US09.02.01](#id-us09-02-01) | P2 | Implement bounded latest/recent-trades surface | implementation | 24 | MCP engineer | Resource/tool and optional standard notifications. | Not Started |
| [ST09.02.01.03](#id-st09-02-01-03) | [US09.02.01](#id-us09-02-01) | P2 | Secure and test browser stream adapter | security-test | 14 | Security engineer | Origin/auth/bounds controls and compatibility tests. | Not Started |
| [ST09.03.01.01](#id-st09-03-01-01) | [US09.03.01](#id-us09-03-01) | P1 | Define batch admission and partial-result schema | design | 10 | API architect | Batch bounds, order, cost reservation and error contract. | Not Started |
| [ST09.03.01.02](#id-st09-03-01-02) | [US09.03.01](#id-us09-03-01) | P1 | Implement bounded batch executor | implementation | 26 | Backend engineer | Reusable ordered concurrency-limited execution service. | Not Started |
| [ST09.03.01.03](#id-st09-03-01-03) | [US09.03.01](#id-us09-03-01) | P1 | Test mixed success, cache and quota states | test | 12 | QA automation engineer | Batch behavior and call-count regression suite. | Not Started |
| [ST09.03.02.01](#id-st09-03-02-01) | [US09.03.02](#id-us09-03-02) | P1 | Implement approved quote/profile/metrics batches | implementation | 20 | Backend engineer | Focused batch tools reusing single-symbol services. | Not Started |
| [ST09.03.02.02](#id-st09-03-02-02) | [US09.03.02](#id-us09-03-02) | P1 | Implement comparison normalization guardrails | implementation | 14 | Financial data engineer | Unit/currency/as-of compatibility validation. | Not Started |
| [ST09.03.02.03](#id-st09-03-02-03) | [US09.03.02](#id-us09-03-02) | P1 | Add composite integration tests | test | 10 | QA automation engineer | Comparison and batch output/token tests. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E09 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A-05 | A. Feature Enhancements | Add upstream Finnhub WebSocket ingestion where justified while retaining standard MCP Streamable HTTP transport. | [US09.01.01](#id-us09-01-01), [US09.02.01](#id-us09-02-01) | [E09](#id-e09) | P2 | Covered | Explicit review question A5. |
| A-06 | A. Feature Enhancements | Support bounded batch queries with deduplication, concurrency control, quota reservation, partial errors, and output limits. | [US09.03.01](#id-us09-03-01), [US09.03.02](#id-us09-03-02) | [E09](#id-e09) | P1 | Covered | Explicit review question A6. |
| RF-116 | Code-review detail | A5 - real-time Finnhub WebSocket support without inventing a WebSocket MCP transport | [US09.01.01](#id-us09-01-01), [US09.02.01](#id-us09-02-01) | [E09](#id-e09) | P2 | Covered | Detailed finding retained from the repository review. |
| RF-117 | Code-review detail | A6 - efficient but quota-safe multi-symbol batch queries | [US09.03.01](#id-us09-03-01), [US09.03.02](#id-us09-03-02) | [E09](#id-e09) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-121 | Code-review detail | C4 - smarter free-tier quota management, retry behavior and distributed limiting | [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01), [US09.03.01](#id-us09-03-01) | [E11](./E11-user-experience-performance-and-quota-control.md#id-e11) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-140 | Code-review detail | Website/playground safety - no browser shared key or public generic Inspector proxy; ranking lab is suitable | [US11.01.01](./E11-user-experience-performance-and-quota-control.md#id-us11-01-01), [US09.02.01](#id-us09-02-01) | [E11](./E11-user-experience-performance-and-quota-control.md#id-e11) | P2 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

### 3. M1 — Contract & Core Data

- **Priority:** P1
- **Indicative window:** Weeks 8–18
- **Primary epics:** E06–E09
- **Goal:** Stabilize output contracts and add the highest-value financial workflows.
- **Entry criteria:** M0B exit gates met; output budget and provider-entitlement policies approved.
- **Exit criteria:** View/fields/pagination/provenance shipped; OHLCV, indicators, corporate actions, statements, filings, and bounded batches available.
- **Delivery note:** Sequence premium-dependent endpoints behind capability flags.

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
| E09 | Epic | — | P1 | Real-Time Streaming and Batch Efficiency | See [E09](#id-e09) | See [E09](#id-e09) | — | finnhub-mcp; epic | — | Not Started |
| F09.01 | Feature | [E09](#id-e09) | P2 | Resilient Finnhub WebSocket ingestion | See [F09.01](#id-f09-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e09 | — | Not Started |
| F09.02 | Feature | [E09](#id-e09) | P2 | Standards-based real-time exposure | See [F09.02](#id-f09-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e09 | — | Not Started |
| F09.03 | Feature | [E09](#id-e09) | P1 | Bounded batch and comparison calls | See [F09.03](#id-f09-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e09 | — | Not Started |
| US09.01.01 | Story | [F09.01](#id-f09-01) | P2 | Operate shared upstream trade streams | See [US09.01.01](#id-us09-01-01) | See [US09.01.01](#id-us09-01-01) | 10d | websocket; streaming; upstream; P2 | [US10.05.01](./E10-service-operations-resources-and-extensibility.md#id-us10-05-01) | Not Started |
| US09.02.01 | Story | [F09.02](#id-f09-02) | P2 | Expose real-time data without a custom MCP transport | See [US09.02.01](#id-us09-02-01) | See [US09.02.01](#id-us09-02-01) | 7d | mcp; streaming; interoperability; P2 | [US09.01.01](#id-us09-01-01), [US07.02.01](./E07-bounded-response-and-token-contract.md#id-us07-02-01) | Not Started |
| US09.03.01 | Story | [F09.03](#id-f09-03) | P1 | Execute safe multi-symbol batches | See [US09.03.01](#id-us09-03-01) | See [US09.03.01](#id-us09-03-01) | 7d | batch; portfolio; quota; P1 | [US07.04.02](./E07-bounded-response-and-token-contract.md#id-us07-04-02), [US11.04.01](./E11-user-experience-performance-and-quota-control.md#id-us11-04-01), [US11.03.01](./E11-user-experience-performance-and-quota-control.md#id-us11-03-01) | Not Started |
| US09.03.02 | Story | [F09.03](#id-f09-03) | P1 | Add focused quote and comparison batches | See [US09.03.02](#id-us09-03-02) | See [US09.03.02](#id-us09-03-02) | 6d | batch; comparison; tool; P1 | [US09.03.01](#id-us09-03-01), [US06.08.01](./E06-financial-data-coverage-and-semantics.md#id-us06-08-01), [US07.03.01](./E07-bounded-response-and-token-contract.md#id-us07-03-01) | Not Started |
| ST09.01.01.01 | Sub-task | [US09.01.01](#id-us09-01-01) | P2 | Design streaming lifecycle and backpressure policy | See [ST09.01.01.01](#id-st09-01-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; design | — | Not Started |
| ST09.01.01.02 | Sub-task | [US09.01.01](#id-us09-01-01) | P2 | Implement shared Finnhub socket manager | See [ST09.01.01.02](#id-st09-01-01-02) | Not applicable; see detail or parent section | 40h | finnhub-mcp; implementation | — | Not Started |
| ST09.01.01.03 | Sub-task | [US09.01.01](#id-us09-01-01) | P2 | Load-test reconnect and slow consumers | See [ST09.01.01.03](#id-st09-01-01-03) | Not applicable; see detail or parent section | 20h | finnhub-mcp; test | — | Not Started |
| ST09.02.01.01 | Sub-task | [US09.02.01](#id-us09-02-01) | P2 | Select supported MCP exposure mechanism | See [ST09.02.01.01](#id-st09-02-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST09.02.01.02 | Sub-task | [US09.02.01](#id-us09-02-01) | P2 | Implement bounded latest/recent-trades surface | See [ST09.02.01.02](#id-st09-02-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST09.02.01.03 | Sub-task | [US09.02.01](#id-us09-02-01) | P2 | Secure and test browser stream adapter | See [ST09.02.01.03](#id-st09-02-01-03) | Not applicable; see detail or parent section | 14h | finnhub-mcp; security-test | — | Not Started |
| ST09.03.01.01 | Sub-task | [US09.03.01](#id-us09-03-01) | P1 | Define batch admission and partial-result schema | See [ST09.03.01.01](#id-st09-03-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; design | — | Not Started |
| ST09.03.01.02 | Sub-task | [US09.03.01](#id-us09-03-01) | P1 | Implement bounded batch executor | See [ST09.03.01.02](#id-st09-03-01-02) | Not applicable; see detail or parent section | 26h | finnhub-mcp; implementation | — | Not Started |
| ST09.03.01.03 | Sub-task | [US09.03.01](#id-us09-03-01) | P1 | Test mixed success, cache and quota states | See [ST09.03.01.03](#id-st09-03-01-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |
| ST09.03.02.01 | Sub-task | [US09.03.02](#id-us09-03-02) | P1 | Implement approved quote/profile/metrics batches | See [ST09.03.02.01](#id-st09-03-02-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST09.03.02.02 | Sub-task | [US09.03.02](#id-us09-03-02) | P1 | Implement comparison normalization guardrails | See [ST09.03.02.02](#id-st09-03-02-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; implementation | — | Not Started |
| ST09.03.02.03 | Sub-task | [US09.03.02](#id-us09-03-02) | P1 | Add composite integration tests | See [ST09.03.02.03](#id-st09-03-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; test | — | Not Started |

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

- [ ] Create E09 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 3 Features under E09.
- [ ] Create all 4 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 12 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 6 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 20 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

