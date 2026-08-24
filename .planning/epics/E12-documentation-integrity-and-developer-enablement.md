---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E12
title: "Documentation Integrity and Developer Enablement"
priority: P0
phase: "M0 — Hardened Core"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 10
  subtasks: 30
  traceability_owned: 20
  traceability_items: 21
story_estimate_days: 42
subtask_estimate_hours: 288
---

<a id="id-e12"></a>
# E12 — Documentation Integrity and Developer Enablement

This is the self-contained coding-agent backlog for E12. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E12 | P0 | 5 | 10 | 30 | 42 | 288 | M0 — Hardened Core | Not Started |

## 2. Epic Definition

**Objective:** Make every public claim verifiable against the shipped implementation and provide complete, navigable guidance for users, operators, and contributors.

**Business value:** Reduces failed installations and unsafe deployments, builds user trust, and shortens the path from discovery to a correct first integration.

**Exit criteria:**

- [ ] README transport, tool, field, limit, confidence, response-view, schema-loading, and test claims match executable behavior.
- [ ] Versioned documentation covers all 12 tools, 3 resources, 3 prompts, transports, security, deployment, errors, quota, data semantics, and compatibility.
- [ ] Architecture, request-flow, and trust-boundary diagrams are reviewed and linked from the documentation landing page.
- [ ] A new contributor can run the project in 30 minutes, understand the architecture in 2 hours, and follow a verified first-tool tutorial.
- [ ] Localization and learning assets have explicit ownership, review, accessibility, and versioning processes.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F12.01](#id-f12-01) | P0 | Documentation Truth and Information Architecture | 2 | 6 | Not Started |
| [F12.02](#id-f12-02) | P1 | Generated API and Contract Reference | 2 | 9 | Not Started |
| [F12.03](#id-f12-03) | P0 | Architecture, Operations, and Data Semantics | 2 | 8 | Not Started |
| [F12.04](#id-f12-04) | P1 | Contributor Onboarding and Decision Governance | 2 | 7 | Not Started |
| [F12.05](#id-f12-05) | P2 | Localization and Learning Media | 2 | 12 | Not Started |

<a id="id-f12-01"></a>
### F12.01 — Documentation Truth and Information Architecture

- **Parent Epic:** [E12](#id-e12)
- **Priority:** P0
- **Status:** Not Started

**Description:** Correct inaccurate public claims and split the monolithic guidance into task-oriented, versioned documentation.

**Expected outcome:** Users can select the right transport, endpoint, parameters, and deployment mode without discovering contradictions at runtime.

**Stories:**

- [US12.01.01](#id-us12-01-01) — Correct README claims against executable behavior (P0, 2d)
- [US12.01.02](#id-us12-01-02) — Split documentation into task-oriented sections (P1, 4d)

<a id="id-f12-02"></a>
### F12.02 — Generated API and Contract Reference

- **Parent Epic:** [E12](#id-e12)
- **Priority:** P1
- **Status:** Not Started

**Description:** Generate precise reference material from code-owned schemas and supplement it with reviewed examples and semantic notes.

**Expected outcome:** Every tool, resource, prompt, response, and failure mode has an accurate, maintainable reference.

**Stories:**

- [US12.02.01](#id-us12-02-01) — Generate a complete per-tool API reference (P1, 5d)
- [US12.02.02](#id-us12-02-02) — Document resources, prompts, errors, quota, and output semantics (P1, 4d)

<a id="id-f12-03"></a>
### F12.03 — Architecture, Operations, and Data Semantics

- **Parent Epic:** [E12](#id-e12)
- **Priority:** P0
- **Status:** Not Started

**Description:** Document component boundaries, request flow, trust boundaries, deployment profiles, quota behavior, and financial-data interpretation.

**Expected outcome:** Operators understand what is trusted, where data moves, how failures surface, and how metrics are calculated.

**Stories:**

- [US12.03.01](#id-us12-03-01) — Publish architecture, request-flow, and trust-boundary diagrams (P1, 3d)
- [US12.03.02](#id-us12-03-02) — Document secure transport and deployment profiles (P0, 5d)

<a id="id-f12-04"></a>
### F12.04 — Contributor Onboarding and Decision Governance

- **Parent Epic:** [E12](#id-e12)
- **Priority:** P1
- **Status:** Not Started

**Description:** Provide a verified development path, tool-authoring checklist, ADR process, and one source of truth for release and planning policy.

**Expected outcome:** A first contribution is predictable and architectural or release decisions do not drift across documents.

**Stories:**

- [US12.04.01](#id-us12-04-01) — Create a verified contributor onboarding path (P1, 4d)
- [US12.04.02](#id-us12-04-02) — Establish ADR and release-policy governance (P1, 3d)

<a id="id-f12-05"></a>
### F12.05 — Localization and Learning Media

- **Parent Epic:** [E12](#id-e12)
- **Priority:** P2
- **Status:** Not Started

**Description:** Add maintainable Chinese documentation and concise, accessible tutorials after the English contracts stabilize.

**Expected outcome:** More users can adopt the project without creating a second, stale documentation system.

**Stories:**

- [US12.05.01](#id-us12-05-01) — Launch reviewed Simplified Chinese documentation (P2, 6d)
- [US12.05.02](#id-us12-05-02) — Produce short tutorials and executable examples (P2, 6d)

## 4. User Stories and Subtasks

<a id="id-us12-01-01"></a>
### US12.01.01 — Correct README claims against executable behavior

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.01](#id-f12-01) | P0 | 2 | 16 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** MCP client user

**User story:** As an MCP client user, I want the README to describe the routes and capabilities that actually ship so that setup instructions do not lead me to stub or conflicting endpoints.

**Acceptance criteria:**

- [ ] The README names the exact SDK-mapped HTTP endpoint and supported STDIO invocation and labels legacy or unsupported transports accurately.
- [ ] Claims for fields projection, limit handling, confidence and exact-match scoring, lazy schema loading, full/raw views, and streaming are either demonstrated by tests or removed and replaced with current behavior.
- [ ] The existing live-smoke description states that direct tool construction does not validate MCP transport, initialization, serialization, middleware, CORS, authentication, resources, or prompts.
- [ ] A CI link checker and a documentation assertion test fail when documented endpoint paths or the count of tools, resources, and prompts diverge from the runtime catalogue.
- [ ] The corrected page includes a dated compatibility note for the reviewed server and SDK versions.

**Dependencies:** [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01)

**Labels:** `documentation` `transport` `truthfulness` `p0`

**Source findings:**

- README documents /mcp while SDK v1.4 MapMcp is mapped at the root and custom /mcp routes are non-protocol stubs.
- README overstates fields, limit, confidence/exact matching, lazy schemas, full/raw output, streaming, and live-smoke coverage.

**Subtasks:**

<a id="id-st12-01-01-01"></a>
- [ ] **ST12.01.01.01 — Build a claim-to-code verification table**
  - Type: analysis
  - Estimate: 4 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Reviewed matrix linking every README feature, route, count, and test claim to source or protocol evidence.
  - Status: Not Started
<a id="id-st12-01-01-02"></a>
- [ ] **ST12.01.01.02 — Rewrite README setup and capability sections**
  - Type: documentation
  - Estimate: 8 hours
  - Suggested owner role: C# maintainer
  - Deliverable/evidence: Corrected README with verified endpoint examples and explicit limitations.
  - Status: Not Started
<a id="id-st12-01-01-03"></a>
- [ ] **ST12.01.01.03 — Add documentation truth checks**
  - Type: test
  - Estimate: 4 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: CI checks for route references, catalogue counts, broken links, and dated compatibility metadata.
  - Status: Not Started

<a id="id-us12-01-02"></a>
### US12.01.02 — Split documentation into task-oriented sections

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.01](#id-f12-01) | P1 | 4 | 28 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Developer evaluating the project

**User story:** As a developer, I want a clear documentation map so that I can move from quick start to integration, deployment, and troubleshooting without searching a single oversized README.

**Acceptance criteria:**

- [ ] The docs navigation includes Quick Start, API Reference, Resources and Prompts, Transports, Architecture, Security, Deployment, Errors and Quota, Data Accuracy, Troubleshooting, Compatibility, and Contributing.
- [ ] The README becomes a concise project overview with verified install examples and links to canonical detail pages.
- [ ] Every page has an owner, last-reviewed date, applicable release range, and links to prerequisites and next steps.
- [ ] Repository checks report orphaned pages, broken anchors, duplicate canonical topics, and stale generated navigation.
- [ ] Versioned documentation remains available for at least the current and previous supported release lines.

**Dependencies:** [US12.01.01](#id-us12-01-01)

**Labels:** `documentation` `information-architecture` `developer-experience`

**Source findings:**

- The README is rich but needs separate quick-start, API, transport, architecture, security, deployment, error/quota, accuracy, troubleshooting, compatibility, and contributor documentation.

**Subtasks:**

<a id="id-st12-01-02-01"></a>
- [ ] **ST12.01.02.01 — Define documentation taxonomy and ownership**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: Documentation architect
  - Deliverable/evidence: Approved navigation tree, canonical-topic map, owner map, and versioning policy.
  - Status: Not Started
<a id="id-st12-01-02-02"></a>
- [ ] **ST12.01.02.02 — Extract canonical pages from README**
  - Type: documentation
  - Estimate: 16 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Task-oriented documentation pages with redirects and cross-links from the concise README.
  - Status: Not Started
<a id="id-st12-01-02-03"></a>
- [ ] **ST12.01.02.03 — Validate navigation and version metadata**
  - Type: automation
  - Estimate: 6 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Orphan, anchor, duplicate-canonical, owner, and supported-version checks in CI.
  - Status: Not Started

<a id="id-us12-02-01"></a>
### US12.02.01 — Generate a complete per-tool API reference

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.02](#id-f12-02) | P1 | 5 | 30 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Application integrator

**User story:** As an integrator, I want exact contracts and examples for every tool so that I can construct valid requests and budget the response before invoking it.

**Acceptance criteria:**

- [ ] A build task generates reference pages for all 12 tools from the registered schema and fails if catalogue and page counts differ.
- [ ] Each page documents purpose, when not to use it, required and optional parameters, constraints, defaults, view behavior, field projection status, output schema, and at least one valid example.
- [ ] Each page states units, currency and timezone behavior, data freshness or delay, upstream request cost, cache TTL, premium-plan requirements, and maximum result/output bounds where applicable.
- [ ] Each page lists stable error codes, pagination or truncation behavior, next_actions shape, and a redacted representative response.
- [ ] Generated content is reproducible and CI fails on an uncommitted schema/reference diff.

**Dependencies:** [US12.01.02](#id-us12-01-02)

**Labels:** `api-reference` `code-generation` `tools`

**Source findings:**

- A separate API reference is needed for parameters, examples, units, freshness, cost, caching, premium requirements, errors, pagination, and next actions for each tool.
- The service exposes 12 tools whose contract details currently live across code and README prose.

**Subtasks:**

<a id="id-st12-02-01-01"></a>
- [ ] **ST12.02.01.01 — Define reference schema and page template**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: API designer
  - Deliverable/evidence: Reference model covering parameters, bounds, output, cost, freshness, errors, and examples.
  - Status: Not Started
<a id="id-st12-02-01-02"></a>
- [ ] **ST12.02.01.02 — Implement catalogue-to-Markdown generator**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: C# engineer
  - Deliverable/evidence: Deterministic generator and pages for all 12 registered tools.
  - Status: Not Started
<a id="id-st12-02-01-03"></a>
- [ ] **ST12.02.01.03 — Add reference drift gates and example review**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: CI schema/page count and diff checks plus reviewed, redacted response fixtures.
  - Status: Not Started

<a id="id-us12-02-02"></a>
### US12.02.02 — Document resources, prompts, errors, quota, and output semantics

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.02](#id-f12-02) | P1 | 4 | 28 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Agent workflow author

**User story:** As a workflow author, I want all non-tool surfaces and cross-cutting contracts documented so that I can reason about failures, quota cost, and financial meaning consistently.

**Acceptance criteria:**

- [ ] Reference pages cover the capabilities, exchanges, and api-status resources and the research-ticker, compare-peers, and news-pulse prompts with URI or argument examples.
- [ ] The error reference distinguishes MCP protocol errors from domain envelopes and documents NotFound, InvalidQuery, ServiceUnavailable, Timeout, InvalidResponse, BudgetExceeded, PremiumRequired, and any newly added authentication, authorization, throttling, cancellation, circuit, and partial-success codes.
- [ ] Quota documentation explains per-call upstream cost, retries, cache hits, multi-call workflows, free-tier assumptions, reservation policy, and Retry-After behavior without presenting a plan limit as permanent fact.
- [ ] Data-accuracy notes define price-summary volatility, insider-signal transaction classification and window, news period boundaries and sentiment completeness, and provenance/as-of metadata.
- [ ] Examples explicitly separate article-volume change from sentiment trend and do not infer unavailable margins or growth metrics.

**Dependencies:** [US12.02.01](#id-us12-02-01)

**Labels:** `resources` `prompts` `errors` `quota` `data-accuracy`

**Source findings:**

- Three resources and three prompts need dedicated reference coverage.
- Financial semantics for volatility, insider Form 4 activity, news windows, and prompt claims require explicit accuracy documentation.
- Current domain errors are not always surfaced as MCP tool failures and the quota tracker does not expose intermediate retry/429 behavior.

**Subtasks:**

<a id="id-st12-02-02-01"></a>
- [ ] **ST12.02.02.01 — Author resource and prompt reference pages**
  - Type: documentation
  - Estimate: 10 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Reference for 3 resources and 3 prompts with corrected workflow examples.
  - Status: Not Started
<a id="id-st12-02-02-02"></a>
- [ ] **ST12.02.02.02 — Author error and quota contract**
  - Type: documentation
  - Estimate: 10 hours
  - Suggested owner role: Platform engineer
  - Deliverable/evidence: Stable failure taxonomy and upstream-call/cache/retry/quota cost guide.
  - Status: Not Started
<a id="id-st12-02-02-03"></a>
- [ ] **ST12.02.02.03 — Review financial data-accuracy notes**
  - Type: domain-review
  - Estimate: 8 hours
  - Suggested owner role: Financial data specialist
  - Deliverable/evidence: Approved definitions and examples for volatility, insider, news, provenance, and prompt semantics.
  - Status: Not Started

<a id="id-us12-03-01"></a>
### US12.03.01 — Publish architecture, request-flow, and trust-boundary diagrams

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.03](#id-f12-03) | P1 | 3 | 17 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Maintainer or security reviewer

**User story:** As a reviewer, I want concise diagrams tied to the code so that I can understand dependencies, data movement, and security boundaries before changing or deploying the server.

**Acceptance criteria:**

- [ ] A Clean Architecture diagram maps Server, Application, Domain, Infrastructure, Finnhub, cache, and MCP client boundaries to actual projects or namespaces.
- [ ] A sequence diagram covers initialize, tools/list, tools/call, middleware, cache hit and miss, quota reservation, Polly retry/circuit behavior, Finnhub response, projection, and MCP serialization.
- [ ] A trust-boundary diagram differentiates STDIO local trust, loopback HTTP, remote HTTP, user/API credentials, demo BFF, cache, logs, and external Finnhub/static hosts.
- [ ] Each diagram has editable source stored in the repository, alternative text, and a review test or checklist that validates named components and endpoints.
- [ ] Diagram links appear in README, architecture docs, security docs, and contributor onboarding.

**Dependencies:** [US12.01.02](#id-us12-01-02)

**Labels:** `architecture` `diagram` `security` `data-flow`

**Source findings:**

- The project needs Clean Architecture, request-sequence, data-flow, and trust-boundary diagrams.
- STDIO, local HTTP, hosted HTTP, Finnhub, cache, and logging have materially different trust assumptions.

**Subtasks:**

<a id="id-st12-03-01-01"></a>
- [ ] **ST12.03.01.01 — Create Clean Architecture component diagram**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: Solution architect
  - Deliverable/evidence: Editable, code-linked component diagram with accessible export.
  - Status: Not Started
<a id="id-st12-03-01-02"></a>
- [ ] **ST12.03.01.02 — Create request and quota sequence diagram**
  - Type: design
  - Estimate: 6 hours
  - Suggested owner role: Platform engineer
  - Deliverable/evidence: MCP-to-cache-to-Finnhub sequence including retries, limits, projection, and serialization.
  - Status: Not Started
<a id="id-st12-03-01-03"></a>
- [ ] **ST12.03.01.03 — Create trust-boundary and deployment diagram**
  - Type: security-design
  - Estimate: 6 hours
  - Suggested owner role: Security architect
  - Deliverable/evidence: Threat-boundary diagram for STDIO, loopback, remote, demo, secrets, logs, cache, and upstreams.
  - Status: Not Started

<a id="id-us12-03-02"></a>
### US12.03.02 — Document secure transport and deployment profiles

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.03](#id-f12-03) | P0 | 5 | 36 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Service operator

**User story:** As an operator, I want prescriptive deployment profiles so that I do not expose a local-development configuration as a multi-tenant internet service.

**Acceptance criteria:**

- [ ] The transport guide gives verified commands, endpoint paths, client configuration, lifecycle behavior, and limitations for STDIO and Streamable HTTP and avoids inventing WebSocket as an MCP transport.
- [ ] Security guidance defines separate STDIO, loopback HTTP, and remote HTTP profiles covering bind address, Host and Origin validation, TLS, CORS allowlists, authentication, authorization scopes, tenant isolation, rate/concurrency/body limits, secrets, logging, and audit events.
- [ ] Deployment guides cover local process, container, and a representative cloud deployment using managed identity or workload identity with a secret manager rather than browser or source-controlled credentials.
- [ ] Troubleshooting includes route ambiguity, initialization failures, quota exhaustion, premium endpoints, cache behavior, CORS/Origin failures, circuit state, and redacted diagnostics.
- [ ] A compatibility matrix records supported .NET, MCP SDK, MCP protocol revision, Finnhub plan assumptions, operating systems, and client versions.

**Dependencies:** [US12.03.01](#id-us12-03-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01)

**Labels:** `security` `deployment` `transport` `operations` `p0`

**Source findings:**

- Hosted HTTP currently lacks a documented secure boundary and custom routes set permissive CORS.
- MCP uses Streamable HTTP; upstream Finnhub WebSocket streaming should be described separately, not as a replacement MCP transport.
- Cloud secret managers and tenant-aware remote deployment require explicit guidance.

**Subtasks:**

<a id="id-st12-03-02-01"></a>
- [ ] **ST12.03.02.01 — Write transport and compatibility guides**
  - Type: documentation
  - Estimate: 12 hours
  - Suggested owner role: MCP engineer
  - Deliverable/evidence: Verified STDIO/Streamable HTTP procedures and release/client/runtime compatibility matrix.
  - Status: Not Started
<a id="id-st12-03-02-02"></a>
- [ ] **ST12.03.02.02 — Write security and secret-management profiles**
  - Type: security-documentation
  - Estimate: 12 hours
  - Suggested owner role: Security engineer
  - Deliverable/evidence: Prescriptive local, loopback, remote, and cloud-secret deployment profiles.
  - Status: Not Started
<a id="id-st12-03-02-03"></a>
- [ ] **ST12.03.02.03 — Write deployment and troubleshooting runbooks**
  - Type: documentation
  - Estimate: 12 hours
  - Suggested owner role: Site reliability engineer
  - Deliverable/evidence: Local, container, cloud, error, quota, cache, route, CORS, circuit, and diagnostic runbooks.
  - Status: Not Started

<a id="id-us12-04-01"></a>
### US12.04.01 — Create a verified contributor onboarding path

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.04](#id-f12-04) | P1 | 4 | 30 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** New contributor

**User story:** As a new contributor, I want a timed, executable path from clone to first tool change so that I can contribute without reverse-engineering project conventions.

**Acceptance criteria:**

- [ ] Onboarding includes prerequisites, clone, secret setup, restore, build, test, format, STDIO run, HTTP run, Inspector use, and a no-key fixture path where possible.
- [ ] A first-tool tutorial covers schema and description, validation, service boundary, source-generation context, caching, quota cost, error mapping, next_actions validation, documentation generation, and protocol tests.
- [ ] A clean-environment CI job executes all documented setup commands and records target budgets of 30 minutes to run, 2 hours to understand architecture, and 1-2 days for a first production-quality tool.
- [ ] The contributor guide defines code style, test layers, financial-semantic review, security review triggers, commit/PR expectations, and release process.
- [ ] Issue and PR templates link to the relevant checklist and require documentation and compatibility impact declarations.

**Dependencies:** [US12.03.01](#id-us12-03-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01)

**Labels:** `contributing` `onboarding` `developer-experience`

**Source findings:**

- Expected onboarding is approximately 15-30 minutes to run, 1-2 hours to understand, and 1-2 days to build a production-quality first tool.
- Contributor documentation needs a complete tool-authoring and verification path.

**Subtasks:**

<a id="id-st12-04-01-01"></a>
- [ ] **ST12.04.01.01 — Build clean-environment setup tutorial**
  - Type: documentation
  - Estimate: 10 hours
  - Suggested owner role: Developer advocate
  - Deliverable/evidence: Timed clone-to-running guide for key and no-key workflows.
  - Status: Not Started
<a id="id-st12-04-01-02"></a>
- [ ] **ST12.04.01.02 — Build first-tool contribution tutorial**
  - Type: documentation
  - Estimate: 12 hours
  - Suggested owner role: Senior C# engineer
  - Deliverable/evidence: End-to-end tool-authoring checklist and tutorial covering code, schema, domain review, docs, and protocol tests.
  - Status: Not Started
<a id="id-st12-04-01-03"></a>
- [ ] **ST12.04.01.03 — Continuously test onboarding**
  - Type: automation
  - Estimate: 8 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Clean-runner workflow executing documented setup plus issue/PR templates linked to checklists.
  - Status: Not Started

<a id="id-us12-04-02"></a>
### US12.04.02 — Establish ADR and release-policy governance

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.04](#id-f12-04) | P1 | 3 | 23 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Project maintainer

**User story:** As a maintainer, I want one authoritative decision and release process so that branch names, planning artifacts, and architectural rationale do not drift.

**Acceptance criteria:**

- [ ] An ADR template and index capture context, decision, alternatives, consequences, security and compatibility impacts, owner, date, and status.
- [ ] Initial ADRs cover transport mapping, local versus hosted trust profiles, cache and quota boundaries, output-view contract, dynamic tool registration policy, and SDK version strategy.
- [ ] Contributor and release documentation use the actual default and release branches and CI checks fail on stale branch references.
- [ ] The ignored .planning material is either promoted into version-controlled canonical docs or removed from all claims of authority.
- [ ] A quarterly documentation-owner review archives superseded decisions and opens tracked issues for unresolved drift.

**Dependencies:** [US12.01.02](#id-us12-01-02)

**Labels:** `adr` `governance` `release` `contributing`

**Source findings:**

- Contributor documentation contains release-branch drift versus main.
- .planning is ignored while documentation refers to it as authoritative.
- Architectural decisions need durable ADRs.

**Subtasks:**

<a id="id-st12-04-02-01"></a>
- [ ] **ST12.04.02.01 — Create ADR template, index, and initial records**
  - Type: governance
  - Estimate: 12 hours
  - Suggested owner role: Solution architect
  - Deliverable/evidence: Version-controlled ADR system and initial decisions for core platform contracts.
  - Status: Not Started
<a id="id-st12-04-02-02"></a>
- [ ] **ST12.04.02.02 — Resolve branch and planning-source drift**
  - Type: documentation
  - Estimate: 6 hours
  - Suggested owner role: Release manager
  - Deliverable/evidence: Correct branch references and version-controlled canonical planning source.
  - Status: Not Started
<a id="id-st12-04-02-03"></a>
- [ ] **ST12.04.02.03 — Automate governance drift review**
  - Type: automation
  - Estimate: 5 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Branch-reference checks and scheduled ADR/documentation ownership review issue.
  - Status: Not Started

<a id="id-us12-05-01"></a>
### US12.05.01 — Launch reviewed Simplified Chinese documentation

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.05](#id-f12-05) | P2 | 6 | 38 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Chinese-speaking developer

**User story:** As a Chinese-speaking developer, I want the highest-risk setup and security guidance in my language so that I can deploy the server correctly.

**Acceptance criteria:**

- [ ] Localization begins only after stable English contracts and Unicode-friendly input behavior are released and documented.
- [ ] Phase one translates Quick Start, Security, Errors and Quota, and Troubleshooting, with English version and last-reviewed metadata visible on every page.
- [ ] A bilingual human reviewer approves financial, security, protocol, and legal terminology before publication.
- [ ] The site falls back to English for missing pages and CI detects broken locale links, missing critical translations, and source pages changed since translation review.
- [ ] A glossary and contribution workflow prevent inconsistent translations of MCP and financial terms.

**Dependencies:** [US12.01.02](#id-us12-01-02), [US12.03.02](#id-us12-03-02)

**Labels:** `i18n` `zh-cn` `documentation`

**Source findings:**

- Chinese documentation is useful but should follow stable English documentation and Unicode input support.
- Quick Start, Security, and Errors should be translated first and human reviewed.

**Subtasks:**

<a id="id-st12-05-01-01"></a>
- [ ] **ST12.05.01.01 — Define Chinese localization glossary and workflow**
  - Type: localization-design
  - Estimate: 8 hours
  - Suggested owner role: Localization lead
  - Deliverable/evidence: Approved glossary, source linkage, reviewer, fallback, and update policy.
  - Status: Not Started
<a id="id-st12-05-01-02"></a>
- [ ] **ST12.05.01.02 — Translate and review phase-one pages**
  - Type: localization
  - Estimate: 24 hours
  - Suggested owner role: Bilingual technical writer
  - Deliverable/evidence: Human-reviewed zh-CN Quick Start, Security, Errors/Quota, and Troubleshooting pages.
  - Status: Not Started
<a id="id-st12-05-01-03"></a>
- [ ] **ST12.05.01.03 — Add localization freshness checks**
  - Type: automation
  - Estimate: 6 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Locale link, critical-page, glossary, and source-change stale checks.
  - Status: Not Started

<a id="id-us12-05-02"></a>
### US12.05.02 — Produce short tutorials and executable examples

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F12.05](#id-f12-05) | P2 | 6 | 42 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Visual learner

**User story:** As a learner, I want concise tutorials backed by executable examples so that I can install, inspect, and understand the server quickly.

**Acceptance criteria:**

- [ ] A 3-5 minute install-and-Inspector tutorial and an 8-10 minute architecture-and-data-flow tutorial use the current release and show redacted configuration.
- [ ] Every video includes captions, transcript, chapter markers, source version, publication date, and links to the equivalent written steps.
- [ ] Copy-paste examples for all 12 tools, 3 resources, and 3 prompts are exercised in CI against fixtures or recorded contract responses.
- [ ] Examples visibly distinguish fixture, local-key, and hosted-demo modes and state quota and premium-plan implications.
- [ ] A release checklist flags tutorials for refresh when endpoints, setup, UI, or contracts change.

**Dependencies:** [US12.02.01](#id-us12-02-01), [US12.03.02](#id-us12-03-02), [US13.02.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-01)

**Labels:** `video` `examples` `education` `accessibility`

**Source findings:**

- Short install/Inspector and architecture tutorials with transcripts would improve onboarding.
- Interactive and executable examples should supplement, not replace, exact written reference.

**Subtasks:**

<a id="id-st12-05-02-01"></a>
- [ ] **ST12.05.02.01 — Script and record core tutorials**
  - Type: media-production
  - Estimate: 20 hours
  - Suggested owner role: Developer advocate
  - Deliverable/evidence: Versioned install/Inspector and architecture/data-flow videos.
  - Status: Not Started
<a id="id-st12-05-02-02"></a>
- [ ] **ST12.05.02.02 — Create accessible transcripts and written equivalents**
  - Type: documentation
  - Estimate: 10 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Captions, transcripts, chapters, alt assets, and matching written tutorials.
  - Status: Not Started
<a id="id-st12-05-02-03"></a>
- [ ] **ST12.05.02.03 — Validate all copy-paste examples**
  - Type: test
  - Estimate: 12 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: CI-executed example corpus covering 12 tools, 3 resources, and 3 prompts.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST12.01.01.01](#id-st12-01-01-01) | [US12.01.01](#id-us12-01-01) | P0 | Build a claim-to-code verification table | analysis | 4 | Technical writer | Reviewed matrix linking every README feature, route, count, and test claim to source or protocol evidence. | Not Started |
| [ST12.01.01.02](#id-st12-01-01-02) | [US12.01.01](#id-us12-01-01) | P0 | Rewrite README setup and capability sections | documentation | 8 | C# maintainer | Corrected README with verified endpoint examples and explicit limitations. | Not Started |
| [ST12.01.01.03](#id-st12-01-01-03) | [US12.01.01](#id-us12-01-01) | P0 | Add documentation truth checks | test | 4 | Test engineer | CI checks for route references, catalogue counts, broken links, and dated compatibility metadata. | Not Started |
| [ST12.01.02.01](#id-st12-01-02-01) | [US12.01.02](#id-us12-01-02) | P1 | Define documentation taxonomy and ownership | design | 6 | Documentation architect | Approved navigation tree, canonical-topic map, owner map, and versioning policy. | Not Started |
| [ST12.01.02.02](#id-st12-01-02-02) | [US12.01.02](#id-us12-01-02) | P1 | Extract canonical pages from README | documentation | 16 | Technical writer | Task-oriented documentation pages with redirects and cross-links from the concise README. | Not Started |
| [ST12.01.02.03](#id-st12-01-02-03) | [US12.01.02](#id-us12-01-02) | P1 | Validate navigation and version metadata | automation | 6 | DevOps engineer | Orphan, anchor, duplicate-canonical, owner, and supported-version checks in CI. | Not Started |
| [ST12.02.01.01](#id-st12-02-01-01) | [US12.02.01](#id-us12-02-01) | P1 | Define reference schema and page template | design | 6 | API designer | Reference model covering parameters, bounds, output, cost, freshness, errors, and examples. | Not Started |
| [ST12.02.01.02](#id-st12-02-01-02) | [US12.02.01](#id-us12-02-01) | P1 | Implement catalogue-to-Markdown generator | implementation | 16 | C# engineer | Deterministic generator and pages for all 12 registered tools. | Not Started |
| [ST12.02.01.03](#id-st12-02-01-03) | [US12.02.01](#id-us12-02-01) | P1 | Add reference drift gates and example review | test | 8 | Test engineer | CI schema/page count and diff checks plus reviewed, redacted response fixtures. | Not Started |
| [ST12.02.02.01](#id-st12-02-02-01) | [US12.02.02](#id-us12-02-02) | P1 | Author resource and prompt reference pages | documentation | 10 | MCP engineer | Reference for 3 resources and 3 prompts with corrected workflow examples. | Not Started |
| [ST12.02.02.02](#id-st12-02-02-02) | [US12.02.02](#id-us12-02-02) | P1 | Author error and quota contract | documentation | 10 | Platform engineer | Stable failure taxonomy and upstream-call/cache/retry/quota cost guide. | Not Started |
| [ST12.02.02.03](#id-st12-02-02-03) | [US12.02.02](#id-us12-02-02) | P1 | Review financial data-accuracy notes | domain-review | 8 | Financial data specialist | Approved definitions and examples for volatility, insider, news, provenance, and prompt semantics. | Not Started |
| [ST12.03.01.01](#id-st12-03-01-01) | [US12.03.01](#id-us12-03-01) | P1 | Create Clean Architecture component diagram | design | 5 | Solution architect | Editable, code-linked component diagram with accessible export. | Not Started |
| [ST12.03.01.02](#id-st12-03-01-02) | [US12.03.01](#id-us12-03-01) | P1 | Create request and quota sequence diagram | design | 6 | Platform engineer | MCP-to-cache-to-Finnhub sequence including retries, limits, projection, and serialization. | Not Started |
| [ST12.03.01.03](#id-st12-03-01-03) | [US12.03.01](#id-us12-03-01) | P1 | Create trust-boundary and deployment diagram | security-design | 6 | Security architect | Threat-boundary diagram for STDIO, loopback, remote, demo, secrets, logs, cache, and upstreams. | Not Started |
| [ST12.03.02.01](#id-st12-03-02-01) | [US12.03.02](#id-us12-03-02) | P0 | Write transport and compatibility guides | documentation | 12 | MCP engineer | Verified STDIO/Streamable HTTP procedures and release/client/runtime compatibility matrix. | Not Started |
| [ST12.03.02.02](#id-st12-03-02-02) | [US12.03.02](#id-us12-03-02) | P0 | Write security and secret-management profiles | security-documentation | 12 | Security engineer | Prescriptive local, loopback, remote, and cloud-secret deployment profiles. | Not Started |
| [ST12.03.02.03](#id-st12-03-02-03) | [US12.03.02](#id-us12-03-02) | P0 | Write deployment and troubleshooting runbooks | documentation | 12 | Site reliability engineer | Local, container, cloud, error, quota, cache, route, CORS, circuit, and diagnostic runbooks. | Not Started |
| [ST12.04.01.01](#id-st12-04-01-01) | [US12.04.01](#id-us12-04-01) | P1 | Build clean-environment setup tutorial | documentation | 10 | Developer advocate | Timed clone-to-running guide for key and no-key workflows. | Not Started |
| [ST12.04.01.02](#id-st12-04-01-02) | [US12.04.01](#id-us12-04-01) | P1 | Build first-tool contribution tutorial | documentation | 12 | Senior C# engineer | End-to-end tool-authoring checklist and tutorial covering code, schema, domain review, docs, and protocol tests. | Not Started |
| [ST12.04.01.03](#id-st12-04-01-03) | [US12.04.01](#id-us12-04-01) | P1 | Continuously test onboarding | automation | 8 | DevOps engineer | Clean-runner workflow executing documented setup plus issue/PR templates linked to checklists. | Not Started |
| [ST12.04.02.01](#id-st12-04-02-01) | [US12.04.02](#id-us12-04-02) | P1 | Create ADR template, index, and initial records | governance | 12 | Solution architect | Version-controlled ADR system and initial decisions for core platform contracts. | Not Started |
| [ST12.04.02.02](#id-st12-04-02-02) | [US12.04.02](#id-us12-04-02) | P1 | Resolve branch and planning-source drift | documentation | 6 | Release manager | Correct branch references and version-controlled canonical planning source. | Not Started |
| [ST12.04.02.03](#id-st12-04-02-03) | [US12.04.02](#id-us12-04-02) | P1 | Automate governance drift review | automation | 5 | DevOps engineer | Branch-reference checks and scheduled ADR/documentation ownership review issue. | Not Started |
| [ST12.05.01.01](#id-st12-05-01-01) | [US12.05.01](#id-us12-05-01) | P2 | Define Chinese localization glossary and workflow | localization-design | 8 | Localization lead | Approved glossary, source linkage, reviewer, fallback, and update policy. | Not Started |
| [ST12.05.01.02](#id-st12-05-01-02) | [US12.05.01](#id-us12-05-01) | P2 | Translate and review phase-one pages | localization | 24 | Bilingual technical writer | Human-reviewed zh-CN Quick Start, Security, Errors/Quota, and Troubleshooting pages. | Not Started |
| [ST12.05.01.03](#id-st12-05-01-03) | [US12.05.01](#id-us12-05-01) | P2 | Add localization freshness checks | automation | 6 | Web engineer | Locale link, critical-page, glossary, and source-change stale checks. | Not Started |
| [ST12.05.02.01](#id-st12-05-02-01) | [US12.05.02](#id-us12-05-02) | P2 | Script and record core tutorials | media-production | 20 | Developer advocate | Versioned install/Inspector and architecture/data-flow videos. | Not Started |
| [ST12.05.02.02](#id-st12-05-02-02) | [US12.05.02](#id-us12-05-02) | P2 | Create accessible transcripts and written equivalents | documentation | 10 | Technical writer | Captions, transcripts, chapters, alt assets, and matching written tutorials. | Not Started |
| [ST12.05.02.03](#id-st12-05-02-03) | [US12.05.02](#id-us12-05-02) | P2 | Validate all copy-paste examples | test | 12 | Test engineer | CI-executed example corpus covering 12 tools, 3 resources, and 3 prompts. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E12 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| E-01 | E. Documentation | Correct README runtime claims and add architecture, data-flow, request-sequence, and trust-boundary diagrams. | [US12.01.01](#id-us12-01-01), [US12.03.01](#id-us12-03-01) | [E12](#id-e12) | P0 | Covered | Explicit review question E1. |
| E-02 | E. Documentation | Publish a generated API reference for every tool/resource/prompt with exact parameters, schemas, examples, units, freshness, cache, premium, errors, pagination, and next actions. | [US12.02.01](#id-us12-02-01), [US12.02.02](#id-us12-02-02) | [E12](#id-e12) | P1 | Covered | Explicit review question E2. |
| E-03 | E. Documentation | Add prioritized Chinese localization after English contracts and Unicode input behavior stabilize, with human review. | [US12.05.01](#id-us12-05-01) | [E12](#id-e12) | P2 | Covered | Explicit review question E3. |
| E-04 | E. Documentation | Create concise installation/Inspector and architecture video tutorials with transcripts and version ownership. | [US12.05.02](#id-us12-05-02) | [E12](#id-e12) | P2 | Covered | Explicit review question E4. |
| E-05 | E. Documentation | Improve contributor onboarding, resolve branch/planning drift, add ADRs and tool-addition guidance, and target a 15–30 minute first run. | [US12.04.01](#id-us12-04-01), [US12.04.02](#id-us12-04-02) | [E12](#id-e12) | P1 | Covered | Explicit review question E5. |
| I-01 | I. Prioritisation | Maintain a P0/P1/P2 roadmap with estimates, dependencies, milestones, and explicit release exit criteria. | [US12.04.02](#id-us12-04-02), [US15.05.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-05-01) | [E12](#id-e12) | P0 | Covered | Explicit review question I. |
| R-16 | Repository finding | Correct claims that search-tools keeps other schemas off the MCP tools/list wire unless actual dynamic tool-list behavior is implemented. | [US12.01.01](#id-us12-01-01), [US10.03.01](./E10-service-operations-resources-and-extensibility.md#id-us10-03-01), [US10.03.02](./E10-service-operations-resources-and-extensibility.md#id-us10-03-02) | [E12](#id-e12) | P0 | Covered | Documentation/context finding. |
| R-24 | Repository finding | Replace direct-construction 'live smoke' coverage with real SDK client initialization/list/call/resource/prompt transport tests and correct route/streaming/field claims. | [US01.03.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-03-01), [US12.01.01](#id-us12-01-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | [E01](./E01-mcp-transport-and-protocol-integrity.md#id-e01) | P0 | Covered | Testing/documentation finding. |
| RF-141 | Code-review detail | README transport route, SSE/streaming, and SDK mapping claims are inaccurate or ambiguous. | [US12.01.01](#id-us12-01-01), [US12.03.02](#id-us12-03-02), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-142 | Code-review detail | README overstates fields projection, limit behavior, confidence/exact match, lazy schemas, full/raw views, and live-smoke coverage. | [US12.01.01](#id-us12-01-01), [US12.02.01](#id-us12-02-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US14.02.01](./E14-ecosystem-positioning-adoption-and-community.md#id-us14-02-01) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-143 | Code-review detail | Documentation should be split into quick start, API, resources/prompts, transports, architecture, security, deployment, errors/quota, data accuracy, troubleshooting, compatibility, contributor, and ADR sections. | [US12.01.02](#id-us12-01-02), [US12.02.01](#id-us12-02-01), [US12.02.02](#id-us12-02-02), [US12.03.02](#id-us12-03-02), [US12.04.02](#id-us12-04-02) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-144 | Code-review detail | Every tool needs exact parameters, schemas, examples, units, freshness, cost, cache, premium, error, pagination/truncation, and next-actions documentation. | [US12.02.01](#id-us12-02-01), [US13.02.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-01) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-145 | Code-review detail | Three resources and three prompts need complete reference and corrected workflow semantics. | [US12.02.02](#id-us12-02-02), [US13.02.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-02) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-146 | Code-review detail | Financial semantics for price volatility, insider signal, news windows/sentiment, provenance, and prompt claims require accuracy documentation. | [US12.02.02](#id-us12-02-02), [US13.02.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-02) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-147 | Code-review detail | Clean Architecture, request/data flow, and security trust boundaries need diagrams. | [US12.03.01](#id-us12-03-01) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-148 | Code-review detail | Security, cloud-secret, transport, deployment, troubleshooting, and compatibility guidance are incomplete. | [US12.03.02](#id-us12-03-02), [US13.04.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-04-01) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-149 | Code-review detail | New-developer setup and first-tool contribution path should meet explicit onboarding time targets. | [US12.04.01](#id-us12-04-01) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-150 | Code-review detail | Contributor release-branch guidance drifts and ignored .planning files are treated as authoritative. | [US12.04.02](#id-us12-04-02), [US15.05.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-05-01) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-151 | Code-review detail | Chinese documentation should follow stable English and Unicode behavior, prioritize setup/security/errors, and be human reviewed. | [US12.05.01](#id-us12-05-01) | [E12](#id-e12) | P2 | Covered | Detailed finding retained from the repository review. |
| RF-152 | Code-review detail | Short install/Inspector and architecture videos plus executable examples would improve onboarding. | [US12.05.02](#id-us12-05-02), [US13.05.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-05-01) | [E12](#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-164 | Code-review detail | Current live smoke bypasses real transport, initialization, listings, serialization, middleware, CORS, auth, route, resource, and prompt behavior. | [US12.01.01](#id-us12-01-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | [E12](#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |

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

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E12 | Epic | — | P0 | Documentation Integrity and Developer Enablement | See [E12](#id-e12) | See [E12](#id-e12) | — | finnhub-mcp; epic | — | Not Started |
| F12.01 | Feature | [E12](#id-e12) | P0 | Documentation Truth and Information Architecture | See [F12.01](#id-f12-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e12 | — | Not Started |
| F12.02 | Feature | [E12](#id-e12) | P1 | Generated API and Contract Reference | See [F12.02](#id-f12-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e12 | — | Not Started |
| F12.03 | Feature | [E12](#id-e12) | P0 | Architecture, Operations, and Data Semantics | See [F12.03](#id-f12-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e12 | — | Not Started |
| F12.04 | Feature | [E12](#id-e12) | P1 | Contributor Onboarding and Decision Governance | See [F12.04](#id-f12-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e12 | — | Not Started |
| F12.05 | Feature | [E12](#id-e12) | P2 | Localization and Learning Media | See [F12.05](#id-f12-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e12 | — | Not Started |
| US12.01.01 | Story | [F12.01](#id-f12-01) | P0 | Correct README claims against executable behavior | See [US12.01.01](#id-us12-01-01) | See [US12.01.01](#id-us12-01-01) | 2d | documentation; transport; truthfulness; p0 | [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | Not Started |
| US12.01.02 | Story | [F12.01](#id-f12-01) | P1 | Split documentation into task-oriented sections | See [US12.01.02](#id-us12-01-02) | See [US12.01.02](#id-us12-01-02) | 4d | documentation; information-architecture; developer-experience | [US12.01.01](#id-us12-01-01) | Not Started |
| US12.02.01 | Story | [F12.02](#id-f12-02) | P1 | Generate a complete per-tool API reference | See [US12.02.01](#id-us12-02-01) | See [US12.02.01](#id-us12-02-01) | 5d | api-reference; code-generation; tools | [US12.01.02](#id-us12-01-02) | Not Started |
| US12.02.02 | Story | [F12.02](#id-f12-02) | P1 | Document resources, prompts, errors, quota, and output semantics | See [US12.02.02](#id-us12-02-02) | See [US12.02.02](#id-us12-02-02) | 4d | resources; prompts; errors; quota; data-accuracy | [US12.02.01](#id-us12-02-01) | Not Started |
| US12.03.01 | Story | [F12.03](#id-f12-03) | P1 | Publish architecture, request-flow, and trust-boundary diagrams | See [US12.03.01](#id-us12-03-01) | See [US12.03.01](#id-us12-03-01) | 3d | architecture; diagram; security; data-flow | [US12.01.02](#id-us12-01-02) | Not Started |
| US12.03.02 | Story | [F12.03](#id-f12-03) | P0 | Document secure transport and deployment profiles | See [US12.03.02](#id-us12-03-02) | See [US12.03.02](#id-us12-03-02) | 5d | security; deployment; transport; operations; p0 | [US12.03.01](#id-us12-03-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | Not Started |
| US12.04.01 | Story | [F12.04](#id-f12-04) | P1 | Create a verified contributor onboarding path | See [US12.04.01](#id-us12-04-01) | See [US12.04.01](#id-us12-04-01) | 4d | contributing; onboarding; developer-experience | [US12.03.01](#id-us12-03-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01) | Not Started |
| US12.04.02 | Story | [F12.04](#id-f12-04) | P1 | Establish ADR and release-policy governance | See [US12.04.02](#id-us12-04-02) | See [US12.04.02](#id-us12-04-02) | 3d | adr; governance; release; contributing | [US12.01.02](#id-us12-01-02) | Not Started |
| US12.05.01 | Story | [F12.05](#id-f12-05) | P2 | Launch reviewed Simplified Chinese documentation | See [US12.05.01](#id-us12-05-01) | See [US12.05.01](#id-us12-05-01) | 6d | i18n; zh-cn; documentation | [US12.01.02](#id-us12-01-02), [US12.03.02](#id-us12-03-02) | Not Started |
| US12.05.02 | Story | [F12.05](#id-f12-05) | P2 | Produce short tutorials and executable examples | See [US12.05.02](#id-us12-05-02) | See [US12.05.02](#id-us12-05-02) | 6d | video; examples; education; accessibility | [US12.02.01](#id-us12-02-01), [US12.03.02](#id-us12-03-02), [US13.02.01](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-02-01) | Not Started |
| ST12.01.01.01 | Sub-task | [US12.01.01](#id-us12-01-01) | P0 | Build a claim-to-code verification table | See [ST12.01.01.01](#id-st12-01-01-01) | Not applicable; see detail or parent section | 4h | finnhub-mcp; analysis | — | Not Started |
| ST12.01.01.02 | Sub-task | [US12.01.01](#id-us12-01-01) | P0 | Rewrite README setup and capability sections | See [ST12.01.01.02](#id-st12-01-01-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; documentation | — | Not Started |
| ST12.01.01.03 | Sub-task | [US12.01.01](#id-us12-01-01) | P0 | Add documentation truth checks | See [ST12.01.01.03](#id-st12-01-01-03) | Not applicable; see detail or parent section | 4h | finnhub-mcp; test | — | Not Started |
| ST12.01.02.01 | Sub-task | [US12.01.02](#id-us12-01-02) | P1 | Define documentation taxonomy and ownership | See [ST12.01.02.01](#id-st12-01-02-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST12.01.02.02 | Sub-task | [US12.01.02](#id-us12-01-02) | P1 | Extract canonical pages from README | See [ST12.01.02.02](#id-st12-01-02-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; documentation | — | Not Started |
| ST12.01.02.03 | Sub-task | [US12.01.02](#id-us12-01-02) | P1 | Validate navigation and version metadata | See [ST12.01.02.03](#id-st12-01-02-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; automation | — | Not Started |
| ST12.02.01.01 | Sub-task | [US12.02.01](#id-us12-02-01) | P1 | Define reference schema and page template | See [ST12.02.01.01](#id-st12-02-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST12.02.01.02 | Sub-task | [US12.02.01](#id-us12-02-01) | P1 | Implement catalogue-to-Markdown generator | See [ST12.02.01.02](#id-st12-02-01-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST12.02.01.03 | Sub-task | [US12.02.01](#id-us12-02-01) | P1 | Add reference drift gates and example review | See [ST12.02.01.03](#id-st12-02-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST12.02.02.01 | Sub-task | [US12.02.02](#id-us12-02-02) | P1 | Author resource and prompt reference pages | See [ST12.02.02.01](#id-st12-02-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; documentation | — | Not Started |
| ST12.02.02.02 | Sub-task | [US12.02.02](#id-us12-02-02) | P1 | Author error and quota contract | See [ST12.02.02.02](#id-st12-02-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; documentation | — | Not Started |
| ST12.02.02.03 | Sub-task | [US12.02.02](#id-us12-02-02) | P1 | Review financial data-accuracy notes | See [ST12.02.02.03](#id-st12-02-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; domain-review | — | Not Started |
| ST12.03.01.01 | Sub-task | [US12.03.01](#id-us12-03-01) | P1 | Create Clean Architecture component diagram | See [ST12.03.01.01](#id-st12-03-01-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST12.03.01.02 | Sub-task | [US12.03.01](#id-us12-03-01) | P1 | Create request and quota sequence diagram | See [ST12.03.01.02](#id-st12-03-01-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; design | — | Not Started |
| ST12.03.01.03 | Sub-task | [US12.03.01](#id-us12-03-01) | P1 | Create trust-boundary and deployment diagram | See [ST12.03.01.03](#id-st12-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; security-design | — | Not Started |
| ST12.03.02.01 | Sub-task | [US12.03.02](#id-us12-03-02) | P0 | Write transport and compatibility guides | See [ST12.03.02.01](#id-st12-03-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; documentation | — | Not Started |
| ST12.03.02.02 | Sub-task | [US12.03.02](#id-us12-03-02) | P0 | Write security and secret-management profiles | See [ST12.03.02.02](#id-st12-03-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; security-documentation | — | Not Started |
| ST12.03.02.03 | Sub-task | [US12.03.02](#id-us12-03-02) | P0 | Write deployment and troubleshooting runbooks | See [ST12.03.02.03](#id-st12-03-02-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; documentation | — | Not Started |
| ST12.04.01.01 | Sub-task | [US12.04.01](#id-us12-04-01) | P1 | Build clean-environment setup tutorial | See [ST12.04.01.01](#id-st12-04-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; documentation | — | Not Started |
| ST12.04.01.02 | Sub-task | [US12.04.01](#id-us12-04-01) | P1 | Build first-tool contribution tutorial | See [ST12.04.01.02](#id-st12-04-01-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; documentation | — | Not Started |
| ST12.04.01.03 | Sub-task | [US12.04.01](#id-us12-04-01) | P1 | Continuously test onboarding | See [ST12.04.01.03](#id-st12-04-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; automation | — | Not Started |
| ST12.04.02.01 | Sub-task | [US12.04.02](#id-us12-04-02) | P1 | Create ADR template, index, and initial records | See [ST12.04.02.01](#id-st12-04-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; governance | — | Not Started |
| ST12.04.02.02 | Sub-task | [US12.04.02](#id-us12-04-02) | P1 | Resolve branch and planning-source drift | See [ST12.04.02.02](#id-st12-04-02-02) | Not applicable; see detail or parent section | 6h | finnhub-mcp; documentation | — | Not Started |
| ST12.04.02.03 | Sub-task | [US12.04.02](#id-us12-04-02) | P1 | Automate governance drift review | See [ST12.04.02.03](#id-st12-04-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; automation | — | Not Started |
| ST12.05.01.01 | Sub-task | [US12.05.01](#id-us12-05-01) | P2 | Define Chinese localization glossary and workflow | See [ST12.05.01.01](#id-st12-05-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; localization-design | — | Not Started |
| ST12.05.01.02 | Sub-task | [US12.05.01](#id-us12-05-01) | P2 | Translate and review phase-one pages | See [ST12.05.01.02](#id-st12-05-01-02) | Not applicable; see detail or parent section | 24h | finnhub-mcp; localization | — | Not Started |
| ST12.05.01.03 | Sub-task | [US12.05.01](#id-us12-05-01) | P2 | Add localization freshness checks | See [ST12.05.01.03](#id-st12-05-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; automation | — | Not Started |
| ST12.05.02.01 | Sub-task | [US12.05.02](#id-us12-05-02) | P2 | Script and record core tutorials | See [ST12.05.02.01](#id-st12-05-02-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; media-production | — | Not Started |
| ST12.05.02.02 | Sub-task | [US12.05.02](#id-us12-05-02) | P2 | Create accessible transcripts and written equivalents | See [ST12.05.02.02](#id-st12-05-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; documentation | — | Not Started |
| ST12.05.02.03 | Sub-task | [US12.05.02](#id-us12-05-02) | P2 | Validate all copy-paste examples | See [ST12.05.02.03](#id-st12-05-02-03) | Not applicable; see detail or parent section | 12h | finnhub-mcp; test | — | Not Started |

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

- [ ] Create E12 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E12.
- [ ] Create all 10 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 30 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 21 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 46 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

