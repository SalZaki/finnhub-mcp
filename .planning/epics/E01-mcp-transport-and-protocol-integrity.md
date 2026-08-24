---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E01
title: "MCP Transport and Protocol Integrity"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 3
  user_stories: 7
  subtasks: 22
  traceability_owned: 12
  traceability_items: 12
story_estimate_days: 16.5
subtask_estimate_hours: 122
---

<a id="id-e01"></a>
# E01 — MCP Transport and Protocol Integrity

This is the self-contained coding-agent backlog for E01. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E01 | P0 | 3 | 7 | 22 | 16.5 | 122 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Expose one standards-compliant MCP surface over Streamable HTTP and STDIO, with correct result semantics and client-level regression coverage.

**Business value:** Prevents route ambiguity, false-success tool calls, and client incompatibility while creating a safe foundation for SDK upgrades.

**Exit criteria:**

- [ ] A real MCP client completes initialize, discovery, tool, resource, and prompt flows over HTTP and STDIO.
- [ ] Only the SDK-owned /mcp route handles MCP traffic; placeholder protocol endpoints and raw body logging are removed.
- [ ] Domain failures are emitted as MCP tool errors and successful object results provide structured content.
- [ ] Protocol conformance tests run in CI against every supported transport and deployment profile.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F01.01](#id-f01-01) | P0 | Canonical MCP Endpoint Topology | 3 | 4 | Not Started |
| [F01.02](#id-f01-02) | P0 | Structured Tool Result Semantics | 2 | 5.5 | Not Started |
| [F01.03](#id-f01-03) | P0 | Protocol Conformance and SDK Lifecycle | 2 | 7 | Not Started |

<a id="id-f01-01"></a>
### F01.01 — Canonical MCP Endpoint Topology

- **Parent Epic:** [E01](#id-e01)
- **Priority:** P0
- **Status:** Not Started

**Description:** Replace ambiguous and simulated endpoints with SDK-owned Streamable HTTP routing and a protocol-clean STDIO host.

**Expected outcome:** MCP clients connect to documented endpoints without route ambiguity or non-protocol responses.

**Stories:**

- [US01.01.01](#id-us01-01-01) — Mount the SDK MCP handler at the documented route (P0, 1.5d)
- [US01.01.02](#id-us01-01-02) — Remove simulated MCP and legacy transport endpoints (P0, 1d)
- [US01.01.03](#id-us01-01-03) — Make STDIO transport protocol-clean (P0, 1.5d)

<a id="id-f01-02"></a>
### F01.02 — Structured Tool Result Semantics

- **Parent Epic:** [E01](#id-e01)
- **Priority:** P0
- **Status:** Not Started

**Description:** Emit structured successful results and protocol-visible tool errors with declared output contracts.

**Expected outcome:** Clients can distinguish success from failure without parsing an application envelope embedded in text.

**Stories:**

- [US01.02.01](#id-us01-02-01) — Enable structured content and declare output schemas (P0, 3d)
- [US01.02.02](#id-us01-02-02) — Map domain failure envelopes to MCP tool errors (P0, 2.5d)

<a id="id-f01-03"></a>
### F01.03 — Protocol Conformance and SDK Lifecycle

- **Parent Epic:** [E01](#id-e01)
- **Priority:** P0
- **Status:** Not Started

**Description:** Test through real MCP transports and create a gated path from the pinned SDK to current supported versions.

**Expected outcome:** Transport, DI, serialization, middleware, and version negotiation regressions fail CI.

**Stories:**

- [US01.03.01](#id-us01-03-01) — Replace direct-tool smoke tests with real MCP client journeys (P0, 4d)
- [US01.03.02](#id-us01-03-02) — Gate MCP SDK upgrades with a compatibility matrix (P1, 3d)

## 4. User Stories and Subtasks

<a id="id-us01-01-01"></a>
### US01.01.01 — Mount the SDK MCP handler at the documented route

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.01](#id-f01-01) | P0 | 1.5 | 12 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP client developer

**User story:** As an MCP client developer, I want /mcp to be handled directly by the official SDK so that Streamable HTTP behaves according to the negotiated MCP protocol.

**Acceptance criteria:**

- [ ] The server calls the SDK endpoint mapper with the explicit /mcp pattern.
- [ ] POST, session GET, and DELETE behavior at /mcp is provided only by the SDK for the configured state mode.
- [ ] The informational root response is moved or constrained so no HTTP method has two matching endpoints.
- [ ] An initialize request to /mcp returns a valid negotiated protocol response rather than a fixed JSON-RPC id or status object.
- [ ] Endpoint metadata tests assert there is exactly one MCP route owner per supported method.

**Dependencies:** —

**Labels:** `transport` `mcp` `http` `routing`

**Source findings:**

- MapMcp() is currently mounted at root while documentation points clients to /mcp.
- MapGet('/') can conflict with the SDK's stateful GET route.

**Subtasks:**

<a id="id-st01-01-01-01"></a>
- [ ] **ST01.01.01.01 — Inventory current endpoint descriptors and route conflicts**
  - Type: analysis
  - Estimate: 3 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Route inventory and approved endpoint table
  - Status: Not Started
<a id="id-st01-01-01-02"></a>
- [ ] **ST01.01.01.02 — Map the SDK handler explicitly to /mcp**
  - Type: implementation
  - Estimate: 4 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Updated ASP.NET endpoint registration
  - Status: Not Started
<a id="id-st01-01-01-03"></a>
- [ ] **ST01.01.01.03 — Add route ownership and initialize integration tests**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Passing route and initialization test suite
  - Status: Not Started

<a id="id-us01-01-02"></a>
### US01.01.02 — Remove simulated MCP and legacy transport endpoints

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.01](#id-f01-01) | P0 | 1 | 9 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** service operator

**User story:** As a service operator, I want placeholder protocol routes removed so that monitoring and clients cannot mistake health-like responses for working MCP sessions.

**Acceptance criteria:**

- [ ] The custom POST /mcp implementation that reads a body and returns a fixed response is deleted.
- [ ] The custom GET /mcp/sse ping loop and POST /mcp/streamable status endpoint are deleted or replaced only by verified SDK compatibility mappings.
- [ ] No middleware or endpoint logs a raw JSON-RPC request body.
- [ ] Unsupported legacy endpoints return a documented 404 or 410 and are not advertised as supported transports.
- [ ] README and configuration examples name only endpoints proven by transport tests.

**Dependencies:** [US01.01.01](#id-us01-01-01)

**Labels:** `transport` `mcp` `sse` `logging`

**Source findings:**

- Custom /mcp, /mcp/sse, and /mcp/streamable routes simulate protocol behavior and use wildcard CORS.
- The custom POST route can log sensitive request bodies.

**Subtasks:**

<a id="id-st01-01-02-01"></a>
- [ ] **ST01.01.02.01 — Delete placeholder MCP/SSE/streamable handlers**
  - Type: implementation
  - Estimate: 3 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Single SDK-owned MCP surface
  - Status: Not Started
<a id="id-st01-01-02-02"></a>
- [ ] **ST01.01.02.02 — Remove request-body capture and wildcard endpoint headers**
  - Type: security
  - Estimate: 3 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Sanitized request pipeline
  - Status: Not Started
<a id="id-st01-01-02-03"></a>
- [ ] **ST01.01.02.03 — Correct transport documentation and endpoint examples**
  - Type: documentation
  - Estimate: 3 hours
  - Suggested owner role: technical writer
  - Deliverable/evidence: Test-aligned transport documentation
  - Status: Not Started

<a id="id-us01-01-03"></a>
### US01.01.03 — Make STDIO transport protocol-clean

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.01](#id-f01-01) | P0 | 1.5 | 13 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** desktop MCP user

**User story:** As a desktop MCP user, I want stdout reserved for MCP frames so that diagnostic output never corrupts the STDIO protocol stream.

**Acceptance criteria:**

- [ ] STDIO mode writes only protocol frames to stdout; application logs and startup messages use stderr.
- [ ] Transport mode is selected explicitly and invalid mixed-mode configuration fails startup with an actionable message.
- [ ] EOF, cancellation, and client shutdown close the host without an orphaned process.
- [ ] A subprocess test initializes, lists capabilities, calls one tool, and shuts down over stdin/stdout.
- [ ] The documented Claude Desktop command matches the tested executable arguments.

**Dependencies:** —

**Labels:** `transport` `stdio` `desktop` `lifecycle`

**Source findings:**

- STDIO is a core supported mode and requires byte-clean stdout plus a real client-level lifecycle test.

**Subtasks:**

<a id="id-st01-01-03-01"></a>
- [ ] **ST01.01.03.01 — Audit stdout and stderr writers in STDIO mode**
  - Type: analysis
  - Estimate: 3 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: STDIO output-channel audit
  - Status: Not Started
<a id="id-st01-01-03-02"></a>
- [ ] **ST01.01.03.02 — Implement explicit mode selection and graceful shutdown**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: backend engineer
  - Deliverable/evidence: Protocol-clean STDIO lifecycle
  - Status: Not Started
<a id="id-st01-01-03-03"></a>
- [ ] **ST01.01.03.03 — Create subprocess STDIO client test**
  - Type: testing
  - Estimate: 5 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Automated STDIO journey test
  - Status: Not Started

<a id="id-us01-02-01"></a>
### US01.02.01 — Enable structured content and declare output schemas

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.02](#id-f01-02) | P0 | 3 | 20 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP application developer

**User story:** As an MCP application developer, I want structured tool results that conform to declared schemas so that I can consume them without reparsing text JSON.

**Acceptance criteria:**

- [ ] Wrapped tool registration enables structured content by default rather than relying on a null option that resolves to false.
- [ ] Every tool publishes an output schema generated from or checked against its response DTO.
- [ ] Successful calls return structuredContent that validates against the published output schema.
- [ ] A backward-compatible text representation may be included, but it is generated from the same result object.
- [ ] Schema snapshots fail CI on accidental breaking changes unless an explicit contract version is updated.

**Dependencies:** [US01.01.01](#id-us01-01-01)

**Labels:** `protocol` `structured-content` `schemas` `compatibility`

**Source findings:**

- WithWrappedTools currently leaves UseStructuredContent false when options are null.
- Object results can be serialized only as text JSON.

**Subtasks:**

<a id="id-st01-02-01-01"></a>
- [ ] **ST01.02.01.01 — Enable structured wrapper options by default**
  - Type: implementation
  - Estimate: 4 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Structured-content wrapper configuration
  - Status: Not Started
<a id="id-st01-02-01-02"></a>
- [ ] **ST01.02.01.02 — Generate and version tool output schemas**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Output schema catalogue
  - Status: Not Started
<a id="id-st01-02-01-03"></a>
- [ ] **ST01.02.01.03 — Add schema-validation and snapshot tests**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Schema contract test suite
  - Status: Not Started

<a id="id-us01-02-02"></a>
### US01.02.02 — Map domain failure envelopes to MCP tool errors

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.02](#id-f01-02) | P0 | 2.5 | 16 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** AI agent

**User story:** As an AI agent, I want failed tool operations marked as protocol errors so that I do not treat NotFound, PremiumRequired, or Timeout envelopes as successful evidence.

**Acceptance criteria:**

- [ ] The wrapper detects every result whose domain success flag is false and sets the MCP tool result isError flag.
- [ ] NotFound, InvalidQuery, Timeout, ServiceUnavailable, PremiumRequired, BudgetExceeded, and new error categories preserve a stable machine code in structured content.
- [ ] Successful results always have isError false and do not contain a failure envelope.
- [ ] Unhandled exceptions are converted to a sanitized Unknown or ServiceUnavailable result with a trace id and no secret or stack trace.
- [ ] Protocol tests cover at least one example of every error category.

**Dependencies:** [US01.02.01](#id-us01-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01)

**Labels:** `protocol` `errors` `tool-wrapper`

**Source findings:**

- Most inner failure envelopes currently remain protocol successes; only BudgetExceeded is reliably marked as an error.

**Subtasks:**

<a id="id-st01-02-02-01"></a>
- [ ] **ST01.02.02.01 — Implement domain-envelope to MCP-error mapping**
  - Type: implementation
  - Estimate: 7 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Central result/error mapper
  - Status: Not Started
<a id="id-st01-02-02-02"></a>
- [ ] **ST01.02.02.02 — Sanitize unhandled exception results**
  - Type: security
  - Estimate: 3 hours
  - Suggested owner role: security engineer
  - Deliverable/evidence: Safe exception boundary
  - Status: Not Started
<a id="id-st01-02-02-03"></a>
- [ ] **ST01.02.02.03 — Test every success and error mapping**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Protocol result matrix tests
  - Status: Not Started

<a id="id-us01-03-01"></a>
### US01.03.01 — Replace direct-tool smoke tests with real MCP client journeys

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.03](#id-f01-03) | P0 | 4 | 31 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** maintainer

**User story:** As a maintainer, I want tests to cross the actual transport and host boundary so that routing, initialization, serialization, middleware, authentication, and DI defects are visible before release.

**Acceptance criteria:**

- [ ] HTTP tests start the production host with a stub Finnhub handler and use an MCP client to initialize and negotiate a protocol version.
- [ ] The client verifies tools/list, tools/call, resources/list, resources/read, prompts/list, and prompts/get.
- [ ] STDIO tests run the built executable as a subprocess and verify the same discovery and representative call flow.
- [ ] Tests verify structured success, protocol error, cancellation, malformed JSON-RPC, and unknown method behavior.
- [ ] Tests never require a live Finnhub key or consume external quota.

**Dependencies:** [US01.01.01](#id-us01-01-01), [US01.01.03](#id-us01-01-03), [US01.02.02](#id-us01-02-02)

**Labels:** `testing` `e2e` `mcp-client` `ci`

**Source findings:**

- The current live smoke suite boots the host but constructs tools directly, bypassing transport, protocol initialization, serialization, middleware, CORS, authentication, and routing.
- Program and DI behavior are effectively excluded from meaningful coverage.

**Subtasks:**

<a id="id-st01-03-01-01"></a>
- [ ] **ST01.03.01.01 — Build deterministic fake Finnhub HTTP handler and fixtures**
  - Type: testing
  - Estimate: 7 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Offline upstream fixture harness
  - Status: Not Started
<a id="id-st01-03-01-02"></a>
- [ ] **ST01.03.01.02 — Implement HTTP MCP client conformance journey**
  - Type: testing
  - Estimate: 10 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: HTTP protocol E2E suite
  - Status: Not Started
<a id="id-st01-03-01-03"></a>
- [ ] **ST01.03.01.03 — Implement STDIO MCP client conformance journey**
  - Type: testing
  - Estimate: 8 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: STDIO protocol E2E suite
  - Status: Not Started
<a id="id-st01-03-01-04"></a>
- [ ] **ST01.03.01.04 — Add malformed, cancellation, and error cases**
  - Type: testing
  - Estimate: 6 hours
  - Suggested owner role: test engineer
  - Deliverable/evidence: Negative protocol tests
  - Status: Not Started

<a id="id-us01-03-02"></a>
### US01.03.02 — Gate MCP SDK upgrades with a compatibility matrix

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F01.03](#id-f01-03) | P1 | 3 | 21 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** maintainer

**User story:** As a maintainer, I want a repeatable SDK upgrade process so that the project can move from the pinned SDK without introducing silent wire-level regressions.

**Acceptance criteria:**

- [ ] The supported MCP protocol and SDK versions are documented in a compatibility matrix.
- [ ] CI runs conformance tests against the pinned production version and the proposed upgrade version before merge.
- [ ] An upgrade ADR records changes in routing, transport state, authorization hooks, schemas, and client behavior.
- [ ] The current major SDK upgrade is delivered separately from functional feature work and has a rollback plan.

**Dependencies:** [US01.03.01](#id-us01-03-01)

**Labels:** `sdk` `upgrade` `compatibility` `adr`

**Source findings:**

- The repository pins ModelContextProtocol SDK 1.4 while a newer major release exists; tests should precede a controlled upgrade.

**Subtasks:**

<a id="id-st01-03-02-01"></a>
- [ ] **ST01.03.02.01 — Record current SDK/protocol behavior baseline**
  - Type: analysis
  - Estimate: 4 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Compatibility baseline
  - Status: Not Started
<a id="id-st01-03-02-02"></a>
- [ ] **ST01.03.02.02 — Run gated upgrade branch and resolve API changes**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: SDK upgrade pull request
  - Status: Not Started
<a id="id-st01-03-02-03"></a>
- [ ] **ST01.03.02.03 — Publish compatibility matrix and upgrade ADR**
  - Type: documentation
  - Estimate: 5 hours
  - Suggested owner role: software architect
  - Deliverable/evidence: Version matrix and ADR
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST01.01.01.01](#id-st01-01-01-01) | [US01.01.01](#id-us01-01-01) | P0 | Inventory current endpoint descriptors and route conflicts | analysis | 3 | backend engineer | Route inventory and approved endpoint table | Not Started |
| [ST01.01.01.02](#id-st01-01-01-02) | [US01.01.01](#id-us01-01-01) | P0 | Map the SDK handler explicitly to /mcp | implementation | 4 | backend engineer | Updated ASP.NET endpoint registration | Not Started |
| [ST01.01.01.03](#id-st01-01-01-03) | [US01.01.01](#id-us01-01-01) | P0 | Add route ownership and initialize integration tests | testing | 5 | test engineer | Passing route and initialization test suite | Not Started |
| [ST01.01.02.01](#id-st01-01-02-01) | [US01.01.02](#id-us01-01-02) | P0 | Delete placeholder MCP/SSE/streamable handlers | implementation | 3 | backend engineer | Single SDK-owned MCP surface | Not Started |
| [ST01.01.02.02](#id-st01-01-02-02) | [US01.01.02](#id-us01-01-02) | P0 | Remove request-body capture and wildcard endpoint headers | security | 3 | security engineer | Sanitized request pipeline | Not Started |
| [ST01.01.02.03](#id-st01-01-02-03) | [US01.01.02](#id-us01-01-02) | P0 | Correct transport documentation and endpoint examples | documentation | 3 | technical writer | Test-aligned transport documentation | Not Started |
| [ST01.01.03.01](#id-st01-01-03-01) | [US01.01.03](#id-us01-01-03) | P0 | Audit stdout and stderr writers in STDIO mode | analysis | 3 | backend engineer | STDIO output-channel audit | Not Started |
| [ST01.01.03.02](#id-st01-01-03-02) | [US01.01.03](#id-us01-01-03) | P0 | Implement explicit mode selection and graceful shutdown | implementation | 5 | backend engineer | Protocol-clean STDIO lifecycle | Not Started |
| [ST01.01.03.03](#id-st01-01-03-03) | [US01.01.03](#id-us01-01-03) | P0 | Create subprocess STDIO client test | testing | 5 | test engineer | Automated STDIO journey test | Not Started |
| [ST01.02.01.01](#id-st01-02-01-01) | [US01.02.01](#id-us01-02-01) | P0 | Enable structured wrapper options by default | implementation | 4 | MCP engineer | Structured-content wrapper configuration | Not Started |
| [ST01.02.01.02](#id-st01-02-01-02) | [US01.02.01](#id-us01-02-01) | P0 | Generate and version tool output schemas | implementation | 10 | MCP engineer | Output schema catalogue | Not Started |
| [ST01.02.01.03](#id-st01-02-01-03) | [US01.02.01](#id-us01-02-01) | P0 | Add schema-validation and snapshot tests | testing | 6 | test engineer | Schema contract test suite | Not Started |
| [ST01.02.02.01](#id-st01-02-02-01) | [US01.02.02](#id-us01-02-02) | P0 | Implement domain-envelope to MCP-error mapping | implementation | 7 | MCP engineer | Central result/error mapper | Not Started |
| [ST01.02.02.02](#id-st01-02-02-02) | [US01.02.02](#id-us01-02-02) | P0 | Sanitize unhandled exception results | security | 3 | security engineer | Safe exception boundary | Not Started |
| [ST01.02.02.03](#id-st01-02-02-03) | [US01.02.02](#id-us01-02-02) | P0 | Test every success and error mapping | testing | 6 | test engineer | Protocol result matrix tests | Not Started |
| [ST01.03.01.01](#id-st01-03-01-01) | [US01.03.01](#id-us01-03-01) | P0 | Build deterministic fake Finnhub HTTP handler and fixtures | testing | 7 | test engineer | Offline upstream fixture harness | Not Started |
| [ST01.03.01.02](#id-st01-03-01-02) | [US01.03.01](#id-us01-03-01) | P0 | Implement HTTP MCP client conformance journey | testing | 10 | MCP engineer | HTTP protocol E2E suite | Not Started |
| [ST01.03.01.03](#id-st01-03-01-03) | [US01.03.01](#id-us01-03-01) | P0 | Implement STDIO MCP client conformance journey | testing | 8 | MCP engineer | STDIO protocol E2E suite | Not Started |
| [ST01.03.01.04](#id-st01-03-01-04) | [US01.03.01](#id-us01-03-01) | P0 | Add malformed, cancellation, and error cases | testing | 6 | test engineer | Negative protocol tests | Not Started |
| [ST01.03.02.01](#id-st01-03-02-01) | [US01.03.02](#id-us01-03-02) | P1 | Record current SDK/protocol behavior baseline | analysis | 4 | MCP engineer | Compatibility baseline | Not Started |
| [ST01.03.02.02](#id-st01-03-02-02) | [US01.03.02](#id-us01-03-02) | P1 | Run gated upgrade branch and resolve API changes | implementation | 12 | MCP engineer | SDK upgrade pull request | Not Started |
| [ST01.03.02.03](#id-st01-03-02-03) | [US01.03.02](#id-us01-03-02) | P1 | Publish compatibility matrix and upgrade ADR | documentation | 5 | software architect | Version matrix and ADR | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E01 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| R-01 | Repository finding | Map the official MCP SDK to a real /mcp endpoint and eliminate the root GET route ambiguity. | [US01.01.01](#id-us01-01-01) | [E01](#id-e01) | P0 | Covered | Code-level P0 finding. |
| R-02 | Repository finding | Remove simulated /mcp, /mcp/sse, and /mcp/streamable handlers that return pings or fixed JSON-RPC responses rather than MCP sessions. | [US01.01.02](#id-us01-01-02) | [E01](#id-e01) | P0 | Covered | Code-level P0 finding. |
| R-08 | Repository finding | Translate unsuccessful inner envelopes into MCP protocol errors with structured content and stable output/error schemas. | [US01.02.01](#id-us01-02-01), [US01.02.02](#id-us01-02-02), [US07.04.01](./E07-bounded-response-and-token-contract.md#id-us07-04-01) | [E01](#id-e01) | P0 | Covered | Code-level P0 interoperability finding. |
| R-24 | Repository finding | Replace direct-construction 'live smoke' coverage with real SDK client initialization/list/call/resource/prompt transport tests and correct route/streaming/field claims. | [US01.03.01](#id-us01-03-01), [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | [E01](#id-e01) | P0 | Covered | Testing/documentation finding. |
| R-25 | Repository finding | Upgrade the pinned MCP C# SDK through a controlled compatibility program after protocol tests exist. | [US01.03.02](#id-us01-03-02), [US15.01.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-01-01), [US15.01.02](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-01-02) | [E01](#id-e01) | P1 | Covered | Dependency lifecycle finding. |
| RF-070 | Code-review detail | HTTP SDK handler is mounted at root while /mcp is documented, creating route ambiguity with the root banner. | [US01.01.01](#id-us01-01-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-071 | Code-review detail | Custom /mcp, /mcp/sse, and /mcp/streamable endpoints simulate rather than implement MCP and include wildcard CORS/raw-body behavior. | [US01.01.02](#id-us01-01-02), [US02.03.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-072 | Code-review detail | STDIO must keep stdout protocol-only and prove lifecycle compatibility with desktop clients. | [US01.01.03](#id-us01-01-03), [US01.03.01](#id-us01-03-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-073 | Code-review detail | Wrapped tools default structured content off and lack enforced output schemas. | [US01.02.01](#id-us01-02-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-074 | Code-review detail | Domain failure envelopes such as NotFound, PremiumRequired, and Timeout can appear as MCP successes. | [US01.02.02](#id-us01-02-02), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-075 | Code-review detail | Existing smoke tests bypass transport, initialization, serialization, middleware, DI, routing, CORS, and authentication. | [US01.03.01](#id-us01-03-01) | [E01](#id-e01) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-076 | Code-review detail | Pinned MCP SDK should be upgraded only after conformance tests and with a compatibility plan. | [US01.03.02](#id-us01-03-02) | [E01](#id-e01) | P1 | Covered | Detailed finding retained from the repository review. |

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

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E01 | Epic | — | P0 | MCP Transport and Protocol Integrity | See [E01](#id-e01) | See [E01](#id-e01) | — | finnhub-mcp; epic | — | Not Started |
| F01.01 | Feature | [E01](#id-e01) | P0 | Canonical MCP Endpoint Topology | See [F01.01](#id-f01-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e01 | — | Not Started |
| F01.02 | Feature | [E01](#id-e01) | P0 | Structured Tool Result Semantics | See [F01.02](#id-f01-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e01 | — | Not Started |
| F01.03 | Feature | [E01](#id-e01) | P0 | Protocol Conformance and SDK Lifecycle | See [F01.03](#id-f01-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e01 | — | Not Started |
| US01.01.01 | Story | [F01.01](#id-f01-01) | P0 | Mount the SDK MCP handler at the documented route | See [US01.01.01](#id-us01-01-01) | See [US01.01.01](#id-us01-01-01) | 1.5d | transport; mcp; http; routing | — | Not Started |
| US01.01.02 | Story | [F01.01](#id-f01-01) | P0 | Remove simulated MCP and legacy transport endpoints | See [US01.01.02](#id-us01-01-02) | See [US01.01.02](#id-us01-01-02) | 1d | transport; mcp; sse; logging | [US01.01.01](#id-us01-01-01) | Not Started |
| US01.01.03 | Story | [F01.01](#id-f01-01) | P0 | Make STDIO transport protocol-clean | See [US01.01.03](#id-us01-01-03) | See [US01.01.03](#id-us01-01-03) | 1.5d | transport; stdio; desktop; lifecycle | — | Not Started |
| US01.02.01 | Story | [F01.02](#id-f01-02) | P0 | Enable structured content and declare output schemas | See [US01.02.01](#id-us01-02-01) | See [US01.02.01](#id-us01-02-01) | 3d | protocol; structured-content; schemas; compatibility | [US01.01.01](#id-us01-01-01) | Not Started |
| US01.02.02 | Story | [F01.02](#id-f01-02) | P0 | Map domain failure envelopes to MCP tool errors | See [US01.02.02](#id-us01-02-02) | See [US01.02.02](#id-us01-02-02) | 2.5d | protocol; errors; tool-wrapper | [US01.02.01](#id-us01-02-01), [US04.01.01](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-01-01) | Not Started |
| US01.03.01 | Story | [F01.03](#id-f01-03) | P0 | Replace direct-tool smoke tests with real MCP client journeys | See [US01.03.01](#id-us01-03-01) | See [US01.03.01](#id-us01-03-01) | 4d | testing; e2e; mcp-client; ci | [US01.01.01](#id-us01-01-01), [US01.01.03](#id-us01-01-03), [US01.02.02](#id-us01-02-02) | Not Started |
| US01.03.02 | Story | [F01.03](#id-f01-03) | P1 | Gate MCP SDK upgrades with a compatibility matrix | See [US01.03.02](#id-us01-03-02) | See [US01.03.02](#id-us01-03-02) | 3d | sdk; upgrade; compatibility; adr | [US01.03.01](#id-us01-03-01) | Not Started |
| ST01.01.01.01 | Sub-task | [US01.01.01](#id-us01-01-01) | P0 | Inventory current endpoint descriptors and route conflicts | See [ST01.01.01.01](#id-st01-01-01-01) | Not applicable; see detail or parent section | 3h | finnhub-mcp; analysis | — | Not Started |
| ST01.01.01.02 | Sub-task | [US01.01.01](#id-us01-01-01) | P0 | Map the SDK handler explicitly to /mcp | See [ST01.01.01.02](#id-st01-01-01-02) | Not applicable; see detail or parent section | 4h | finnhub-mcp; implementation | — | Not Started |
| ST01.01.01.03 | Sub-task | [US01.01.01](#id-us01-01-01) | P0 | Add route ownership and initialize integration tests | See [ST01.01.01.03](#id-st01-01-01-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST01.01.02.01 | Sub-task | [US01.01.02](#id-us01-01-02) | P0 | Delete placeholder MCP/SSE/streamable handlers | See [ST01.01.02.01](#id-st01-01-02-01) | Not applicable; see detail or parent section | 3h | finnhub-mcp; implementation | — | Not Started |
| ST01.01.02.02 | Sub-task | [US01.01.02](#id-us01-01-02) | P0 | Remove request-body capture and wildcard endpoint headers | See [ST01.01.02.02](#id-st01-01-02-02) | Not applicable; see detail or parent section | 3h | finnhub-mcp; security | — | Not Started |
| ST01.01.02.03 | Sub-task | [US01.01.02](#id-us01-01-02) | P0 | Correct transport documentation and endpoint examples | See [ST01.01.02.03](#id-st01-01-02-03) | Not applicable; see detail or parent section | 3h | finnhub-mcp; documentation | — | Not Started |
| ST01.01.03.01 | Sub-task | [US01.01.03](#id-us01-01-03) | P0 | Audit stdout and stderr writers in STDIO mode | See [ST01.01.03.01](#id-st01-01-03-01) | Not applicable; see detail or parent section | 3h | finnhub-mcp; analysis | — | Not Started |
| ST01.01.03.02 | Sub-task | [US01.01.03](#id-us01-01-03) | P0 | Implement explicit mode selection and graceful shutdown | See [ST01.01.03.02](#id-st01-01-03-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST01.01.03.03 | Sub-task | [US01.01.03](#id-us01-01-03) | P0 | Create subprocess STDIO client test | See [ST01.01.03.03](#id-st01-01-03-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; testing | — | Not Started |
| ST01.02.01.01 | Sub-task | [US01.02.01](#id-us01-02-01) | P0 | Enable structured wrapper options by default | See [ST01.02.01.01](#id-st01-02-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; implementation | — | Not Started |
| ST01.02.01.02 | Sub-task | [US01.02.01](#id-us01-02-01) | P0 | Generate and version tool output schemas | See [ST01.02.01.02](#id-st01-02-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST01.02.01.03 | Sub-task | [US01.02.01](#id-us01-02-01) | P0 | Add schema-validation and snapshot tests | See [ST01.02.01.03](#id-st01-02-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST01.02.02.01 | Sub-task | [US01.02.02](#id-us01-02-02) | P0 | Implement domain-envelope to MCP-error mapping | See [ST01.02.02.01](#id-st01-02-02-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; implementation | — | Not Started |
| ST01.02.02.02 | Sub-task | [US01.02.02](#id-us01-02-02) | P0 | Sanitize unhandled exception results | See [ST01.02.02.02](#id-st01-02-02-02) | Not applicable; see detail or parent section | 3h | finnhub-mcp; security | — | Not Started |
| ST01.02.02.03 | Sub-task | [US01.02.02](#id-us01-02-02) | P0 | Test every success and error mapping | See [ST01.02.02.03](#id-st01-02-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST01.03.01.01 | Sub-task | [US01.03.01](#id-us01-03-01) | P0 | Build deterministic fake Finnhub HTTP handler and fixtures | See [ST01.03.01.01](#id-st01-03-01-01) | Not applicable; see detail or parent section | 7h | finnhub-mcp; testing | — | Not Started |
| ST01.03.01.02 | Sub-task | [US01.03.01](#id-us01-03-01) | P0 | Implement HTTP MCP client conformance journey | See [ST01.03.01.02](#id-st01-03-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; testing | — | Not Started |
| ST01.03.01.03 | Sub-task | [US01.03.01](#id-us01-03-01) | P0 | Implement STDIO MCP client conformance journey | See [ST01.03.01.03](#id-st01-03-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; testing | — | Not Started |
| ST01.03.01.04 | Sub-task | [US01.03.01](#id-us01-03-01) | P0 | Add malformed, cancellation, and error cases | See [ST01.03.01.04](#id-st01-03-01-04) | Not applicable; see detail or parent section | 6h | finnhub-mcp; testing | — | Not Started |
| ST01.03.02.01 | Sub-task | [US01.03.02](#id-us01-03-02) | P1 | Record current SDK/protocol behavior baseline | See [ST01.03.02.01](#id-st01-03-02-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; analysis | — | Not Started |
| ST01.03.02.02 | Sub-task | [US01.03.02](#id-us01-03-02) | P1 | Run gated upgrade branch and resolve API changes | See [ST01.03.02.02](#id-st01-03-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST01.03.02.03 | Sub-task | [US01.03.02](#id-us01-03-02) | P1 | Publish compatibility matrix and upgrade ADR | See [ST01.03.02.03](#id-st01-03-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; documentation | — | Not Started |

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

- [ ] Create E01 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 3 Features under E01.
- [ ] Create all 7 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 22 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 12 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 33 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

