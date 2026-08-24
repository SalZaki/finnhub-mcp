---
project: finnhub-mcp
document_type: epic-backlog
epic_id: E13
title: "Showcase Website and Safe Interactive Experience"
priority: P1
phase: "M3 — Scale & Ecosystem"
status: Not Started
baseline_commit: 2443648f220f0b4575b69c482425309e1e950f21
counts:
  features: 5
  user_stories: 10
  subtasks: 30
  traceability_owned: 11
  traceability_items: 19
story_estimate_days: 56
subtask_estimate_hours: 382
---

<a id="id-e13"></a>
# E13 — Showcase Website and Safe Interactive Experience

This is the self-contained coding-agent backlog for E13. It is one part of the E01–E15 Finnhub MCP programme and preserves the relevant slices of every workbook tab.

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
| E13 | P1 | 5 | 10 | 30 | 56 | 382 | M3 — Scale & Ecosystem | Not Started |

## 2. Epic Definition

**Objective:** Create a polished project site that demonstrates the product without exposing credentials or publishing an unsafe general-purpose MCP proxy.

**Business value:** Improves product comprehension and adoption while giving evaluators a low-friction, secure way to explore tools, workflows, and search ranking.

**Exit criteria:**

- [ ] The site ships the agreed core pages with responsive, accessible, fast, and search-indexable navigation.
- [ ] All 12 tools can be explored in deterministic fixture mode without a Finnhub key.
- [ ] Any live demo is isolated behind a hardened backend-for-frontend and cannot expose or accept a shared Finnhub credential in the browser.
- [ ] The search-ranking lab explains ranking evidence and supports baseline comparison without presenting relevance scores as probabilities.
- [ ] MCP Inspector guidance defaults to local use; any hosted instance is locked to an allowlisted demo server and protected by authentication.

## 3. Features

