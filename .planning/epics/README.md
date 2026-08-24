---
project: finnhub-mcp
document_type: epic-backlog-index
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
epic_range: E01-E15
---

# Finnhub MCP Epic Backlog - E01 to E15

This package splits the comprehensive backlog into 15 self-contained Markdown specifications. A coding agent should start here, select the next Epic from the roadmap, and then work inside that Epic file in dependency order.

## Package Totals

| Epics | Features | User Stories | Subtasks | Story Days | Subtask Hours | Traceability Rows | Roadmap Milestones |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 15 | 69 | 128 | 390 | 568.5 | 4003 | 168 | 6 |

## Known Dependency Cycle Requiring Backlog Decision

The source backlog contains one unresolved circular blocker: [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01) → [US04.04.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-04-02) → [US02.04.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-04-01). It has been preserved rather than silently altered. Before an agent automates implementation sequencing, the backlog owner should designate one edge as the true blocker, reclassify one edge as coordination, or introduce a shared foundation Story.

| Dependency Edges | Same-Epic | Cross-Epic | Dangling IDs | Known Circular Blockers |
| --- | --- | --- | --- | --- |
| 214 | 113 | 101 | 0 | 1 |

## Epic Files

| Epic | Priority | Title | Features | Stories | Subtasks | Story Days | Subtask Hours | Owned Trace | Relevant Trace | Roadmap Rows | File |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E01 | P0 | MCP Transport and Protocol Integrity | 3 | 7 | 22 | 16.5 | 122 | 12 | 12 | 1 | [Open](./E01-mcp-transport-and-protocol-integrity.md) |
| E02 | P0 | Hosted Security, Authorization, and Tenant Isolation | 5 | 10 | 31 | 33.5 | 218 | 15 | 16 | 1 | [Open](./E02-hosted-security-authorization-and-tenant-isolation.md) |
| E03 | P0 | Credential Lifecycle and Secret Containment | 3 | 6 | 18 | 16.5 | 116 | 9 | 9 | 1 | [Open](./E03-credential-lifecycle-and-secret-containment.md) |
| E04 | P0 | Resilience, Quota Governance, and Cache Correctness | 4 | 10 | 33 | 36.5 | 231 | 14 | 19 | 2 | [Open](./E04-resilience-quota-governance-and-cache-correctness.md) |
| E05 | P0 | Financial and Symbol Data Correctness | 5 | 11 | 34 | 32.5 | 205 | 18 | 20 | 1 | [Open](./E05-financial-and-symbol-data-correctness.md) |
| E06 | P1 | Financial Data Coverage and Semantics | 9 | 15 | 44 | 74 | 551 | 7 | 11 | 1 | [Open](./E06-financial-data-coverage-and-semantics.md) |
| E07 | P0 | Bounded Response and Token Contract | 4 | 7 | 21 | 32 | 246 | 8 | 13 | 2 | [Open](./E07-bounded-response-and-token-contract.md) |
| E08 | P1 | Intelligent Discovery and Context Engineering | 5 | 11 | 32 | 57 | 407 | 18 | 21 | 2 | [Open](./E08-intelligent-discovery-and-context-engineering.md) |
| E09 | P1 | Real-Time Streaming and Batch Efficiency | 3 | 4 | 12 | 30 | 216 | 4 | 6 | 2 | [Open](./E09-real-time-streaming-and-batch-efficiency.md) |
| E10 | P1 | Service Operations, Resources and Extensibility | 5 | 6 | 18 | 37 | 266 | 13 | 14 | 2 | [Open](./E10-service-operations-resources-and-extensibility.md) |
| E11 | P1 | User Experience, Performance and Quota Control | 5 | 6 | 20 | 43 | 318 | 6 | 11 | 3 | [Open](./E11-user-experience-performance-and-quota-control.md) |
| E12 | P0 | Documentation Integrity and Developer Enablement | 5 | 10 | 30 | 42 | 288 | 20 | 21 | 1 | [Open](./E12-documentation-integrity-and-developer-enablement.md) |
| E13 | P1 | Showcase Website and Safe Interactive Experience | 5 | 10 | 30 | 56 | 382 | 11 | 19 | 1 | [Open](./E13-showcase-website-and-safe-interactive-experience.md) |
| E14 | P1 | Ecosystem Positioning, Adoption, and Community | 3 | 6 | 18 | 22 | 145 | 6 | 7 | 2 | [Open](./E14-ecosystem-positioning-adoption-and-community.md) |
| E15 | P0 | SDK Modernization and Software Supply-Chain Assurance | 5 | 9 | 27 | 40 | 292 | 7 | 14 | 2 | [Open](./E15-sdk-modernization-and-software-supply-chain-assurance.md) |

## Recommended Execution Sequence

### 1. M0A — Critical Stabilization

- **Priority:** P0
- **Window:** Weeks 1–3
- **Primary Epics:** E01, E03, E04
- **Goal:** Patch the highest-risk protocol, secret-containment, and failure-signaling defects.
- **Exit criteria:** Real clients reach the SDK endpoint; simulated routes are gone; redirect leakage is blocked; domain failures set protocol errors; regression tests cover both transports.

### 2. M0B — Hardened Service

- **Priority:** P0
- **Window:** Weeks 3–10
- **Primary Epics:** E02, E04, E05, E07, E15
- **Goal:** Complete hosted security, quota controls, core data correctness, response bounds, and release gates.
- **Exit criteria:** Hosted trust boundary is enforced; quota/retry/cache behavior is safe; search and calculations are correct; P0 release suite and supply-chain gates pass.

### 3. M1 — Contract & Core Data

- **Priority:** P1
- **Window:** Weeks 8–18
- **Primary Epics:** E06–E09
- **Goal:** Stabilize output contracts and add the highest-value financial workflows.
- **Exit criteria:** View/fields/pagination/provenance shipped; OHLCV, indicators, corporate actions, statements, filings, and bounded batches available.

### 4. M2 — Operability & Context

- **Priority:** P1
- **Window:** Weeks 12–22
- **Primary Epics:** E08, E10–E12, E14, E15
- **Goal:** Improve discovery quality, observability, documentation, and delivery controls.
- **Exit criteria:** Golden intent evals run in CI; metrics/health are live; API docs are generated; release and supply-chain controls are enforced.

### 5. M3 — Showcase & Ecosystem

- **Priority:** P1/P2
- **Window:** Weeks 18–28
- **Primary Epics:** E11, E13, E14
- **Goal:** Create a safe product showcase and evidence-based differentiation story.
- **Exit criteria:** Fixture playground is live; hardened demo is bounded; comparison/positioning is published; contributor path is measurable.

### 6. M4 — Scale & Streaming

- **Priority:** P2
- **Window:** Weeks 24–36
- **Primary Epics:** E09–E11
- **Goal:** Add upstream WebSocket ingestion, distributed controls, and extension mechanisms.
- **Exit criteria:** Reconnect/backpressure/gap handling verified; distributed limiter/cache available; extension contract versioned.

## Package-Level Agent Checklist

- [ ] Read the relevant Epic file before creating tracker items or changing code.
- [ ] Create Epics, Features, Stories, and Subtasks using the stable IDs supplied.
- [ ] Resolve the US02.04.01 ↔ US04.04.02 circular blocker before automated implementation sequencing.
- [ ] Resolve cross-Epic blockers through the relative links in dependency and traceability fields.
- [ ] Keep all 168 traceability rows covered across the programme.
- [ ] Do not bypass P0 security, protocol, financial-accuracy, or release gates to start later breadth work.
- [ ] Reconcile package totals after any approved backlog change.
