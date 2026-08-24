---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E14
title: "Ecosystem Positioning, Adoption, and Community"
priority: P1
phase: "M2 — Operability & Adoption"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 3
  user_stories: 6
  subtasks: 18
  traceability_owned: 6
  traceability_items: 7
story_estimate_days: 22
subtask_estimate_hours: 145
---

<a id="id-e14"></a>
# E14 — Ecosystem Positioning, Adoption, and Community

This is the self-contained coding-agent backlog for E14. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E14 | P1 | 3 | 6 | 18 | 22 | 145 | M2 — Operability & Adoption | Not Started |

## 2. Epic Definition

**Objective:** Position finnhub-mcp accurately against open-source and official alternatives and create a measurable, maintainable community adoption path.

**Business value:** Helps users choose the right server, focuses the roadmap on defensible strengths, and converts interest into sustained usage and contribution.

**Exit criteria:**

- [ ] A dated, source-linked comparison covers NimbleBrainInc/mcp-finnhub, cfdude/mcp-finnhub, and the official Finnhub MCP server.
- [ ] Public positioning states both strengths and limitations and is consistent across README, website, repository metadata, and release content.
- [ ] Contribution and adoption funnels have named owners, response expectations, measurable events, and privacy-safe reporting.
- [ ] Comparison evidence and product claims are revalidated on a documented cadence.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F14.01](#id-f14-01) | P1 | Evidence-Based Competitive Comparison | 2 | 7 | Not Started |
| [F14.02](#id-f14-02) | P1 | Product Positioning and Decision Guidance | 2 | 7 | Not Started |
| [F14.03](#id-f14-03) | P2 | Community and Adoption Operations | 2 | 8 | Not Started |

<a id="id-f14-01"></a>
### F14.01 — Evidence-Based Competitive Comparison

- **Parent Epic:** [E14](#id-e14)
- **Priority:** P1
- **Status:** Not Started

**Description:** Maintain a fair comparison of feature breadth, architecture, transport, security, quota controls, context engineering, and operating model.

**Expected outcome:** Evaluation decisions are grounded in dated evidence rather than marketing claims.

**Stories:**

- [US14.01.01](#id-us14-01-01) — Publish a sourced competitor capability matrix (P1, 4d)
- [US14.01.02](#id-us14-01-02) — Automate recurring comparison evidence refresh (P2, 3d)

<a id="id-f14-02"></a>
### F14.02 — Product Positioning and Decision Guidance

- **Parent Epic:** [E14](#id-e14)
- **Priority:** P1
- **Status:** Not Started

**Description:** Define the project's defensible category and help users choose between self-hosted, broad-operation, and official hosted alternatives.

**Expected outcome:** The project attracts users whose needs match its strengths and sets honest expectations about breadth and hosted maturity.

**Stories:**

- [US14.02.01](#id-us14-02-01) — Define and apply a defensible product position (P1, 3d)
- [US14.02.02](#id-us14-02-02) — Publish an adoption and migration decision guide (P1, 4d)

<a id="id-f14-03"></a>
### F14.03 — Community and Adoption Operations

- **Parent Epic:** [E14](#id-e14)
- **Priority:** P2
- **Status:** Not Started

**Description:** Create contribution funnels, feedback loops, release communications, and privacy-safe adoption measurement.

**Expected outcome:** Interest turns into successful installs, useful issues, repeat contributors, and measurable retention.

**Stories:**

- [US14.03.01](#id-us14-03-01) — Create an accountable community contribution funnel (P2, 4d)
- [US14.03.02](#id-us14-03-02) — Measure and improve the privacy-safe adoption funnel (P2, 4d)

## 4. User Stories and Subtasks

<a id="id-us14-01-01"></a>
### US14.01.01 — Publish a sourced competitor capability matrix

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.01](#id-f14-01) | P1 | 4 | 28 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Technical decision maker

**User story:** As a decision maker, I want a dated, fair comparison so that I can choose the server whose breadth, governance, and operating model fit my use case.

**Acceptance criteria:**

- [ ] The matrix compares finnhub-mcp, NimbleBrainInc/mcp-finnhub, cfdude/mcp-finnhub, and the official Finnhub MCP server using source-linked commits or official pages and a visible review date.
- [ ] Dimensions include implementation language, license/source availability, deployment model, transport, authentication, tool/operation/resource/prompt counts, historical data, indicators, dividends, filings, batch/jobs/export, caching, quota, retry/circuit, tests, context discovery, and extensibility.
- [ ] The finnhub-mcp row states its strong Clean Architecture, typed C# contracts, 12 tools/3 resources/3 prompts, search-tools, context-aware response intent, distribution, and tests as well as current hosted security, HTTP-route, breadth, and financial-semantic limitations.
- [ ] The NimbleBrain row reconciles the source-visible 10 direct tools plus 1 resource with older README counts and notes per-tool optional API-key exposure and limited cache/limiter/retry/auth controls without speculative claims.
- [ ] The cfdude and official rows distinguish cfdude's broad 53 operations/15 tools and management jobs from the official hosted Streamable HTTP/OAuth service with approximately 70 tools, while clearly labeling snapshot dates and unverifiable implementation details.

**Dependencies:** [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01)

**Labels:** `comparison` `competitive-analysis` `documentation`

**Source findings:**

- finnhub-mcp has the strongest reviewed open-source architecture and agent UX but less breadth and weak hosted readiness.
- NimbleBrain source exposes 10 direct tools and 1 resource while its README count is stale and it lacks comparable operational controls.
- cfdude exposes 15 tools/53 operations including OHLCV, indicators, dividends, filings, estimates, crypto/FX, export and jobs, but uses a less discoverable generic-operation surface and has configuration/documentation drift.
- The official Finnhub MCP server is hosted, Streamable HTTP, OAuth, and broad, but its implementation is not public.

**Subtasks:**

<a id="id-st14-01-01-01"></a>
- [ ] **ST14.01.01.01 — Capture immutable competitor evidence**
  - Type: research
  - Estimate: 10 hours
  - Suggested owner role: Technical product manager
  - Deliverable/evidence: Dated commit/source evidence pack for all four compared servers.
  - Status: Not Started
<a id="id-st14-01-01-02"></a>
- [ ] **ST14.01.01.02 — Build and fact-review capability matrix**
  - Type: documentation
  - Estimate: 12 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Balanced source-linked matrix with counts, strengths, weaknesses, unknowns, and snapshot date.
  - Status: Not Started
<a id="id-st14-01-01-03"></a>
- [ ] **ST14.01.01.03 — Publish comparison in repository and site**
  - Type: implementation
  - Estimate: 6 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Accessible comparison page and concise README link using canonical data.
  - Status: Not Started

<a id="id-us14-01-02"></a>
### US14.01.02 — Automate recurring comparison evidence refresh

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.01](#id-f14-01) | P2 | 3 | 18 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Product maintainer

**User story:** As a product maintainer, I want comparison evidence revalidated on a schedule so that changing repositories and official services do not leave stale claims on the site.

**Acceptance criteria:**

- [ ] A quarterly workflow opens a review issue containing pinned comparison sources, prior snapshots, and an owner checklist rather than silently rewriting claims.
- [ ] Machine-readable evidence records repository commit, release, retrieval date, counted registrations, transport/auth claims, and source URL for each product.
- [ ] Automated checks flag count or README/source divergence but require human review before publishing a competitive claim.
- [ ] The comparison page shows last verified date, methodology, unknown/not-public states, and a correction channel.
- [ ] Claims older than the configured review window display a stale badge until revalidated.

**Dependencies:** [US14.01.01](#id-us14-01-01)

**Labels:** `comparison` `automation` `governance`

**Source findings:**

- Competitor counts and implementations have already drifted from their READMEs and require dated source-level verification.
- The official hosted service can evolve independently and should not be described without a review date.

**Subtasks:**

<a id="id-st14-01-02-01"></a>
- [ ] **ST14.01.02.01 — Define comparison evidence schema**
  - Type: design
  - Estimate: 5 hours
  - Suggested owner role: Data engineer
  - Deliverable/evidence: Machine-readable schema for commit, date, counts, claims, sources, unknowns, and review status.
  - Status: Not Started
<a id="id-st14-01-02-02"></a>
- [ ] **ST14.01.02.02 — Automate stale-evidence review issues**
  - Type: automation
  - Estimate: 8 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Quarterly workflow that snapshots change signals and assigns human review.
  - Status: Not Started
<a id="id-st14-01-02-03"></a>
- [ ] **ST14.01.02.03 — Add freshness and correction UX**
  - Type: implementation
  - Estimate: 5 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Last-verified/stale badges, methodology, source links, and correction form.
  - Status: Not Started

<a id="id-us14-02-01"></a>
### US14.02.01 — Define and apply a defensible product position

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.02](#id-f14-02) | P1 | 3 | 19 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Project owner

**User story:** As the project owner, I want a specific and honest product position so that roadmap and messaging reinforce the same differentiated value.

**Acceptance criteria:**

- [ ] The primary position is approved as an open-source, self-hosted, token- and quota-efficient Finnhub research server for governed AI agents, or an explicitly documented replacement.
- [ ] Messaging names target users, core jobs, differentiators, evidence, non-goals, hosted-readiness status, plan/licensing dependency, and alternatives for broader managed use cases.
- [ ] README, repository description/topics, website Home, Comparison, architecture docs, and release templates use the same reviewed terminology and current counts.
- [ ] A decision guide recommends finnhub-mcp for typed self-hosting and research workflows, cfdude for broad operations, the official server for managed breadth/OAuth, and NimbleBrain only where its simpler Python surface fits, with dated caveats.
- [ ] No claim uses terms such as real-time streaming, secure hosted, confidence probability, full raw, or lazy schema loading unless backed by an acceptance test.

**Dependencies:** [US14.01.01](#id-us14-01-01)

**Labels:** `positioning` `messaging` `product-strategy`

**Source findings:**

- Recommended position: open-source, self-hosted, token- and quota-efficient Finnhub research server for governed AI agents.
- Public claims must reflect the difference between architectural potential, local STDIO quality, and current hosted HTTP maturity.

**Subtasks:**

<a id="id-st14-02-01-01"></a>
- [ ] **ST14.02.01.01 — Define audience, jobs, differentiators, and non-goals**
  - Type: product-strategy
  - Estimate: 6 hours
  - Suggested owner role: Product manager
  - Deliverable/evidence: Approved positioning brief grounded in comparison and repository evidence.
  - Status: Not Started
<a id="id-st14-02-01-02"></a>
- [ ] **ST14.02.01.02 — Create claims and terminology guardrail**
  - Type: governance
  - Estimate: 5 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Approved/banned claim lexicon with evidence and test requirements.
  - Status: Not Started
<a id="id-st14-02-01-03"></a>
- [ ] **ST14.02.01.03 — Apply positioning across public surfaces**
  - Type: documentation
  - Estimate: 8 hours
  - Suggested owner role: Developer marketer
  - Deliverable/evidence: Consistent README, repository metadata, site, architecture, comparison, and release messaging.
  - Status: Not Started

<a id="id-us14-02-02"></a>
### US14.02.02 — Publish an adoption and migration decision guide

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.02](#id-f14-02) | P1 | 4 | 30 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Existing Finnhub MCP user

**User story:** As an existing user of another Finnhub MCP server, I want an explicit decision and migration guide so that I can evaluate switching cost and behavioral differences safely.

**Acceptance criteria:**

- [ ] The guide compares setup, credentials, transport, tool naming, schema shape, pagination, errors, quota behavior, caching, premium requirements, and deployment responsibility.
- [ ] Migration examples map common quote, profile, search, peer, candle, indicator, dividend, filing, and batch needs to current finnhub-mcp support or clearly identified roadmap gaps.
- [ ] The guide explains when the official hosted service is preferable and never implies that self-hosted operation bypasses Finnhub plans, licensing, or quotas.
- [ ] Compatibility examples are versioned and validated where both public implementations can be exercised.
- [ ] The page includes rollback steps and a channel for corrections or missing scenarios.

**Dependencies:** [US14.01.01](#id-us14-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01)

**Labels:** `migration` `adoption` `comparison`

**Source findings:**

- Competitors differ materially in direct tools versus generic operations, breadth, transport, authentication, and hosted responsibility.
- finnhub-mcp currently lacks several high-value data surfaces and should state gaps during migration planning.

**Subtasks:**

<a id="id-st14-02-02-01"></a>
- [ ] **ST14.02.02.01 — Map competitor contracts and common use cases**
  - Type: analysis
  - Estimate: 10 hours
  - Suggested owner role: Solutions engineer
  - Deliverable/evidence: Versioned mapping of setup, tools/operations, responses, errors, quota, and supported/gap scenarios.
  - Status: Not Started
<a id="id-st14-02-02-02"></a>
- [ ] **ST14.02.02.02 — Author decision and migration guide**
  - Type: documentation
  - Estimate: 12 hours
  - Suggested owner role: Technical writer
  - Deliverable/evidence: Honest choose/migrate/rollback guide with plan and licensing caveats.
  - Status: Not Started
<a id="id-st14-02-02-03"></a>
- [ ] **ST14.02.02.03 — Validate cross-server examples**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Integration test engineer
  - Deliverable/evidence: Pinned executable examples where public endpoints/source permit and explicit unverified markers elsewhere.
  - Status: Not Started

<a id="id-us14-03-01"></a>
### US14.03.01 — Create an accountable community contribution funnel

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.03](#id-f14-03) | P2 | 4 | 24 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Potential contributor

**User story:** As a potential contributor, I want clear entry points and response expectations so that I can propose features or fixes without guessing how the project is governed.

**Acceptance criteria:**

- [ ] Issue forms exist for bugs, tool proposals, financial-semantic questions, security redirects, documentation, and ranking-evaluation cases with required reproduction and version fields.
- [ ] A public roadmap maps P0/P1/P2 work to epics, identifies accepted contribution areas, and labels bounded good-first and help-wanted issues.
- [ ] CODE_OF_CONDUCT, governance, maintainer roles, triage cadence, response targets, support boundary, and security-reporting route are published.
- [ ] A contributor dashboard reports privacy-safe issue response, first-PR completion, review time, and repeat-contributor measures without ranking individuals.
- [ ] Quarterly community notes document accepted feedback, rejected proposals with rationale, and upcoming contribution opportunities.

**Dependencies:** [US12.04.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-01), [US12.04.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-02), [US15.03.02](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-03-02)

**Labels:** `community` `governance` `contributing`

**Source findings:**

- A showcase and richer contributor documentation should be paired with clear contribution and governance paths.
- Tool, financial-semantic, ranking, and security changes require different review information.

**Subtasks:**

<a id="id-st14-03-01-01"></a>
- [ ] **ST14.03.01.01 — Implement specialized issue and PR forms**
  - Type: community-operations
  - Estimate: 8 hours
  - Suggested owner role: Community maintainer
  - Deliverable/evidence: Forms and templates for bugs, tools, domain semantics, security, docs, and ranking cases.
  - Status: Not Started
<a id="id-st14-03-01-02"></a>
- [ ] **ST14.03.01.02 — Publish governance, conduct, roadmap, and support policy**
  - Type: governance
  - Estimate: 10 hours
  - Suggested owner role: Project maintainer
  - Deliverable/evidence: Community policies, role/triage model, contribution areas, and curated entry issues.
  - Status: Not Started
<a id="id-st14-03-01-03"></a>
- [ ] **ST14.03.01.03 — Create privacy-safe community review cadence**
  - Type: operations
  - Estimate: 6 hours
  - Suggested owner role: Community manager
  - Deliverable/evidence: Aggregate contributor dashboard and quarterly decision/opportunity note process.
  - Status: Not Started

<a id="id-us14-03-02"></a>
### US14.03.02 — Measure and improve the privacy-safe adoption funnel

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F14.03](#id-f14-03) | P2 | 4 | 26 | M2 — Operability & Adoption | Not Started | Unassigned |

**Persona:** Product maintainer

**User story:** As a product maintainer, I want privacy-safe adoption signals so that I can improve discovery and onboarding without collecting financial intents or ticker histories unnecessarily.

**Acceptance criteria:**

- [ ] The funnel defines discover, quick-start, successful local initialize, first tool call, repeat use, documentation return, and contribution events with owner and target conversion.
- [ ] Website analytics are consent-aware and collect page and aggregate interaction events without raw intents, symbols, API keys, user identifiers, or response payloads by default.
- [ ] CLI/server telemetry remains off by default and any future opt-in telemetry has published schema, retention, deletion, and local disable controls.
- [ ] Monthly review combines privacy-safe analytics, documentation search misses, anonymized support themes, release adoption, and opt-in ranking feedback into prioritized experiments.
- [ ] Release notes, blog posts, sample workflows, and social cards have trackable but non-identifying campaign links and named refresh owners.

**Dependencies:** [US13.01.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-01-02), [US14.02.01](#id-us14-02-01)

**Labels:** `adoption` `analytics` `privacy` `community`

**Source findings:**

- Search queries, ticker/watch history, and user activity can be more sensitive than public insider names and should not become high-cardinality telemetry.
- Blog/changelog, examples, and site funnels should support measurable adoption without compromising privacy.

**Subtasks:**

<a id="id-st14-03-02-01"></a>
- [ ] **ST14.03.02.01 — Define funnel events and privacy model**
  - Type: analytics-design
  - Estimate: 8 hours
  - Suggested owner role: Privacy engineer
  - Deliverable/evidence: Event catalogue, consent basis, excluded sensitive fields, retention, ownership, and target conversions.
  - Status: Not Started
<a id="id-st14-03-02-02"></a>
- [ ] **ST14.03.02.02 — Implement consent-aware website analytics**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Aggregate page/funnel analytics that exclude raw financial intents, symbols, keys, users, and payloads.
  - Status: Not Started
<a id="id-st14-03-02-03"></a>
- [ ] **ST14.03.02.03 — Establish experiment and content review**
  - Type: product-operations
  - Estimate: 8 hours
  - Suggested owner role: Product manager
  - Deliverable/evidence: Monthly funnel review and owned release/blog/workflow campaign calendar.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST14.01.01.01](#id-st14-01-01-01) | [US14.01.01](#id-us14-01-01) | P1 | Capture immutable competitor evidence | research | 10 | Technical product manager | Dated commit/source evidence pack for all four compared servers. | Not Started |
| [ST14.01.01.02](#id-st14-01-01-02) | [US14.01.01](#id-us14-01-01) | P1 | Build and fact-review capability matrix | documentation | 12 | Technical writer | Balanced source-linked matrix with counts, strengths, weaknesses, unknowns, and snapshot date. | Not Started |
| [ST14.01.01.03](#id-st14-01-01-03) | [US14.01.01](#id-us14-01-01) | P1 | Publish comparison in repository and site | implementation | 6 | Web engineer | Accessible comparison page and concise README link using canonical data. | Not Started |
| [ST14.01.02.01](#id-st14-01-02-01) | [US14.01.02](#id-us14-01-02) | P2 | Define comparison evidence schema | design | 5 | Data engineer | Machine-readable schema for commit, date, counts, claims, sources, unknowns, and review status. | Not Started |
| [ST14.01.02.02](#id-st14-01-02-02) | [US14.01.02](#id-us14-01-02) | P2 | Automate stale-evidence review issues | automation | 8 | DevOps engineer | Quarterly workflow that snapshots change signals and assigns human review. | Not Started |
| [ST14.01.02.03](#id-st14-01-02-03) | [US14.01.02](#id-us14-01-02) | P2 | Add freshness and correction UX | implementation | 5 | Frontend engineer | Last-verified/stale badges, methodology, source links, and correction form. | Not Started |
| [ST14.02.01.01](#id-st14-02-01-01) | [US14.02.01](#id-us14-02-01) | P1 | Define audience, jobs, differentiators, and non-goals | product-strategy | 6 | Product manager | Approved positioning brief grounded in comparison and repository evidence. | Not Started |
| [ST14.02.01.02](#id-st14-02-01-02) | [US14.02.01](#id-us14-02-01) | P1 | Create claims and terminology guardrail | governance | 5 | Technical writer | Approved/banned claim lexicon with evidence and test requirements. | Not Started |
| [ST14.02.01.03](#id-st14-02-01-03) | [US14.02.01](#id-us14-02-01) | P1 | Apply positioning across public surfaces | documentation | 8 | Developer marketer | Consistent README, repository metadata, site, architecture, comparison, and release messaging. | Not Started |
| [ST14.02.02.01](#id-st14-02-02-01) | [US14.02.02](#id-us14-02-02) | P1 | Map competitor contracts and common use cases | analysis | 10 | Solutions engineer | Versioned mapping of setup, tools/operations, responses, errors, quota, and supported/gap scenarios. | Not Started |
| [ST14.02.02.02](#id-st14-02-02-02) | [US14.02.02](#id-us14-02-02) | P1 | Author decision and migration guide | documentation | 12 | Technical writer | Honest choose/migrate/rollback guide with plan and licensing caveats. | Not Started |
| [ST14.02.02.03](#id-st14-02-02-03) | [US14.02.02](#id-us14-02-02) | P1 | Validate cross-server examples | test | 8 | Integration test engineer | Pinned executable examples where public endpoints/source permit and explicit unverified markers elsewhere. | Not Started |
| [ST14.03.01.01](#id-st14-03-01-01) | [US14.03.01](#id-us14-03-01) | P2 | Implement specialized issue and PR forms | community-operations | 8 | Community maintainer | Forms and templates for bugs, tools, domain semantics, security, docs, and ranking cases. | Not Started |
| [ST14.03.01.02](#id-st14-03-01-02) | [US14.03.01](#id-us14-03-01) | P2 | Publish governance, conduct, roadmap, and support policy | governance | 10 | Project maintainer | Community policies, role/triage model, contribution areas, and curated entry issues. | Not Started |
| [ST14.03.01.03](#id-st14-03-01-03) | [US14.03.01](#id-us14-03-01) | P2 | Create privacy-safe community review cadence | operations | 6 | Community manager | Aggregate contributor dashboard and quarterly decision/opportunity note process. | Not Started |
| [ST14.03.02.01](#id-st14-03-02-01) | [US14.03.02](#id-us14-03-02) | P2 | Define funnel events and privacy model | analytics-design | 8 | Privacy engineer | Event catalogue, consent basis, excluded sensitive fields, retention, ownership, and target conversions. | Not Started |
| [ST14.03.02.02](#id-st14-03-02-02) | [US14.03.02](#id-us14-03-02) | P2 | Implement consent-aware website analytics | implementation | 10 | Web engineer | Aggregate page/funnel analytics that exclude raw financial intents, symbols, keys, users, and payloads. | Not Started |
| [ST14.03.02.03](#id-st14-03-02-03) | [US14.03.02](#id-us14-03-02) | P2 | Establish experiment and content review | product-operations | 8 | Product manager | Monthly funnel review and owned release/blog/workflow campaign calendar. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E14 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| H-01 | H. Comparative Analysis | Operationalize comparative strengths and weaknesses versus NimbleBrainInc/mcp-finnhub, cfdude/mcp-finnhub, and Finnhub's official hosted MCP server. | [US14.01.01](#id-us14-01-01), [US14.01.02](#id-us14-01-02), [US14.02.01](#id-us14-02-01), [US14.02.02](#id-us14-02-02) | [E14](#id-e14) | P1 | Covered | Explicit review question H. |
| R-29 | Repository finding | Position the project as the open-source, self-hosted, token- and quota-efficient governed Finnhub research server rather than competing only on raw tool count. | [US14.02.01](#id-us14-02-01) | [E14](#id-e14) | P1 | Covered | Comparative strategy finding. |
| R-30 | Repository finding | Document the trade-off versus Finnhub's official hosted OAuth server with roughly 70 tools: transparency/control versus breadth/managed operations. | [US14.01.01](#id-us14-01-01), [US14.02.02](#id-us14-02-02) | [E14](#id-e14) | P1 | Covered | Comparative strategy finding. |
| RF-142 | Code-review detail | README overstates fields projection, limit behavior, confidence/exact match, lazy schemas, full/raw views, and live-smoke coverage. | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US14.02.01](#id-us14-02-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-160 | Code-review detail | Comparison must fairly cover NimbleBrain, cfdude, official Finnhub, and finnhub-mcp strengths and weaknesses with dated sources. | [US14.01.01](#id-us14-01-01), [US14.01.02](#id-us14-01-02), [US14.02.02](#id-us14-02-02) | [E14](#id-e14) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-161 | Code-review detail | Recommended positioning is an open-source, self-hosted, token- and quota-efficient Finnhub research server for governed AI agents. | [US14.02.01](#id-us14-02-01) | [E14](#id-e14) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-162 | Code-review detail | Community contribution, governance, release content, and privacy-safe adoption measurement should support sustained adoption. | [US14.03.01](#id-us14-03-01), [US14.03.02](#id-us14-03-02) | [E14](#id-e14) | P2 | Covered | Detailed finding retained from the repository review. |

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

## 8. Issue Import Manifest

This is the flattened issue-tracker projection for this Epic. Description and acceptance-criteria cells link to the authoritative sections in this file.

| Issue ID | Issue Type | Parent ID | Priority | Summary | Description | Acceptance Criteria | Original Estimate | Labels | Dependencies | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| E14 | Epic | — | P1 | Ecosystem Positioning, Adoption, and Community | See [E14](#id-e14) | See [E14](#id-e14) | — | finnhub-mcp; epic | — | Not Started |
| F14.01 | Feature | [E14](#id-e14) | P1 | Evidence-Based Competitive Comparison | See [F14.01](#id-f14-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e14 | — | Not Started |
| F14.02 | Feature | [E14](#id-e14) | P1 | Product Positioning and Decision Guidance | See [F14.02](#id-f14-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e14 | — | Not Started |
| F14.03 | Feature | [E14](#id-e14) | P2 | Community and Adoption Operations | See [F14.03](#id-f14-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e14 | — | Not Started |
| US14.01.01 | Story | [F14.01](#id-f14-01) | P1 | Publish a sourced competitor capability matrix | See [US14.01.01](#id-us14-01-01) | See [US14.01.01](#id-us14-01-01) | 4d | comparison; competitive-analysis; documentation | [US12.01.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-01) | Not Started |
| US14.01.02 | Story | [F14.01](#id-f14-01) | P2 | Automate recurring comparison evidence refresh | See [US14.01.02](#id-us14-01-02) | See [US14.01.02](#id-us14-01-02) | 3d | comparison; automation; governance | [US14.01.01](#id-us14-01-01) | Not Started |
| US14.02.01 | Story | [F14.02](#id-f14-02) | P1 | Define and apply a defensible product position | See [US14.02.01](#id-us14-02-01) | See [US14.02.01](#id-us14-02-01) | 3d | positioning; messaging; product-strategy | [US14.01.01](#id-us14-01-01) | Not Started |
| US14.02.02 | Story | [F14.02](#id-f14-02) | P1 | Publish an adoption and migration decision guide | See [US14.02.02](#id-us14-02-02) | See [US14.02.02](#id-us14-02-02) | 4d | migration; adoption; comparison | [US14.01.01](#id-us14-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01) | Not Started |
| US14.03.01 | Story | [F14.03](#id-f14-03) | P2 | Create an accountable community contribution funnel | See [US14.03.01](#id-us14-03-01) | See [US14.03.01](#id-us14-03-01) | 4d | community; governance; contributing | [US12.04.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-01), [US12.04.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-04-02), [US15.03.02](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-03-02) | Not Started |
| US14.03.02 | Story | [F14.03](#id-f14-03) | P2 | Measure and improve the privacy-safe adoption funnel | See [US14.03.02](#id-us14-03-02) | See [US14.03.02](#id-us14-03-02) | 4d | adoption; analytics; privacy; community | [US13.01.02](./E13-showcase-website-and-safe-interactive-experience.md#id-us13-01-02), [US14.02.01](#id-us14-02-01) | Not Started |
| ST14.01.01.01 | Sub-task | [US14.01.01](#id-us14-01-01) | P1 | Capture immutable competitor evidence | See [ST14.01.01.01](#id-st14-01-01-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; research | — | Not Started |
| ST14.01.01.02 | Sub-task | [US14.01.01](#id-us14-01-01) | P1 | Build and fact-review capability matrix | See [ST14.01.01.02](#id-st14-01-01-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; documentation | — | Not Started |
| ST14.01.01.03 | Sub-task | [US14.01.01](#id-us14-01-01) | P1 | Publish comparison in repository and site | See [ST14.01.01.03](#id-st14-01-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; implementation | — | Not Started |
| ST14.01.02.01 | Sub-task | [US14.01.02](#id-us14-01-02) | P2 | Define comparison evidence schema | See [ST14.01.02.01](#id-st14-01-02-01) | Not applicable; see detail or parent section | 5h | finnhub-mcp; design | — | Not Started |
| ST14.01.02.02 | Sub-task | [US14.01.02](#id-us14-01-02) | P2 | Automate stale-evidence review issues | See [ST14.01.02.02](#id-st14-01-02-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; automation | — | Not Started |
| ST14.01.02.03 | Sub-task | [US14.01.02](#id-us14-01-02) | P2 | Add freshness and correction UX | See [ST14.01.02.03](#id-st14-01-02-03) | Not applicable; see detail or parent section | 5h | finnhub-mcp; implementation | — | Not Started |
| ST14.02.01.01 | Sub-task | [US14.02.01](#id-us14-02-01) | P1 | Define audience, jobs, differentiators, and non-goals | See [ST14.02.01.01](#id-st14-02-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; product-strategy | — | Not Started |
| ST14.02.01.02 | Sub-task | [US14.02.01](#id-us14-02-01) | P1 | Create claims and terminology guardrail | See [ST14.02.01.02](#id-st14-02-01-02) | Not applicable; see detail or parent section | 5h | finnhub-mcp; governance | — | Not Started |
| ST14.02.01.03 | Sub-task | [US14.02.01](#id-us14-02-01) | P1 | Apply positioning across public surfaces | See [ST14.02.01.03](#id-st14-02-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; documentation | — | Not Started |
| ST14.02.02.01 | Sub-task | [US14.02.02](#id-us14-02-02) | P1 | Map competitor contracts and common use cases | See [ST14.02.02.01](#id-st14-02-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; analysis | — | Not Started |
| ST14.02.02.02 | Sub-task | [US14.02.02](#id-us14-02-02) | P1 | Author decision and migration guide | See [ST14.02.02.02](#id-st14-02-02-02) | Not applicable; see detail or parent section | 12h | finnhub-mcp; documentation | — | Not Started |
| ST14.02.02.03 | Sub-task | [US14.02.02](#id-us14-02-02) | P1 | Validate cross-server examples | See [ST14.02.02.03](#id-st14-02-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST14.03.01.01 | Sub-task | [US14.03.01](#id-us14-03-01) | P2 | Implement specialized issue and PR forms | See [ST14.03.01.01](#id-st14-03-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; community-operations | — | Not Started |
| ST14.03.01.02 | Sub-task | [US14.03.01](#id-us14-03-01) | P2 | Publish governance, conduct, roadmap, and support policy | See [ST14.03.01.02](#id-st14-03-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; governance | — | Not Started |
| ST14.03.01.03 | Sub-task | [US14.03.01](#id-us14-03-01) | P2 | Create privacy-safe community review cadence | See [ST14.03.01.03](#id-st14-03-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; operations | — | Not Started |
| ST14.03.02.01 | Sub-task | [US14.03.02](#id-us14-03-02) | P2 | Define funnel events and privacy model | See [ST14.03.02.01](#id-st14-03-02-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; analytics-design | — | Not Started |
| ST14.03.02.02 | Sub-task | [US14.03.02](#id-us14-03-02) | P2 | Implement consent-aware website analytics | See [ST14.03.02.02](#id-st14-03-02-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST14.03.02.03 | Sub-task | [US14.03.02](#id-us14-03-02) | P2 | Establish experiment and content review | See [ST14.03.02.03](#id-st14-03-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; product-operations | — | Not Started |

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

- [ ] Create E14 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 3 Features under E14.
- [ ] Create all 6 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 18 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 7 relevant traceability rows covered.
- [ ] Satisfy all 2 relevant roadmap milestone gates.
- [ ] Reconcile all 28 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