| Feature | Priority | Title | Story Count | Estimate Days | Status |
| --- | --- | --- | --- | --- | --- |
| [F13.01](#id-f13-01) | P1 | Website Platform and Experience Design | 2 | 11 | Not Started |
| [F13.02](#id-f13-02) | P1 | Tool Explorer, Workflow Demos, and Published Docs | 3 | 15 | Not Started |
| [F13.03](#id-f13-03) | P1 | Search Ranking Lab | 2 | 12 | Not Started |
| [F13.04](#id-f13-04) | P0 | Hardened Live Demo and Service Status | 2 | 15 | Not Started |
| [F13.05](#id-f13-05) | P1 | MCP Inspector Integration Strategy | 1 | 3 | Not Started |

<a id="id-f13-01"></a>
### F13.01 — Website Platform and Experience Design

- **Parent Epic:** [E13](#id-e13)
- **Priority:** P1
- **Status:** Not Started

**Description:** Establish the static-first site architecture, information hierarchy, design system, accessibility, performance, and hosting pipeline.

**Expected outcome:** The project has a fast, coherent, low-maintenance public home.

**Stories:**

- [US13.01.01](#id-us13-01-01) — Build a static-first Astro and Starlight site (P1, 5d)
- [US13.01.02](#id-us13-01-02) — Implement the complete site information architecture and design system (P1, 6d)

<a id="id-f13-02"></a>
### F13.02 — Tool Explorer, Workflow Demos, and Published Docs

- **Parent Epic:** [E13](#id-e13)
- **Priority:** P1
- **Status:** Not Started

**Description:** Demonstrate the complete MCP surface in deterministic fixture mode and publish searchable documentation from the repository.

**Expected outcome:** Visitors can understand inputs, outputs, costs, and workflows without installing the server or spending API quota.

**Stories:**

- [US13.02.01](#id-us13-02-01) — Create a fixture-backed explorer for all MCP surfaces (P1, 7d)
- [US13.02.02](#id-us13-02-02) — Demonstrate end-to-end research workflows (P1, 5d)
- [US13.02.03](#id-us13-02-03) — Publish searchable versioned documentation on the site (P1, 3d)

<a id="id-f13-03"></a>
### F13.03 — Search Ranking Lab

- **Parent Epic:** [E13](#id-e13)
- **Priority:** P1
- **Status:** Not Started

**Description:** Expose BM25 tokenization, matching, ranking, golden cases, and controlled feedback as an educational playground.

**Expected outcome:** Users and maintainers can explain and evaluate search-tools results instead of treating the score as opaque.

**Stories:**

- [US13.03.01](#id-us13-03-01) — Visualize search-tools tokenization and BM25 ranking (P1, 5d)
- [US13.03.02](#id-us13-03-02) — Compare ranking candidates and collect opt-in feedback (P2, 7d)

<a id="id-f13-04"></a>
### F13.04 — Hardened Live Demo and Service Status

- **Parent Epic:** [E13](#id-e13)
- **Priority:** P0
- **Status:** Not Started

**Description:** Isolate optional live Finnhub access behind policy-enforcing infrastructure with explicit budgets, bounds, observability, and legal review.

**Expected outcome:** A live demo, if enabled, is safe to operate and cannot leak credentials or become an unbounded data proxy.

**Stories:**

- [US13.04.01](#id-us13-04-01) — Implement a policy-enforcing live-demo backend (P0, 10d)
- [US13.04.02](#id-us13-04-02) — Operate live demo with budgets, graceful fallback, and status (P1, 5d)

<a id="id-f13-05"></a>
### F13.05 — MCP Inspector Integration Strategy

- **Parent Epic:** [E13](#id-e13)
- **Priority:** P1
- **Status:** Not Started

**Description:** Provide safe local Inspector usage and define strict controls for any hosted diagnostic experience.

**Expected outcome:** Users can inspect MCP behavior without exposing arbitrary connection or process-spawning capabilities on the public internet.

**Stories:**

- [US13.05.01](#id-us13-05-01) — Provide safe MCP Inspector workflows (P1, 3d)

## 4. User Stories and Subtasks

<a id="id-us13-01-01"></a>
### US13.01.01 — Build a static-first Astro and Starlight site

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.01](#id-f13-01) | P1 | 5 | 34 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Website maintainer

**User story:** As a maintainer, I want a low-maintenance static site with isolated interactive components so that documentation remains fast and deployable without the live service.

**Acceptance criteria:**

- [ ] The site uses Astro with Starlight for documentation and TypeScript/React islands only where interactivity is required.
- [ ] Static assets deploy to Cloudflare Pages or an equivalent preview-capable static host with separate preview and production environments.
- [ ] The repository contains formatting, type-checking, unit, accessibility, broken-link, and production-build commands executed in CI.
- [ ] Core pages meet defined budgets for JavaScript, image weight, and Core Web Vitals and work with JavaScript disabled except explicitly interactive labs.
- [ ] The live API origin, if present, is configured separately from the static site and is never bundled with a Finnhub credential.

**Dependencies:** [US12.01.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-02)

**Labels:** `website` `astro` `starlight` `cloudflare-pages`

**Source findings:**

- Recommended stack is Astro plus Starlight, TypeScript/React islands, and Cloudflare Pages for static hosting.
- Live infrastructure should be a separately hardened ASP.NET service rather than part of the browser bundle.

**Subtasks:**

<a id="id-st13-01-01-01"></a>
- [ ] **ST13.01.01.01 — Scaffold Astro, Starlight, and interactive islands**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Typed site workspace with docs, React island boundaries, and production build.
  - Status: Not Started
<a id="id-st13-01-01-02"></a>
- [ ] **ST13.01.01.02 — Configure preview and production hosting**
  - Type: devops
  - Estimate: 10 hours
  - Suggested owner role: Cloud engineer
  - Deliverable/evidence: Cloudflare Pages preview/production pipeline with isolated configuration.
  - Status: Not Started
<a id="id-st13-01-01-03"></a>
- [ ] **ST13.01.01.03 — Enforce web quality and performance budgets**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Frontend test engineer
  - Deliverable/evidence: CI type, format, unit, accessibility, link, bundle, and performance gates.
  - Status: Not Started

<a id="id-us13-01-02"></a>
### US13.01.02 — Implement the complete site information architecture and design system

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.01](#id-f13-01) | P1 | 6 | 40 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Prospective user

**User story:** As a prospective user, I want a coherent public site so that I can understand the value, evaluate capabilities, and find the correct next action quickly.

**Acceptance criteria:**

- [ ] Primary navigation includes Home/Quick Start, Tool Explorer, Playground, Workflows, Search Ranking Lab, Documentation, Architecture and Governance, Comparison, Blog/Changelog, Status, and Contribute.
- [ ] Home explains the project, self-hosted model, supported transports, 12-tool/3-resource/3-prompt surface, prerequisites, and a verified quick start without claiming hosted production readiness.
- [ ] A reusable visual system covers color, typography, spacing, code and JSON, financial tables, status, errors, charts, loading, empty states, and dark mode.
- [ ] All pages meet WCAG 2.2 AA keyboard, contrast, focus, semantic heading, reduced-motion, and screen-reader requirements.
- [ ] Metadata, sitemap, social cards, structured data, canonical URLs, privacy notice, and analytics consent are implemented and tested.

**Dependencies:** [US13.01.01](#id-us13-01-01), [US14.02.01](./E14-ecosystem-positioning-adoption-and-community.md#id-us14-02-01)

**Labels:** `website` `design-system` `accessibility` `seo`

**Source findings:**

- The showcase needs Home, feature demos, documentation, live demo, blog/changelog, workflows, ranking lab, architecture/governance, comparison, status, and contribution pages.

**Subtasks:**

<a id="id-st13-01-02-01"></a>
- [ ] **ST13.01.02.01 — Produce site map and content wireframes**
  - Type: ux-design
  - Estimate: 10 hours
  - Suggested owner role: Product designer
  - Deliverable/evidence: Approved responsive wireframes for all core pages and key user journeys.
  - Status: Not Started
<a id="id-st13-01-02-02"></a>
- [ ] **ST13.01.02.02 — Implement accessible visual system**
  - Type: implementation
  - Estimate: 20 hours
  - Suggested owner role: Design systems engineer
  - Deliverable/evidence: Reusable tokens and components for finance, code, errors, status, and dark mode.
  - Status: Not Started
<a id="id-st13-01-02-03"></a>
- [ ] **ST13.01.02.03 — Implement discovery, metadata, and privacy controls**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Navigation, metadata, sitemap, social cards, structured data, privacy notice, and consent controls.
  - Status: Not Started

<a id="id-us13-02-01"></a>
### US13.02.01 — Create a fixture-backed explorer for all MCP surfaces

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.02](#id-f13-02) | P1 | 7 | 48 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Technical evaluator

**User story:** As an evaluator, I want to exercise every tool with deterministic examples so that I can understand the contract without providing an API key.

**Acceptance criteria:**

- [ ] The explorer lists and runs fixture scenarios for all 12 tools and displays the 3 resources and 3 prompts.
- [ ] Each scenario exposes schema, parameters, selected view or fields, request JSON, response JSON, cache/freshness metadata, quota cost, approximate token estimate, errors, and next_actions.
- [ ] Fixtures cover success, validation error, not found, premium required, quota exceeded, partial or degraded data, truncation, and stale cache where applicable.
- [ ] Fixtures are versioned, sanitized, license-reviewed, and validated against application DTO schemas in CI.
- [ ] The UI clearly labels fixture data and never implies that displayed prices or status are current.

**Dependencies:** [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01), [US13.01.02](#id-us13-01-02)

**Labels:** `playground` `tools` `fixtures` `website`

**Source findings:**

- A feature demo should cover all 12 tools.
- The website should begin in fixture mode and not require or expose a Finnhub key.

**Subtasks:**

<a id="id-st13-02-01-01"></a>
- [ ] **ST13.02.01.01 — Design explorer states and fixture schema**
  - Type: design
  - Estimate: 8 hours
  - Suggested owner role: Product designer
  - Deliverable/evidence: Explorer UX and versioned fixture contract for success, failure, degraded, and truncated states.
  - Status: Not Started
<a id="id-st13-02-01-02"></a>
- [ ] **ST13.02.01.02 — Capture and sanitize complete fixture set**
  - Type: data-preparation
  - Estimate: 16 hours
  - Suggested owner role: API engineer
  - Deliverable/evidence: License-reviewed fixtures for 12 tools, 3 resources, 3 prompts, and key error cases.
  - Status: Not Started
<a id="id-st13-02-01-03"></a>
- [ ] **ST13.02.01.03 — Implement and contract-test explorer**
  - Type: implementation
  - Estimate: 24 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Interactive fixture explorer with schema, cost, tokens, freshness, errors, and next-actions views.
  - Status: Not Started

<a id="id-us13-02-02"></a>
### US13.02.02 — Demonstrate end-to-end research workflows

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.02](#id-f13-02) | P1 | 5 | 34 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Agent designer

**User story:** As an agent designer, I want guided workflows so that I can see how discovery, tools, resources, prompts, quota, and bounded outputs compose into useful research.

**Acceptance criteria:**

- [ ] The Workflows page demonstrates research-ticker, compare-peers, and news-pulse using corrected prompt logic and fixture responses.
- [ ] Each workflow visualizes tool order, parallelizable calls, cache hits, upstream-call budget, output-token budget, premium fallbacks, and stop conditions.
- [ ] The research workflow includes company profile; peer comparison requests only available metrics; news workflow separates article volume from sentiment.
- [ ] Users can change a bounded set of inputs and view the resulting plan without causing network calls in fixture mode.
- [ ] Every workflow links to relevant tool, prompt, quota, error, and data-accuracy reference pages.

**Dependencies:** [US13.02.01](#id-us13-02-01), [US12.02.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-02)

**Labels:** `workflows` `prompts` `quota` `context-engineering`

**Source findings:**

- Current research prompt omits profile, peer prompt asks for unavailable metrics, and news prompt conflates volume with sentiment.
- Workflow examples should make cost, cache, context, and fallback behavior visible.

**Subtasks:**

<a id="id-st13-02-02-01"></a>
- [ ] **ST13.02.02.01 — Correct and model three workflow plans**
  - Type: context-design
  - Estimate: 10 hours
  - Suggested owner role: LLM evaluation engineer
  - Deliverable/evidence: Versioned research, peer, and news workflows with valid tools and semantics.
  - Status: Not Started
<a id="id-st13-02-02-02"></a>
- [ ] **ST13.02.02.02 — Implement workflow visualization**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Interactive bounded workflow diagrams with call, cache, quota, token, fallback, and stop state.
  - Status: Not Started
<a id="id-st13-02-02-03"></a>
- [ ] **ST13.02.02.03 — Validate workflow links and budgets**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: Golden tests for valid tool arguments, reference links, quota counts, and output bounds.
  - Status: Not Started

<a id="id-us13-02-03"></a>
### US13.02.03 — Publish searchable versioned documentation on the site

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.02](#id-f13-02) | P1 | 3 | 22 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Documentation reader

**User story:** As a reader, I want the repository documentation published with search and version context so that I can find guidance matching my installed release.

**Acceptance criteria:**

- [ ] Starlight publishes the canonical repository docs without maintaining a manually duplicated web copy.
- [ ] Search indexes titles, headings, tool names, error codes, parameters, resources, prompts, and aliases and returns the applicable documentation version.
- [ ] Every API page links to the fixture explorer and every explorer scenario links back to the exact reference section.
- [ ] Preview builds annotate the pull request and production deploys only from a validated release or documentation commit.
- [ ] 404s offer relevant search results and version-switch guidance rather than silently redirecting to latest.

**Dependencies:** [US13.01.01](#id-us13-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01)

**Labels:** `documentation` `search` `versioning` `website`

**Source findings:**

- The site should contain the complete API reference and documentation, with one canonical source in the repository.

**Subtasks:**

<a id="id-st13-02-03-01"></a>
- [ ] **ST13.02.03.01 — Connect repository docs to Starlight**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Single-source versioned documentation build.
  - Status: Not Started
<a id="id-st13-02-03-02"></a>
- [ ] **ST13.02.03.02 — Configure semantic documentation search**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Version-aware index covering tools, parameters, errors, resources, prompts, and aliases.
  - Status: Not Started
<a id="id-st13-02-03-03"></a>
- [ ] **ST13.02.03.03 — Add reference/explorer links and deploy checks**
  - Type: test
  - Estimate: 6 hours
  - Suggested owner role: DevOps engineer
  - Deliverable/evidence: Bidirectional deep links, useful 404 behavior, preview annotations, and production-source gates.
  - Status: Not Started

<a id="id-us13-03-01"></a>
### US13.03.01 — Visualize search-tools tokenization and BM25 ranking

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.03](#id-f13-03) | P1 | 5 | 32 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** MCP user

**User story:** As an MCP user, I want to enter an intent and inspect ranked evidence so that I can understand why search-tools chose each tool.

**Acceptance criteria:**

- [ ] Users can enter a bounded intent and see normalized tokens, matched concepts, ranked tool names, descriptions, example intents, and raw relevance values.
- [ ] The UI labels the relevance value as a ranking score, not a probability or calibrated confidence.
- [ ] The lab runs from a versioned client-side index snapshot or a no-key allowlisted endpoint and makes no Finnhub calls.
- [ ] Input is length-bounded, control characters are rejected, and no query text is logged or persisted without explicit opt-in.
- [ ] The displayed catalogue version, tokenizer version, ranking parameters, and build commit are visible and shareable.

**Dependencies:** [US13.01.02](#id-us13-01-02)

**Labels:** `bm25` `search-tools` `playground` `explainability`

**Source findings:**

- An intent playground should show tokenization, ranking, raw relevance, and matched concepts.
- Current BM25 scores are uncalibrated and must not be presented as probabilities.

**Subtasks:**

<a id="id-st13-03-01-01"></a>
- [ ] **ST13.03.01.01 — Export versioned search index snapshot**
  - Type: implementation
  - Estimate: 8 hours
  - Suggested owner role: C# engineer
  - Deliverable/evidence: No-key catalogue/tokenizer/ranker snapshot with version metadata and golden checksum.
  - Status: Not Started
<a id="id-st13-03-01-02"></a>
- [ ] **ST13.03.01.02 — Implement ranking explanation UI**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Intent input, tokens, matched concepts, ranked results, raw score labels, and shareable configuration.
  - Status: Not Started
<a id="id-st13-03-01-03"></a>
- [ ] **ST13.03.01.03 — Add privacy and parity tests**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Security test engineer
  - Deliverable/evidence: Bounds/control-character/logging tests and parity suite against server search results.
  - Status: Not Started

<a id="id-us13-03-02"></a>
### US13.03.02 — Compare ranking candidates and collect opt-in feedback

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.03](#id-f13-03) | P2 | 7 | 52 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Search relevance maintainer

**User story:** As a relevance maintainer, I want baseline-versus-candidate experiments and labeled feedback so that improvements are evaluated offline instead of learned unsafely in production.

**Acceptance criteria:**

- [ ] The lab can compare two versioned ranking configurations against a 100-300 intent golden corpus and report tool@1, Recall@3, MRR or nDCG, zero-match rate, and confusion pairs.
- [ ] Golden cases include positive, negative, ambiguous, typo, symbol/company-name, and multilingual intents and are independently reviewed before merge.
- [ ] Shareable experiment links contain configuration and fixture identifiers, not raw private query text.
- [ ] Feedback is explicitly opt-in, stores the selected tool and consented query under a retention policy, and supports deletion and export.
- [ ] Feedback enters a moderated offline labeling queue and never changes production ranking automatically.

**Dependencies:** [US13.03.01](#id-us13-03-01)

**Labels:** `evaluation` `feedback` `privacy` `search-relevance`

**Source findings:**

- Search-tools has only a small ranking acceptance set and needs a 100-300 intent corpus with top-1, Recall@3, MRR/nDCG, zero-match, and confusion metrics.
- User feedback should be opt-in and moderated offline because online self-learning invites privacy leakage and poisoning.

**Subtasks:**

<a id="id-st13-03-02-01"></a>
- [ ] **ST13.03.02.01 — Curate and review ranking corpus**
  - Type: evaluation
  - Estimate: 20 hours
  - Suggested owner role: Search relevance engineer
  - Deliverable/evidence: Versioned 100-300 intent corpus with labels, splits, edge cases, and review record.
  - Status: Not Started
<a id="id-st13-03-02-02"></a>
- [ ] **ST13.03.02.02 — Build baseline-candidate evaluation view**
  - Type: implementation
  - Estimate: 16 hours
  - Suggested owner role: Frontend engineer
  - Deliverable/evidence: Comparison UI and metric computation for top-1, Recall@3, MRR/nDCG, no-match, and confusion.
  - Status: Not Started
<a id="id-st13-03-02-03"></a>
- [ ] **ST13.03.02.03 — Implement consented feedback pipeline**
  - Type: privacy-engineering
  - Estimate: 16 hours
  - Suggested owner role: Backend engineer
  - Deliverable/evidence: Opt-in, retention-bound, deletable feedback store and moderated offline labeling queue.
  - Status: Not Started

<a id="id-us13-04-01"></a>
### US13.04.01 — Implement a policy-enforcing live-demo backend

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.04](#id-f13-04) | P0 | 10 | 64 | M0 — Hardened Core | Not Started | Unassigned |

**Persona:** Demo operator

**User story:** As a demo operator, I want live Finnhub calls isolated behind a hardened backend so that browser users cannot extract credentials or turn the demo into an unbounded proxy.

**Acceptance criteria:**

- [ ] The browser never receives, stores, or submits a shared Finnhub key; the backend obtains it from an approved secret manager using workload identity or equivalent short-lived platform credentials.
- [ ] The ASP.NET backend requires demo authentication or abuse-resistant session proof and enforces exact Origin/Host allowlists, TLS, request-body limits, per-user and global rate/concurrency limits, and short deadlines.
- [ ] Only allowlisted tools, symbols/exchanges, parameter ranges, views, fields, and maximum result sizes can be invoked; arbitrary URL, MCP server, tool, or raw Finnhub proxying is impossible.
- [ ] The backend partitions cache and quota state appropriately, reserves budget for health and operators, redacts logs, records privacy-safe audit events, and returns bounded partial errors.
- [ ] Threat modeling, Finnhub licensing/terms review, penetration testing, and all platform P0 transport/authentication/redirect/error fixes are approved before the feature flag can enable live mode.

**Dependencies:** [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01), [US01.01.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-02), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US02.02.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-02-01), [US02.02.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-02-02), [US02.03.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-01), [US02.03.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-02), [US03.03.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-03-01), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US15.03.02](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-03-02)

**Labels:** `live-demo` `security` `backend-for-frontend` `p0`

**Source findings:**

- A shared Finnhub key must never be embedded in or accepted by the public browser client.
- A live site needs a hardened ASP.NET BFF with authentication, allowlists, cache, rate and output bounds, redacted logging, and licensing review.
- The existing hosted HTTP surface is not ready to expose as a public live demo.

**Subtasks:**

<a id="id-st13-04-01-01"></a>
- [ ] **ST13.04.01.01 — Threat-model live demo and approve policy**
  - Type: security-design
  - Estimate: 12 hours
  - Suggested owner role: Security architect
  - Deliverable/evidence: Approved threat model, allowlist policy, abuse cases, licensing review, and go-live gates.
  - Status: Not Started
<a id="id-st13-04-01-02"></a>
- [ ] **ST13.04.01.02 — Implement hardened ASP.NET demo BFF**
  - Type: implementation
  - Estimate: 32 hours
  - Suggested owner role: Senior backend engineer
  - Deliverable/evidence: Authenticated bounded proxy with secret-manager identity, exact origin/host, cache/quota partition, and audit controls.
  - Status: Not Started
<a id="id-st13-04-01-03"></a>
- [ ] **ST13.04.01.03 — Pen-test abuse and credential boundaries**
  - Type: security-test
  - Estimate: 20 hours
  - Suggested owner role: Application security engineer
  - Deliverable/evidence: Passing tests for key exfiltration, arbitrary proxying, auth bypass, origin abuse, limit bypass, SSRF, and log leakage.
  - Status: Not Started

<a id="id-us13-04-02"></a>
### US13.04.02 — Operate live demo with budgets, graceful fallback, and status

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.04](#id-f13-04) | P1 | 5 | 36 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** Demo visitor

**User story:** As a visitor, I want transparent service status and fixture fallback so that quota or upstream outages do not make the project appear broken or provide misleading current data.

**Acceptance criteria:**

- [ ] Live mode is separately feature-flagged by environment and automatically falls back to clearly labeled fixtures when disabled, budget-exhausted, circuit-open, or upstream-unavailable.
- [ ] A status page reports website, documentation, and live-demo availability plus freshness and known incidents without probing Finnhub in a quota-consuming loop.
- [ ] Dashboards and alerts cover allowed low-cardinality tool/outcome counts, error categories, latency, cache hit rate, limiter rejections, remaining demo budget, and circuit state without symbol, query, user, or key labels.
- [ ] Redis or another shared state store is introduced before horizontal scaling for global rate, budget, and session enforcement.
- [ ] The UI shows data source, as-of time, delay, cache age, fixture/live state, plan limitations, and a non-investment-advice notice.

**Dependencies:** [US13.04.01](#id-us13-04-01), [US13.01.02](#id-us13-01-02)

**Labels:** `operations` `status` `observability` `fallback`

**Source findings:**

- The site should start fixture-only and enable live data only after P0 hardening.
- Redis is appropriate if the demo scales horizontally.
- Status and metrics should not burn Finnhub quota or use high-cardinality financial/user labels.

**Subtasks:**

<a id="id-st13-04-02-01"></a>
- [ ] **ST13.04.02.01 — Implement live/fixture feature flag and fallback**
  - Type: implementation
  - Estimate: 12 hours
  - Suggested owner role: Full-stack engineer
  - Deliverable/evidence: Environment-isolated mode control with accurate UI labels and automatic fixture fallback.
  - Status: Not Started
<a id="id-st13-04-02-02"></a>
- [ ] **ST13.04.02.02 — Build low-cardinality observability and budget alerts**
  - Type: observability
  - Estimate: 14 hours
  - Suggested owner role: Site reliability engineer
  - Deliverable/evidence: Dashboards and alerts for availability, latency, cache, limits, quota budget, circuit, and redaction.
  - Status: Not Started
<a id="id-st13-04-02-03"></a>
- [ ] **ST13.04.02.03 — Publish status, freshness, and legal notices**
  - Type: implementation
  - Estimate: 10 hours
  - Suggested owner role: Web engineer
  - Deliverable/evidence: Quota-neutral status page and source/as-of/delay/cache/fixture/plan/non-advice UI.
  - Status: Not Started

<a id="id-us13-05-01"></a>
### US13.05.01 — Provide safe MCP Inspector workflows

| Feature | Priority | Estimate Days | Subtask Hours | Phase | Status | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| [F13.05](#id-f13-05) | P1 | 3 | 20 | M3 — Scale & Ecosystem | Not Started | Unassigned |

**Persona:** MCP developer

**User story:** As an MCP developer, I want a verified local Inspector workflow so that I can inspect the server without relying on a dangerous public generic proxy.

**Acceptance criteria:**

- [ ] Documentation provides pinned local npx and CLI Inspector commands for STDIO and the verified Streamable HTTP endpoint, including expected initialization and listing results.
- [ ] CI runs a pinned Inspector CLI or equivalent protocol client against an ephemeral server and archives only redacted diagnostics.
- [ ] The public site links to local instructions and a curated fixture explorer and does not expose an unauthenticated generic Inspector proxy.
- [ ] If a hosted Inspector is later approved, it is authenticated, session-isolated, network-egress restricted, unable to spawn arbitrary processes, and hard-wired to an allowlisted demo endpoint.
- [ ] A threat model and kill switch are required before any hosted Inspector route is deployed.

**Dependencies:** [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US13.02.01](#id-us13-02-01)

**Labels:** `mcp-inspector` `security` `developer-tools`

**Source findings:**

- A generic public MCP Inspector can connect to arbitrary servers or spawn processes and should not be exposed.
- Prefer local Inspector instructions and CI; a hosted version must be locked, authenticated, and allowlisted.

**Subtasks:**

<a id="id-st13-05-01-01"></a>
- [ ] **ST13.05.01.01 — Document pinned local Inspector use**
  - Type: documentation
  - Estimate: 6 hours
  - Suggested owner role: Developer advocate
  - Deliverable/evidence: Verified STDIO and HTTP Inspector commands and expected lifecycle output.
  - Status: Not Started
<a id="id-st13-05-01-02"></a>
- [ ] **ST13.05.01.02 — Add Inspector-compatible CI smoke**
  - Type: test
  - Estimate: 8 hours
  - Suggested owner role: Test engineer
  - Deliverable/evidence: Pinned local protocol inspection smoke with redacted artifacts.
  - Status: Not Started
<a id="id-st13-05-01-03"></a>
- [ ] **ST13.05.01.03 — Define hosted Inspector prohibitions and controls**
  - Type: security-design
  - Estimate: 6 hours
  - Suggested owner role: Security architect
  - Deliverable/evidence: Threat model and policy banning generic public proxying and specifying auth, isolation, egress, allowlist, and kill switch.
  - Status: Not Started

## 5. Subtask Index

| Subtask | Story | Priority | Title | Type | Hours | Owner Role | Deliverable / Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| [ST13.01.01.01](#id-st13-01-01-01) | [US13.01.01](#id-us13-01-01) | P1 | Scaffold Astro, Starlight, and interactive islands | implementation | 16 | Frontend engineer | Typed site workspace with docs, React island boundaries, and production build. | Not Started |
| [ST13.01.01.02](#id-st13-01-01-02) | [US13.01.01](#id-us13-01-01) | P1 | Configure preview and production hosting | devops | 10 | Cloud engineer | Cloudflare Pages preview/production pipeline with isolated configuration. | Not Started |
| [ST13.01.01.03](#id-st13-01-01-03) | [US13.01.01](#id-us13-01-01) | P1 | Enforce web quality and performance budgets | test | 8 | Frontend test engineer | CI type, format, unit, accessibility, link, bundle, and performance gates. | Not Started |
| [ST13.01.02.01](#id-st13-01-02-01) | [US13.01.02](#id-us13-01-02) | P1 | Produce site map and content wireframes | ux-design | 10 | Product designer | Approved responsive wireframes for all core pages and key user journeys. | Not Started |
| [ST13.01.02.02](#id-st13-01-02-02) | [US13.01.02](#id-us13-01-02) | P1 | Implement accessible visual system | implementation | 20 | Design systems engineer | Reusable tokens and components for finance, code, errors, status, and dark mode. | Not Started |
| [ST13.01.02.03](#id-st13-01-02-03) | [US13.01.02](#id-us13-01-02) | P1 | Implement discovery, metadata, and privacy controls | implementation | 10 | Web engineer | Navigation, metadata, sitemap, social cards, structured data, privacy notice, and consent controls. | Not Started |
| [ST13.02.01.01](#id-st13-02-01-01) | [US13.02.01](#id-us13-02-01) | P1 | Design explorer states and fixture schema | design | 8 | Product designer | Explorer UX and versioned fixture contract for success, failure, degraded, and truncated states. | Not Started |
| [ST13.02.01.02](#id-st13-02-01-02) | [US13.02.01](#id-us13-02-01) | P1 | Capture and sanitize complete fixture set | data-preparation | 16 | API engineer | License-reviewed fixtures for 12 tools, 3 resources, 3 prompts, and key error cases. | Not Started |
| [ST13.02.01.03](#id-st13-02-01-03) | [US13.02.01](#id-us13-02-01) | P1 | Implement and contract-test explorer | implementation | 24 | Frontend engineer | Interactive fixture explorer with schema, cost, tokens, freshness, errors, and next-actions views. | Not Started |
| [ST13.02.02.01](#id-st13-02-02-01) | [US13.02.02](#id-us13-02-02) | P1 | Correct and model three workflow plans | context-design | 10 | LLM evaluation engineer | Versioned research, peer, and news workflows with valid tools and semantics. | Not Started |
| [ST13.02.02.02](#id-st13-02-02-02) | [US13.02.02](#id-us13-02-02) | P1 | Implement workflow visualization | implementation | 16 | Frontend engineer | Interactive bounded workflow diagrams with call, cache, quota, token, fallback, and stop state. | Not Started |
| [ST13.02.02.03](#id-st13-02-02-03) | [US13.02.02](#id-us13-02-02) | P1 | Validate workflow links and budgets | test | 8 | Test engineer | Golden tests for valid tool arguments, reference links, quota counts, and output bounds. | Not Started |
| [ST13.02.03.01](#id-st13-02-03-01) | [US13.02.03](#id-us13-02-03) | P1 | Connect repository docs to Starlight | implementation | 8 | Web engineer | Single-source versioned documentation build. | Not Started |
| [ST13.02.03.02](#id-st13-02-03-02) | [US13.02.03](#id-us13-02-03) | P1 | Configure semantic documentation search | implementation | 8 | Frontend engineer | Version-aware index covering tools, parameters, errors, resources, prompts, and aliases. | Not Started |
| [ST13.02.03.03](#id-st13-02-03-03) | [US13.02.03](#id-us13-02-03) | P1 | Add reference/explorer links and deploy checks | test | 6 | DevOps engineer | Bidirectional deep links, useful 404 behavior, preview annotations, and production-source gates. | Not Started |
| [ST13.03.01.01](#id-st13-03-01-01) | [US13.03.01](#id-us13-03-01) | P1 | Export versioned search index snapshot | implementation | 8 | C# engineer | No-key catalogue/tokenizer/ranker snapshot with version metadata and golden checksum. | Not Started |
| [ST13.03.01.02](#id-st13-03-01-02) | [US13.03.01](#id-us13-03-01) | P1 | Implement ranking explanation UI | implementation | 16 | Frontend engineer | Intent input, tokens, matched concepts, ranked results, raw score labels, and shareable configuration. | Not Started |
| [ST13.03.01.03](#id-st13-03-01-03) | [US13.03.01](#id-us13-03-01) | P1 | Add privacy and parity tests | test | 8 | Security test engineer | Bounds/control-character/logging tests and parity suite against server search results. | Not Started |
| [ST13.03.02.01](#id-st13-03-02-01) | [US13.03.02](#id-us13-03-02) | P2 | Curate and review ranking corpus | evaluation | 20 | Search relevance engineer | Versioned 100-300 intent corpus with labels, splits, edge cases, and review record. | Not Started |
| [ST13.03.02.02](#id-st13-03-02-02) | [US13.03.02](#id-us13-03-02) | P2 | Build baseline-candidate evaluation view | implementation | 16 | Frontend engineer | Comparison UI and metric computation for top-1, Recall@3, MRR/nDCG, no-match, and confusion. | Not Started |
| [ST13.03.02.03](#id-st13-03-02-03) | [US13.03.02](#id-us13-03-02) | P2 | Implement consented feedback pipeline | privacy-engineering | 16 | Backend engineer | Opt-in, retention-bound, deletable feedback store and moderated offline labeling queue. | Not Started |
| [ST13.04.01.01](#id-st13-04-01-01) | [US13.04.01](#id-us13-04-01) | P0 | Threat-model live demo and approve policy | security-design | 12 | Security architect | Approved threat model, allowlist policy, abuse cases, licensing review, and go-live gates. | Not Started |
| [ST13.04.01.02](#id-st13-04-01-02) | [US13.04.01](#id-us13-04-01) | P0 | Implement hardened ASP.NET demo BFF | implementation | 32 | Senior backend engineer | Authenticated bounded proxy with secret-manager identity, exact origin/host, cache/quota partition, and audit controls. | Not Started |
| [ST13.04.01.03](#id-st13-04-01-03) | [US13.04.01](#id-us13-04-01) | P0 | Pen-test abuse and credential boundaries | security-test | 20 | Application security engineer | Passing tests for key exfiltration, arbitrary proxying, auth bypass, origin abuse, limit bypass, SSRF, and log leakage. | Not Started |
| [ST13.04.02.01](#id-st13-04-02-01) | [US13.04.02](#id-us13-04-02) | P1 | Implement live/fixture feature flag and fallback | implementation | 12 | Full-stack engineer | Environment-isolated mode control with accurate UI labels and automatic fixture fallback. | Not Started |
| [ST13.04.02.02](#id-st13-04-02-02) | [US13.04.02](#id-us13-04-02) | P1 | Build low-cardinality observability and budget alerts | observability | 14 | Site reliability engineer | Dashboards and alerts for availability, latency, cache, limits, quota budget, circuit, and redaction. | Not Started |
| [ST13.04.02.03](#id-st13-04-02-03) | [US13.04.02](#id-us13-04-02) | P1 | Publish status, freshness, and legal notices | implementation | 10 | Web engineer | Quota-neutral status page and source/as-of/delay/cache/fixture/plan/non-advice UI. | Not Started |
| [ST13.05.01.01](#id-st13-05-01-01) | [US13.05.01](#id-us13-05-01) | P1 | Document pinned local Inspector use | documentation | 6 | Developer advocate | Verified STDIO and HTTP Inspector commands and expected lifecycle output. | Not Started |
| [ST13.05.01.02](#id-st13-05-01-02) | [US13.05.01](#id-us13-05-01) | P1 | Add Inspector-compatible CI smoke | test | 8 | Test engineer | Pinned local protocol inspection smoke with redacted artifacts. | Not Started |
| [ST13.05.01.03](#id-st13-05-01-03) | [US13.05.01](#id-us13-05-01) | P1 | Define hosted Inspector prohibitions and controls | security-design | 6 | Security architect | Threat model and policy banning generic public proxying and specifying auth, isolation, egress, allowlist, and kill switch. | Not Started |

## 6. Relevant Traceability

Rows whose **Primary Epic** is E13 are canonically owned in this file. Rows owned by another Epic are duplicated here only as cross-Epic references because they cover a local Story.

| Trace ID | Dimension | Review Item / Finding | Covered Story IDs | Primary Epic | Priority | Coverage | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A-04 | A. Feature Enhancements | Define BM25 index rebuild rules and an opt-in, offline user-feedback learning loop with poisoning/privacy controls. | [US08.02.03](./E08-intelligent-discovery-and-context-engineering.md#id-us08-02-03), [US13.03.02](#id-us13-03-02) | [E08](./E08-intelligent-discovery-and-context-engineering.md#id-e08) | P1 | Covered | Explicit review question A4. |
| C-02 | C. User Experience | Provide a safe CLI and/or simple web explorer for tool discovery and end-user experimentation. | [US11.01.01](./E11-user-experience-performance-and-quota-control.md#id-us11-01-01), [US11.02.01](./E11-user-experience-performance-and-quota-control.md#id-us11-02-01), [US13.02.01](#id-us13-02-01) | [E11](./E11-user-experience-performance-and-quota-control.md#id-e11) | P1 | Covered | Explicit review question C2. |
| D-01 | D. Tools & Resources | Expose health/liveness/readiness safely for operators and an AI-readable service-status resource without quota-burning probes. | [US10.01.01](./E10-service-operations-resources-and-extensibility.md#id-us10-01-01), [US13.04.02](#id-us13-04-02) | [E10](./E10-service-operations-resources-and-extensibility.md#id-e10) | P1 | Covered | Explicit review question D1. |
| F-01 | F. Showcase Website | Build a showcase site with Home/Quick Start, all-tool explorer, complete docs, workflows, safe live demo, blog/changelog, status, architecture, comparison, and contribution pages. | [US13.01.02](#id-us13-01-02), [US13.02.01](#id-us13-02-01), [US13.02.02](#id-us13-02-02), [US13.02.03](#id-us13-02-03), [US13.04.02](#id-us13-04-02) | [E13](#id-e13) | P1 | Covered | Explicit review question F1. |
| F-02 | F. Showcase Website | Use a static-first site stack such as Astro/Starlight with isolated interactive islands and a separately hardened ASP.NET BFF for live calls. | [US13.01.01](#id-us13-01-01), [US13.04.01](#id-us13-04-01) | [E13](#id-e13) | P0 | Covered | Explicit review question F2. |
| F-03 | F. Showcase Website | Avoid a public generic MCP Inspector proxy; provide local Inspector instructions or a locked, authenticated, allowlisted deployment only. | [US13.05.01](#id-us13-05-01) | [E13](#id-e13) | P1 | Covered | Explicit review question F3. |
| F-04 | F. Showcase Website | Provide a search-tools ranking lab showing tokenization, matched concepts, raw scores, top-k, baseline/candidate comparisons, and opt-in feedback. | [US13.03.01](#id-us13-03-01), [US13.03.02](#id-us13-03-02) | [E13](#id-e13) | P1 | Covered | Explicit review question F4. |
| RF-144 | Code-review detail | Every tool needs exact parameters, schemas, examples, units, freshness, cost, cache, premium, error, pagination/truncation, and next-actions documentation. | [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01), [US13.02.01](#id-us13-02-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-145 | Code-review detail | Three resources and three prompts need complete reference and corrected workflow semantics. | [US12.02.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-02), [US13.02.02](#id-us13-02-02) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-146 | Code-review detail | Financial semantics for price volatility, insider signal, news windows/sentiment, provenance, and prompt claims require accuracy documentation. | [US12.02.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-02), [US13.02.02](#id-us13-02-02) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-148 | Code-review detail | Security, cloud-secret, transport, deployment, troubleshooting, and compatibility guidance are incomplete. | [US12.03.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-03-02), [US13.04.01](#id-us13-04-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-152 | Code-review detail | Short install/Inspector and architecture videos plus executable examples would improve onboarding. | [US12.05.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-05-02), [US13.05.01](#id-us13-05-01) | [E12](./E12-documentation-integrity-and-developer-enablement.md#id-e12) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-153 | Code-review detail | Recommended showcase stack is Astro/Starlight with React islands, static hosting, and a separate hardened ASP.NET live service. | [US13.01.01](#id-us13-01-01), [US13.04.01](#id-us13-04-01) | [E13](#id-e13) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-154 | Code-review detail | Showcase requires Home, explorer, playground, workflows, ranking lab, docs, architecture/governance, comparison, blog/changelog, status, and contribute pages. | [US13.01.02](#id-us13-01-02), [US13.02.01](#id-us13-02-01), [US13.02.02](#id-us13-02-02), [US13.02.03](#id-us13-02-03) | [E13](#id-e13) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-155 | Code-review detail | All 12 tools should be explorable without a Finnhub key using explicit fixture mode. | [US13.02.01](#id-us13-02-01) | [E13](#id-e13) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-156 | Code-review detail | A live demo must not put a shared API key in the browser and needs auth, allowlists, limits, cache, redaction, audit, and licensing controls. | [US13.04.01](#id-us13-04-01), [US13.04.02](#id-us13-04-02) | [E13](#id-e13) | P0 | Covered | Detailed finding retained from the repository review. |
| RF-157 | Code-review detail | MCP Inspector should default to local usage; a generic unauthenticated hosted proxy is unsafe. | [US13.05.01](#id-us13-05-01) | [E13](#id-e13) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-158 | Code-review detail | The search-ranking lab should expose tokens, matched concepts, raw rank scores, configuration, baseline/candidate comparison, and safe opt-in feedback. | [US13.03.01](#id-us13-03-01), [US13.03.02](#id-us13-03-02) | [E13](#id-e13) | P1 | Covered | Detailed finding retained from the repository review. |
| RF-159 | Code-review detail | Search ranking requires a larger labeled corpus and offline metrics; production self-learning risks poisoning and privacy leakage. | [US13.03.02](#id-us13-03-02) | [E13](#id-e13) | P2 | Covered | Detailed finding retained from the repository review. |

## 7. Relevant Roadmap Milestones

Calendar ranges assume a 4–6 person cross-functional team with overlapping workstreams and must be recalibrated against actual capacity.

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
| E13 | Epic | — | P1 | Showcase Website and Safe Interactive Experience | See [E13](#id-e13) | See [E13](#id-e13) | — | finnhub-mcp; epic | — | Not Started |
| F13.01 | Feature | [E13](#id-e13) | P1 | Website Platform and Experience Design | See [F13.01](#id-f13-01) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e13 | — | Not Started |
| F13.02 | Feature | [E13](#id-e13) | P1 | Tool Explorer, Workflow Demos, and Published Docs | See [F13.02](#id-f13-02) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e13 | — | Not Started |
| F13.03 | Feature | [E13](#id-e13) | P1 | Search Ranking Lab | See [F13.03](#id-f13-03) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e13 | — | Not Started |
| F13.04 | Feature | [E13](#id-e13) | P0 | Hardened Live Demo and Service Status | See [F13.04](#id-f13-04) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e13 | — | Not Started |
| F13.05 | Feature | [E13](#id-e13) | P1 | MCP Inspector Integration Strategy | See [F13.05](#id-f13-05) | Not applicable; see detail or parent section | — | finnhub-mcp; feature; e13 | — | Not Started |
| US13.01.01 | Story | [F13.01](#id-f13-01) | P1 | Build a static-first Astro and Starlight site | See [US13.01.01](#id-us13-01-01) | See [US13.01.01](#id-us13-01-01) | 5d | website; astro; starlight; cloudflare-pages | [US12.01.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-01-02) | Not Started |
| US13.01.02 | Story | [F13.01](#id-f13-01) | P1 | Implement the complete site information architecture and design system | See [US13.01.02](#id-us13-01-02) | See [US13.01.02](#id-us13-01-02) | 6d | website; design-system; accessibility; seo | [US13.01.01](#id-us13-01-01), [US14.02.01](./E14-ecosystem-positioning-adoption-and-community.md#id-us14-02-01) | Not Started |
| US13.02.01 | Story | [F13.02](#id-f13-02) | P1 | Create a fixture-backed explorer for all MCP surfaces | See [US13.02.01](#id-us13-02-01) | See [US13.02.01](#id-us13-02-01) | 7d | playground; tools; fixtures; website | [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01), [US13.01.02](#id-us13-01-02) | Not Started |
| US13.02.02 | Story | [F13.02](#id-f13-02) | P1 | Demonstrate end-to-end research workflows | See [US13.02.02](#id-us13-02-02) | See [US13.02.02](#id-us13-02-02) | 5d | workflows; prompts; quota; context-engineering | [US13.02.01](#id-us13-02-01), [US12.02.02](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-02) | Not Started |
| US13.02.03 | Story | [F13.02](#id-f13-02) | P1 | Publish searchable versioned documentation on the site | See [US13.02.03](#id-us13-02-03) | See [US13.02.03](#id-us13-02-03) | 3d | documentation; search; versioning; website | [US13.01.01](#id-us13-01-01), [US12.02.01](./E12-documentation-integrity-and-developer-enablement.md#id-us12-02-01) | Not Started |
| US13.03.01 | Story | [F13.03](#id-f13-03) | P1 | Visualize search-tools tokenization and BM25 ranking | See [US13.03.01](#id-us13-03-01) | See [US13.03.01](#id-us13-03-01) | 5d | bm25; search-tools; playground; explainability | [US13.01.02](#id-us13-01-02) | Not Started |
| US13.03.02 | Story | [F13.03](#id-f13-03) | P2 | Compare ranking candidates and collect opt-in feedback | See [US13.03.02](#id-us13-03-02) | See [US13.03.02](#id-us13-03-02) | 7d | evaluation; feedback; privacy; search-relevance | [US13.03.01](#id-us13-03-01) | Not Started |
| US13.04.01 | Story | [F13.04](#id-f13-04) | P0 | Implement a policy-enforcing live-demo backend | See [US13.04.01](#id-us13-04-01) | See [US13.04.01](#id-us13-04-01) | 10d | live-demo; security; backend-for-frontend; p0 | [US01.01.01](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-01), [US01.01.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-01-02), [US01.02.02](./E01-mcp-transport-and-protocol-integrity.md#id-us01-02-02), [US02.02.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-02-01), [US02.02.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-02-02), [US02.03.01](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-01), [US02.03.02](./E02-hosted-security-authorization-and-tenant-isolation.md#id-us02-03-02), [US03.03.01](./E03-credential-lifecycle-and-secret-containment.md#id-us03-03-01), [US04.03.02](./E04-resilience-quota-governance-and-cache-correctness.md#id-us04-03-02), [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US15.03.02](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-03-02) | Not Started |
| US13.04.02 | Story | [F13.04](#id-f13-04) | P1 | Operate live demo with budgets, graceful fallback, and status | See [US13.04.02](#id-us13-04-02) | See [US13.04.02](#id-us13-04-02) | 5d | operations; status; observability; fallback | [US13.04.01](#id-us13-04-01), [US13.01.02](#id-us13-01-02) | Not Started |
| US13.05.01 | Story | [F13.05](#id-f13-05) | P1 | Provide safe MCP Inspector workflows | See [US13.05.01](#id-us13-05-01) | See [US13.05.01](#id-us13-05-01) | 3d | mcp-inspector; security; developer-tools | [US15.02.01](./E15-sdk-modernization-and-software-supply-chain-assurance.md#id-us15-02-01), [US13.02.01](#id-us13-02-01) | Not Started |
| ST13.01.01.01 | Sub-task | [US13.01.01](#id-us13-01-01) | P1 | Scaffold Astro, Starlight, and interactive islands | See [ST13.01.01.01](#id-st13-01-01-01) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST13.01.01.02 | Sub-task | [US13.01.01](#id-us13-01-01) | P1 | Configure preview and production hosting | See [ST13.01.01.02](#id-st13-01-01-02) | Not applicable; see detail or parent section | 10h | finnhub-mcp; devops | — | Not Started |
| ST13.01.01.03 | Sub-task | [US13.01.01](#id-us13-01-01) | P1 | Enforce web quality and performance budgets | See [ST13.01.01.03](#id-st13-01-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST13.01.02.01 | Sub-task | [US13.01.02](#id-us13-01-02) | P1 | Produce site map and content wireframes | See [ST13.01.02.01](#id-st13-01-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; ux-design | — | Not Started |
| ST13.01.02.02 | Sub-task | [US13.01.02](#id-us13-01-02) | P1 | Implement accessible visual system | See [ST13.01.02.02](#id-st13-01-02-02) | Not applicable; see detail or parent section | 20h | finnhub-mcp; implementation | — | Not Started |
| ST13.01.02.03 | Sub-task | [US13.01.02](#id-us13-01-02) | P1 | Implement discovery, metadata, and privacy controls | See [ST13.01.02.03](#id-st13-01-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST13.02.01.01 | Sub-task | [US13.02.01](#id-us13-02-01) | P1 | Design explorer states and fixture schema | See [ST13.02.01.01](#id-st13-02-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; design | — | Not Started |
| ST13.02.01.02 | Sub-task | [US13.02.01](#id-us13-02-01) | P1 | Capture and sanitize complete fixture set | See [ST13.02.01.02](#id-st13-02-01-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; data-preparation | — | Not Started |
| ST13.02.01.03 | Sub-task | [US13.02.01](#id-us13-02-01) | P1 | Implement and contract-test explorer | See [ST13.02.01.03](#id-st13-02-01-03) | Not applicable; see detail or parent section | 24h | finnhub-mcp; implementation | — | Not Started |
| ST13.02.02.01 | Sub-task | [US13.02.02](#id-us13-02-02) | P1 | Correct and model three workflow plans | See [ST13.02.02.01](#id-st13-02-02-01) | Not applicable; see detail or parent section | 10h | finnhub-mcp; context-design | — | Not Started |
| ST13.02.02.02 | Sub-task | [US13.02.02](#id-us13-02-02) | P1 | Implement workflow visualization | See [ST13.02.02.02](#id-st13-02-02-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST13.02.02.03 | Sub-task | [US13.02.02](#id-us13-02-02) | P1 | Validate workflow links and budgets | See [ST13.02.02.03](#id-st13-02-02-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST13.02.03.01 | Sub-task | [US13.02.03](#id-us13-02-03) | P1 | Connect repository docs to Starlight | See [ST13.02.03.01](#id-st13-02-03-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST13.02.03.02 | Sub-task | [US13.02.03](#id-us13-02-03) | P1 | Configure semantic documentation search | See [ST13.02.03.02](#id-st13-02-03-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST13.02.03.03 | Sub-task | [US13.02.03](#id-us13-02-03) | P1 | Add reference/explorer links and deploy checks | See [ST13.02.03.03](#id-st13-02-03-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; test | — | Not Started |
| ST13.03.01.01 | Sub-task | [US13.03.01](#id-us13-03-01) | P1 | Export versioned search index snapshot | See [ST13.03.01.01](#id-st13-03-01-01) | Not applicable; see detail or parent section | 8h | finnhub-mcp; implementation | — | Not Started |
| ST13.03.01.02 | Sub-task | [US13.03.01](#id-us13-03-01) | P1 | Implement ranking explanation UI | See [ST13.03.01.02](#id-st13-03-01-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST13.03.01.03 | Sub-task | [US13.03.01](#id-us13-03-01) | P1 | Add privacy and parity tests | See [ST13.03.01.03](#id-st13-03-01-03) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST13.03.02.01 | Sub-task | [US13.03.02](#id-us13-03-02) | P2 | Curate and review ranking corpus | See [ST13.03.02.01](#id-st13-03-02-01) | Not applicable; see detail or parent section | 20h | finnhub-mcp; evaluation | — | Not Started |
| ST13.03.02.02 | Sub-task | [US13.03.02](#id-us13-03-02) | P2 | Build baseline-candidate evaluation view | See [ST13.03.02.02](#id-st13-03-02-02) | Not applicable; see detail or parent section | 16h | finnhub-mcp; implementation | — | Not Started |
| ST13.03.02.03 | Sub-task | [US13.03.02](#id-us13-03-02) | P2 | Implement consented feedback pipeline | See [ST13.03.02.03](#id-st13-03-02-03) | Not applicable; see detail or parent section | 16h | finnhub-mcp; privacy-engineering | — | Not Started |
| ST13.04.01.01 | Sub-task | [US13.04.01](#id-us13-04-01) | P0 | Threat-model live demo and approve policy | See [ST13.04.01.01](#id-st13-04-01-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; security-design | — | Not Started |
| ST13.04.01.02 | Sub-task | [US13.04.01](#id-us13-04-01) | P0 | Implement hardened ASP.NET demo BFF | See [ST13.04.01.02](#id-st13-04-01-02) | Not applicable; see detail or parent section | 32h | finnhub-mcp; implementation | — | Not Started |
| ST13.04.01.03 | Sub-task | [US13.04.01](#id-us13-04-01) | P0 | Pen-test abuse and credential boundaries | See [ST13.04.01.03](#id-st13-04-01-03) | Not applicable; see detail or parent section | 20h | finnhub-mcp; security-test | — | Not Started |
| ST13.04.02.01 | Sub-task | [US13.04.02](#id-us13-04-02) | P1 | Implement live/fixture feature flag and fallback | See [ST13.04.02.01](#id-st13-04-02-01) | Not applicable; see detail or parent section | 12h | finnhub-mcp; implementation | — | Not Started |
| ST13.04.02.02 | Sub-task | [US13.04.02](#id-us13-04-02) | P1 | Build low-cardinality observability and budget alerts | See [ST13.04.02.02](#id-st13-04-02-02) | Not applicable; see detail or parent section | 14h | finnhub-mcp; observability | — | Not Started |
| ST13.04.02.03 | Sub-task | [US13.04.02](#id-us13-04-02) | P1 | Publish status, freshness, and legal notices | See [ST13.04.02.03](#id-st13-04-02-03) | Not applicable; see detail or parent section | 10h | finnhub-mcp; implementation | — | Not Started |
| ST13.05.01.01 | Sub-task | [US13.05.01](#id-us13-05-01) | P1 | Document pinned local Inspector use | See [ST13.05.01.01](#id-st13-05-01-01) | Not applicable; see detail or parent section | 6h | finnhub-mcp; documentation | — | Not Started |
| ST13.05.01.02 | Sub-task | [US13.05.01](#id-us13-05-01) | P1 | Add Inspector-compatible CI smoke | See [ST13.05.01.02](#id-st13-05-01-02) | Not applicable; see detail or parent section | 8h | finnhub-mcp; test | — | Not Started |
| ST13.05.01.03 | Sub-task | [US13.05.01](#id-us13-05-01) | P1 | Define hosted Inspector prohibitions and controls | See [ST13.05.01.03](#id-st13-05-01-03) | Not applicable; see detail or parent section | 6h | finnhub-mcp; security-design | — | Not Started |

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

- [ ] Create E13 with its objective, business value, priority, phase, and exit criteria.
- [ ] Create all 5 Features under E13.
- [ ] Create all 10 User Stories with complete acceptance criteria and dependency links.
- [ ] Create all 30 Subtasks with hours, roles, and deliverables.
- [ ] Keep all 19 relevant traceability rows covered.
- [ ] Satisfy all 1 relevant roadmap milestone gates.
- [ ] Reconcile all 46 issue-import rows for this Epic.
- [ ] Apply the Delivery Guide and do not close the Epic while any required item is incomplete.

